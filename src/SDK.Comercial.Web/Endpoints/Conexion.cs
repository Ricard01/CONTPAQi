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
    [EndpointDescription("Verifica que el SDK esté iniciado y que la empresa configurada en ComercialSdk:RutaEmpresa esté abierta.")]
    public static async Task<IResult> VerificarConexion(IConexionComercial conexion, CancellationToken cancellationToken)
    {
        var resultado = await conexion.VerificarAsync(cancellationToken);

        return TypedResults.Ok(resultado);
    }
}
