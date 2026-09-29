using SDK.Comercial.Application.Conexion;

namespace SDK.Comercial.Web.Endpoints;

public static class ConexionEndpoints
{
    public static IEndpointRouteBuilder MapConexionEndpoints(this IEndpointRouteBuilder app)
    {
        // ?empresa=<ruta> para probar otra empresa; sin él se usa ComercialSdk:Empresa.
        app.MapGet("/api/conexion", async (IConexionComercial conexion, string? empresa, CancellationToken cancellationToken) =>
                TypedResults.Ok(await conexion.VerificarAsync(empresa, cancellationToken)))
            .WithTags("Conexión")
            .WithName("VerificarConexion");

        return app;
    }
}
