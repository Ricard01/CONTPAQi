namespace SDK.Comercial.Application.Conexion;

/// <summary>
/// Conexión con CONTPAQi Comercial: permite comprobar que el SDK está iniciado y que una empresa se puede abrir.
/// </summary>
public interface IConexionComercial
{
    /// <summary>
    /// Abre la empresa indicada (o la predeterminada si <paramref name="rutaEmpresa"/> es nula) y devuelve el estado.
    /// Lanza <see cref="Common.Exceptions.ComercialSdkException"/> si el SDK no inició o la empresa no se pudo abrir.
    /// </summary>
    Task<EstadoConexion> VerificarAsync(string? rutaEmpresa = null, CancellationToken cancellationToken = default);
}

/// <param name="EmpresaAbierta">Ruta de la empresa que quedó abierta en el SDK.</param>
public sealed record EstadoConexion(string EmpresaAbierta);
