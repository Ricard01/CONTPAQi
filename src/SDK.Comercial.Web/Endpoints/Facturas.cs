using SDK.Comercial.Application.Facturas;
using SDK.Comercial.Web.Infrastructure;

namespace SDK.Comercial.Web.Endpoints;

public class Facturas : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CrearFactura);
        groupBuilder.MapDelete(BorrarFactura, "{idDocumento:int}");
    }

    [EndpointSummary("Borra una factura sin timbrar")]
    [EndpointDescription("Borra de la empresa predeterminada una factura que todavía no está timbrada. " +
                         "Sirve para deshacer una factura recién creada cuando el paso siguiente del cliente falla. " +
                         "Responde 404 si el documento no es una factura y 400 si ya está timbrada.")]
    public static async Task<IResult> BorrarFactura(IFacturaRepository facturas, int idDocumento, CancellationToken cancellationToken)
    {
        await facturas.BorrarAsync(idDocumento, cancellationToken);

        return TypedResults.NoContent();
    }

    [EndpointSummary("Crea una factura")]
    [EndpointDescription("Da de alta en la empresa predeterminada un documento con sus movimientos. " +
                         "La serie y el folio los asigna el SDK con el siguiente disponible del concepto.")]
    public static async Task<IResult> CrearFactura(IFacturaRepository facturas, NuevaFactura factura, CancellationToken cancellationToken)
    {
        var resultado = await facturas.CrearAsync(factura, cancellationToken);

        return TypedResults.Ok(resultado);
    }
}
