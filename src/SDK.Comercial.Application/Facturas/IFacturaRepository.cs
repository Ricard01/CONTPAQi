namespace SDK.Comercial.Application.Facturas;

/// <summary>
/// Alta de facturas (documentos de venta con sus movimientos) en la empresa predeterminada de Comercial.
/// </summary>
public interface IFacturaRepository
{
    /// <summary>
    /// Inserta el documento y todos sus movimientos. Si algún movimiento falla, el documento se borra
    /// para no dejar una factura incompleta.
    /// Lanza <see cref="Common.Exceptions.ValidationException"/> si los datos son inválidos (sin llegar al SDK)
    /// y <see cref="Common.Exceptions.ComercialSdkException"/> si el SDK rechaza la operación.
    /// </summary>
    Task<FacturaCreada> CrearAsync(NuevaFactura factura, CancellationToken cancellationToken = default);
}
