using SDK.Comercial.Application.Common.Exceptions;

namespace SDK.Comercial.Application.Facturas;

/// <param name="CodigoConcepto">Concepto de documento de Comercial.</param>
/// <param name="Serie">Serie del documento; nula o vacía usa la serie configurada en el concepto.</param>
/// <param name="Fecha">Fecha del documento.</param>
/// <param name="CodigoCliente">Código del cliente.</param>
/// <param name="Referencia">Referencia opcional del documento.</param>
/// <param name="Movimientos">Partidas de la factura; debe haber al menos una.</param>
/// <param name="Observaciones">Observaciones opcionales del documento (<c>COBSERVACIONES</c>).</param>
/// <param name="TextoExtra1">Texto extra 1 opcional del documento (<c>CTEXTOEXTRA1</c>).</param>
/// <param name="TextoExtra2">Texto extra 2 opcional del documento (<c>CTEXTOEXTRA2</c>).</param>
/// <param name="TextoExtra3">Texto extra 3 opcional del documento (<c>CTEXTOEXTRA3</c>).</param>
/// <remarks>Los importes y porcentaje de impuestos y retenciones se calculan con base a la configuracion del concepto. </remarks>
public sealed record NuevaFactura(
    string CodigoConcepto,
    string? Serie,
    DateOnly Fecha,
    string CodigoCliente,
    string? Referencia,
    IReadOnlyList<NuevoMovimiento> Movimientos,
    string? Observaciones = null,
    string? TextoExtra1 = null,
    string? TextoExtra2 = null,
    string? TextoExtra3 = null)
{
    /// <summary>Lanza <see cref="ValidationException"/> con todos los errores encontrados.</summary>
    public void Validar()
    {
        var errores = new Dictionary<string, string[]>();
        if (string.IsNullOrWhiteSpace(CodigoConcepto))
        {
            errores[nameof(CodigoConcepto)] = ["El concepto es obligatorio."];
        }

        if (string.IsNullOrWhiteSpace(CodigoCliente))
        {
            errores[nameof(CodigoCliente)] = ["El cliente es obligatorio."];
        }

        if (Movimientos.Count == 0)
        {
            errores[nameof(Movimientos)] = ["La factura debe tener al menos un movimiento."];
        }
        else
        {
            for (var i = 0; i < Movimientos.Count; i++)
            {
                var m = Movimientos[i];
                var mensajes = new List<string>();
                if (string.IsNullOrWhiteSpace(m.CodigoProducto))
                {
                    mensajes.Add("El producto es obligatorio.");
                }

                if (string.IsNullOrWhiteSpace(m.CodigoAlmacen))
                {
                    mensajes.Add("El almacén es obligatorio.");
                }

                if (m.Unidades <= 0)
                {
                    mensajes.Add("Las unidades deben ser mayores que cero.");
                }

                if (m.Precio < 0)
                {
                    mensajes.Add("El precio no puede ser negativo.");
                }

                if (m.PorcentajeDescuento is not null && m.ImporteDescuento is not null)
                {
                    mensajes.Add("Indique el descuento como porcentaje o como importe, no ambos.");
                }

                if (m.PorcentajeDescuento is < 0 or > 100)
                {
                    mensajes.Add("El porcentaje de descuento debe estar entre 0 y 100.");
                }

                if (m.ImporteDescuento < 0)
                {
                    mensajes.Add("El importe de descuento no puede ser negativo.");
                }
                else if (m.ImporteDescuento > m.Unidades * m.Precio)
                {
                    mensajes.Add("El importe de descuento no puede ser mayor que el importe del movimiento.");
                }
                

                if (mensajes.Count > 0)
                {
                    errores[$"{nameof(Movimientos)}[{i}]"] = [.. mensajes];
                }
            }
        }

        if (errores.Count > 0)
        {
            throw new ValidationException(errores);
        }
    }
}

/// <param name="CodigoProducto">Código del producto o servicio.</param>
/// <param name="CodigoAlmacen">Código del almacén de donde sale el producto.</param>
/// <param name="Unidades">Cantidad vendida.</param>
/// <param name="Precio">Precio unitario.</param>
/// <param name="Observaciones">Observaciones opcionales del movimiento (<c>COBSERVAMOV</c>).</param>
/// <param name="PorcentajeDescuento">Porcentaje opcional del descuento 1; excluyente con <paramref name="ImporteDescuento"/>.</param>
/// <param name="ImporteDescuento">Importe opcional del descuento 1; excluyente con <paramref name="PorcentajeDescuento"/>.</param>
public sealed record NuevoMovimiento(
    string CodigoProducto,
    string CodigoAlmacen,
    double Unidades,
    double Precio,
    string? Observaciones = null,
    double? PorcentajeDescuento = null,
    double? ImporteDescuento = null);

/// <param name="IdDocumento">Identificador interno asignado por Comercial.</param>
/// <param name="CodigoConcepto">Concepto con el que se creó.</param>
/// <param name="Serie">Serie resuelta por el SDK.</param>
/// <param name="Folio">Folio asignado.</param>
/// <param name="IdMovimientos">Identificadores de los movimientos, en el mismo orden que se enviaron.</param>
public sealed record FacturaCreada(
    int IdDocumento,
    string CodigoConcepto,
    string Serie,
    double Folio,
    IReadOnlyList<int> IdMovimientos);
