using SDK.Comercial.Application.Conexion;
using SDK.Comercial.Web.Infrastructure;

namespace SDK.Comercial.Web.Endpoints;

public  class Conexion : IEndpointGroup
{
    
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(VerificarConexion);
    }

    [EndpointSummary("Verificar conexión con CONTPAQi Comercial")]
    [EndpointDescription("Verifica que sea posible establecer una conexión con CONTPAQi Comercial. " +
                         "Opcionalmente se puede especificar una empresa mediante el parámetro 'empresa'.")]
    public static async Task<IResult> VerificarConexion(IConexionComercial conexion, string? empresa, CancellationToken cancellationToken)
    {
        var resultado = await conexion.VerificarAsync(empresa, cancellationToken);

        return TypedResults.Ok(resultado);
    }
}
