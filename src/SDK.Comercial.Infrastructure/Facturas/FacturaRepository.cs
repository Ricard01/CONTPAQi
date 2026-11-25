using System.Globalization;
using Microsoft.Extensions.Logging;
using Ardalis.GuardClauses;
using SDK.Comercial.Application.Common.Exceptions;
using SDK.Comercial.Application.Facturas;
using SDK.Comercial.Infrastructure.Sdk;
using SDK.Comercial.Infrastructure.Sdk.Cola;
using SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

namespace SDK.Comercial.Infrastructure.Facturas;

/// <summary>
/// Crea facturas con las funciones de alto nivel del SDK: folio → encabezado → movimientos, y edita después
/// los campos que esas estructuras no incluyen (observaciones y textos extra).
/// Todo ocurre dentro de una sola operación de la cola, así ningún otro trabajo puede intercalarse
/// entre el alta del documento y la de sus movimientos.
/// </summary>
internal sealed class FacturaRepository(SdkColaTrabajo cola, IDocumentosSdk documentos, ILogger<FacturaRepository> logger) : IFacturaRepository
{
    private const int SistemaOrigen = 205;
    private const int MonedaPesos = 1;
    private const int TipoDocumentoFactura = 4;

    public Task<FacturaCreada> CrearAsync(NuevaFactura factura, CancellationToken cancellationToken = default)
    {
        // Los datos inválidos se rechazan antes de ocupar un turno en la cola.
        factura.Validar();
        return cola.EncolarAsync(contexto => Crear(contexto, factura), cancellationToken);
    }

    private FacturaCreada Crear(SdkContexto contexto, NuevaFactura factura)
    {
        contexto.UsarEmpresa();
        try
        {
            return CrearDocumento(factura);
        }
        finally
        {
            // Mientras la sesión del SDK siga con la empresa abierta, Comercial reporta el último documento
            // que tocó como "en uso por otro usuario". Cerrarla lo libera; el siguiente trabajo la reabre con UsarEmpresa().
            contexto.CerrarEmpresa();
        }
    }

    private FacturaCreada CrearDocumento(NuevaFactura factura)
    {
        var (serie, folio) = documentos.SiguienteFolio(factura.CodigoConcepto);
        var idDocumento = documentos.AltaDocumento(new tDocumento
        {
            aFolio = folio,
            aNumMoneda = MonedaPesos,
            aTipoCambio = 1,
            aSistemaOrigen = SistemaOrigen,
            aCodConcepto = factura.CodigoConcepto,
            aSerie = serie,
            aFecha = factura.Fecha.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
            aCodigoCteProv = factura.CodigoCliente,
            aCodigoAgente = factura.CodigoAgente ?? string.Empty,
            aReferencia = factura.Referencia ?? string.Empty,
        });

        var idMovimientos = new List<int>(factura.Movimientos.Count);
        try
        {
            for (var i = 0; i < factura.Movimientos.Count; i++)
            {
                var m = factura.Movimientos[i];
                var idMovimiento = documentos.AltaMovimiento(idDocumento, new tMovimientoDesc
                {
                    aConsecutivo = i + 1,
                    aUnidades = m.Unidades,
                    aPrecio = m.Precio,
                    aPorcDescto1 = m.PorcentajeDescuento ?? 0,
                    aImporteDescto1 = m.ImporteDescuento ?? 0,
                    aCodProdSer = m.CodigoProducto,
                    aCodAlmacen = m.CodigoAlmacen,
                    aReferencia = string.Empty,
                    aCodClasificacion = string.Empty,
                });
                idMovimientos.Add(idMovimiento);

                // La estructura del alta no trae observaciones: se escriben editando el movimiento.
                var camposMovimiento = CamposMovimiento(m);
                if (camposMovimiento.Count > 0)
                {
                    documentos.ActualizarMovimiento(idMovimiento, camposMovimiento);
                }
            }

            var camposDocumento = CamposDocumento(factura);
            if (camposDocumento.Count > 0)
            {
                documentos.ActualizarDocumento(idDocumento, camposDocumento);
            }
        }
        catch
        {
            // Sin transacciones en el SDK: se compensa borrando el encabezado para no dejar un documento a medias.
            DescartarDocumento(idDocumento);
            throw;
        }

        return new FacturaCreada(idDocumento, factura.CodigoConcepto, serie, folio, idMovimientos);
    }

    private static List<(string Campo, string Valor)> CamposMovimiento(NuevoMovimiento movimiento)
    {
        var campos = new List<(string Campo, string Valor)>();
        AgregarCampo(campos, "COBSERVAMOV", movimiento.Observaciones);
        AgregarCampo(campos, "CTEXTOEXTRA1", movimiento.TextoExtra1);
        AgregarCampo(campos, "CTEXTOEXTRA2", movimiento.TextoExtra2);
        AgregarCampo(campos, "CTEXTOEXTRA3", movimiento.TextoExtra3);

        return campos;
    }

    private static List<(string Campo, string Valor)> CamposDocumento(NuevaFactura factura)
    {
        var campos = new List<(string Campo, string Valor)>();
        AgregarCampo(campos, "COBSERVACIONES", factura.Observaciones);
        AgregarCampo(campos, "CTEXTOEXTRA1", factura.TextoExtra1);
        AgregarCampo(campos, "CTEXTOEXTRA2", factura.TextoExtra2);
        AgregarCampo(campos, "CTEXTOEXTRA3", factura.TextoExtra3);
        AgregarCampo(campos, "CDESTINATARIO", factura.Destinatario);

        return campos;
    }

    private static void AgregarCampo(List<(string Campo, string Valor)> campos, string campo, string? valor)
    {
        if (!string.IsNullOrEmpty(valor))
        {
            campos.Add((campo, valor));
        }
    }

    public Task BorrarAsync(int idDocumento, CancellationToken cancellationToken = default)
    {
        return cola.EncolarAsync(contexto =>
        {
            contexto.UsarEmpresa();
            try
            {
                var datos = documentos.LeerDocumento(idDocumento);
                if (datos is null || datos.TipoDocumento != TipoDocumentoFactura)
                {
                    throw new NotFoundException(idDocumento.ToString(CultureInfo.InvariantCulture), "Factura");
                }

                // El tipo se revisa antes: fDocumentoUUID solo responde para documentos con folio digital.
                var uuid = documentos.ObtenerUuid(datos.CodigoConcepto, datos.Serie, datos.Folio);
                if (!string.IsNullOrEmpty(uuid))
                {
                    throw new ValidationException(new Dictionary<string, string[]>
                    {
                        [nameof(idDocumento)] = [$"La factura {datos.Serie}-{datos.Folio} ya está timbrada (UUID {uuid}) y no se puede borrar."],
                    });
                }

                documentos.BorrarDocumento(idDocumento);
                logger.LogInformation("Factura {Serie}-{Folio} (id {IdDocumento}) borrada", datos.Serie, datos.Folio, idDocumento);
                return 0;
            }
            finally
            {
                contexto.CerrarEmpresa();
            }
        }, cancellationToken);
    }

    private void DescartarDocumento(int idDocumento)
    {
        try
        {
            documentos.BorrarDocumento(idDocumento);
        }
        catch (Exception ex)
        {
            // El error original es el que importa al llamador; este solo se registra.
            logger.LogError(ex, "No se pudo borrar el documento incompleto {IdDocumento}", idDocumento);
        }
    }
}
