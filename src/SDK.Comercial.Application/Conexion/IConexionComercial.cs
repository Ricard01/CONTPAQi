namespace SDK.Comercial.Application.Conexion;

/// <summary>
/// Conexión con CONTPAQi Comercial: permite comprobar que el SDK está iniciado y que la empresa configurada está abierta.
/// </summary>
public interface IConexionComercial
{
    /// <summary>
    /// Abre la empresa configurada si aún no lo está y devuelve el estado.
    /// Lanza <see cref="Common.Exceptions.ComercialSdkException"/> si el SDK no inició o la empresa no se pudo abrir.
    /// </summary>
    Task<EstadoConexion> VerificarAsync(CancellationToken cancellationToken = default);
}

/// <param name="Empresa">Ruta de la empresa con la que trabaja la API.</param>
public sealed record EstadoConexion(string Empresa);
