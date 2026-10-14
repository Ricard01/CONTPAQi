using SDK.Comercial.Application.Conexion;
using SDK.Comercial.Infrastructure.Sdk.Cola;

namespace SDK.Comercial.Infrastructure.Conexion;

internal sealed class ConexionComercial(SdkColaTrabajo cola) : IConexionComercial
{
    public Task<EstadoConexion> VerificarAsync(CancellationToken cancellationToken = default) =>
        cola.EncolarAsync(contexto =>
        {
            contexto.UsarEmpresa();
            return new EstadoConexion(contexto.RutaEmpresa);
        }, cancellationToken);
}
