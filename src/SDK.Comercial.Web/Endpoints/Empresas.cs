using SDK.Comercial.Application.Empresas;
using SDK.Comercial.Web.Infrastructure;

namespace SDK.Comercial.Web.Endpoints;

public  class Empresas : IEndpointGroup
{
    
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetEmpresas);
    }

    [EndpointSummary("Obtiene una lista de empresas")]
    [EndpointDescription("Obtiene la lista de empresas de CONTPAQi Comercial.")]
    public static async Task<IResult> GetEmpresas(IEmpresaRepository empresas, CancellationToken cancellationToken)
    {
        var resultado = await empresas.ListarAsync(cancellationToken);

        return TypedResults.Ok(resultado);
    }
}
