using SDK.Comercial.Application.Conexion;
using SDK.Comercial.Infrastructure.Sdk.Cola;

namespace SDK.Comercial.Infrastructure.Conexion;

internal sealed class ConexionComercial(SdkColaTrabajo cola) : IConexionComercial
{
    public Task<EstadoConexion> VerificarAsync(string? rutaEmpresa = null, CancellationToken cancellationToken = default) =>
        cola.EncolarAsync(contexto =>
        {
            if (string.IsNullOrWhiteSpace(rutaEmpresa))
            {
                contexto.UsarEmpresa();
            }
            else
            {
                contexto.UsarEmpresa(rutaEmpresa);
            }

            return new EstadoConexion(contexto.EmpresaAbierta!);
        }, cancellationToken);
}
