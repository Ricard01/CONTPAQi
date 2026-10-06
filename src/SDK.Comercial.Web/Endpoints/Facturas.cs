using SDK.Comercial.Application.Facturas;
using SDK.Comercial.Web.Infrastructure;

namespace SDK.Comercial.Web.Endpoints;

public class Facturas : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CrearFactura);
    }

    [EndpointSummary("Crea una factura")]
    [EndpointDescription("Da de alta en la empresa predeterminada un documento con sus movimientos. " +
                         "El folio lo asigna el SDK con el siguiente disponible del concepto y serie.")]
    public static async Task<IResult> CrearFactura(IFacturaRepository facturas, NuevaFactura factura, CancellationToken cancellationToken)
    {
        var resultado = await facturas.CrearAsync(factura, cancellationToken);

        return TypedResults.Ok(resultado);
    }
}
