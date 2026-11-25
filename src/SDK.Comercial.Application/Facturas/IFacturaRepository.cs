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

    /// <summary>
    /// Borra una factura que todavía no está timbrada. Pensado para deshacer una factura recién creada
    /// cuando el paso siguiente del llamador falla (por ejemplo, ligarla a su cotización).
    /// Lanza <see cref="NotFoundException"/> si el documento no es una factura,
    /// <see cref="Common.Exceptions.ValidationException"/> si ya está timbrada
    /// y <see cref="Common.Exceptions.ComercialSdkException"/> si el SDK rechaza la operación.
    /// </summary>
    Task BorrarAsync(int idDocumento, CancellationToken cancellationToken = default);
}
