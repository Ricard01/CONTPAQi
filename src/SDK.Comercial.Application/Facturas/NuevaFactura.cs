using SDK.Comercial.Application.Common.Exceptions;

namespace SDK.Comercial.Application.Facturas;

/// <param name="CodigoConcepto">Concepto de documento de Comercial (p. ej. el de "Factura").</param>
/// <param name="Serie">Serie del documento; nula o vacía usa la serie configurada en el concepto.</param>
/// <param name="Fecha">Fecha del documento.</param>
/// <param name="CodigoCliente">Código del cliente.</param>
/// <param name="Referencia">Referencia opcional del documento.</param>
/// <param name="Movimientos">Partidas de la factura; debe haber al menos una.</param>
public sealed record NuevaFactura(
    string CodigoConcepto,
    string? Serie,
    DateOnly Fecha,
    string CodigoCliente,
    string? Referencia,
    IReadOnlyList<NuevoMovimiento> Movimientos)
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

        if (Movimientos is null || Movimientos.Count == 0)
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
/// <param name="Referencia">Referencia opcional del movimiento.</param>
public sealed record NuevoMovimiento(
    string CodigoProducto,
    string CodigoAlmacen,
    double Unidades,
    double Precio,
    string? Referencia = null);

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
