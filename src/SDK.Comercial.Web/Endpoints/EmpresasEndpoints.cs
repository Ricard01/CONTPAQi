using SDK.Comercial.Application.Empresas;

namespace SDK.Comercial.Web.Endpoints;

public static class EmpresasEndpoints
{
    public static IEndpointRouteBuilder MapEmpresasEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/empresas").WithTags("Empresas");

        grupo.MapGet("/", async (IEmpresaRepository empresas, CancellationToken cancellationToken) =>
                TypedResults.Ok(await empresas.ListarAsync(cancellationToken)))
            .WithName("ListarEmpresas");

        return app;
    }
}
