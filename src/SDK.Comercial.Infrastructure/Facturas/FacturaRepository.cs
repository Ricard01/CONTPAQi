using System.Globalization;
using Microsoft.Extensions.Logging;
using SDK.Comercial.Application.Facturas;
using SDK.Comercial.Infrastructure.Sdk;
using SDK.Comercial.Infrastructure.Sdk.Cola;
using SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

namespace SDK.Comercial.Infrastructure.Facturas;

/// <summary>
/// Crea facturas con las funciones de alto nivel del SDK: folio → encabezado → movimientos, y edita después
/// los campos que esas estructuras no incluyen (observaciones, textos extra y retenciones).
/// Todo ocurre dentro de una sola operación de la cola, así ningún otro trabajo puede intercalarse
/// entre el alta del documento y la de sus movimientos.
/// </summary>
internal sealed class FacturaRepository(SdkColaTrabajo cola, IDocumentosSdk documentos, ILogger<FacturaRepository> logger) : IFacturaRepository
{
    private const int SistemaOrigen = 205;
    private const int MonedaPesos = 1;

    public Task<FacturaCreada> CrearAsync(NuevaFactura factura, CancellationToken cancellationToken = default)
    {
        // Los datos inválidos se rechazan antes de ocupar un turno en la cola.
        factura.Validar();
        return cola.EncolarAsync(contexto => Crear(contexto, factura), cancellationToken);
    }

    private FacturaCreada Crear(SdkContexto contexto, NuevaFactura factura)
    {
        contexto.UsarEmpresa();

        var (serie, folio) = documentos.SiguienteFolio(factura.CodigoConcepto, factura.Serie ?? string.Empty);
        var idDocumento = documentos.AltaDocumento(new tDocumento
        {
            aFolio = folio,
            aNumMoneda = MonedaPesos,
            aTipoCambio = 1,
            aSistemaOrigen = SistemaOrigen,
            aCodConcepto = factura.CodigoConcepto,
            aSerie = serie,
            // tDocumento.aFecha se interpreta como mm/dd/aaaa, sin depender de la cultura del servidor.
            aFecha = factura.Fecha.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
            aCodigoCteProv = factura.CodigoCliente,
            aCodigoAgente = string.Empty,
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

                // La estructura del alta no trae observaciones ni retenciones: se escriben editando el movimiento.
                var camposMovimiento = CamposMovimiento(m);
                if (camposMovimiento.Count > 0)
                {
                    documentos.ActualizarMovimiento(idMovimiento, camposMovimiento);
                }
            }

            // Al final, para que la retención indicada no la sobrescriba el recálculo de los movimientos.
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
        if (!string.IsNullOrEmpty(movimiento.Observaciones))
        {
            campos.Add(("COBSERVAMOV", movimiento.Observaciones));
        }

        return campos;
    }

    private static List<(string Campo, string Valor)> CamposDocumento(NuevaFactura factura)
    {
        var campos = new List<(string Campo, string Valor)>();
        Agregar("COBSERVACIONES", factura.Observaciones);
        Agregar("CTEXTOEXTRA1", factura.TextoExtra1);
        Agregar("CTEXTOEXTRA2", factura.TextoExtra2);
        Agregar("CTEXTOEXTRA3", factura.TextoExtra3);

        return campos;

        void Agregar(string campo, string? valor)
        {
            if (!string.IsNullOrEmpty(valor))
            {
                campos.Add((campo, valor));
            }
        }
    }

    // fSetDato* recibe texto: siempre con punto decimal, sin depender de la cultura del servidor.
    private static string Numero(double valor) => valor.ToString(CultureInfo.InvariantCulture);

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
