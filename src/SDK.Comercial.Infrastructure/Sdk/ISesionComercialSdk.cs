using SDK.Comercial.Application.Common.Exceptions;

namespace SDK.Comercial.Infrastructure.Sdk;

/// <summary>
/// Administra el ciclo de vida de la sesión global de CONTPAQi Comercial y su empresa activa.
/// Se mantiene separado de la cola para poder probar la ejecución serializada sin cargar la DLL nativa.
/// No debe crecer con operaciones de clientes, productos o documentos; esas pertenecen a sus repositorios.
/// </summary>
internal interface ISesionComercialSdk
{
    /// <summary>
    /// Proporciona las credenciales e inicializa la sesión global del SDK de Comercial Premium.
    /// Debe ejecutarse una sola vez antes de abrir empresas o realizar operaciones.
    /// </summary>
    /// <exception cref="ComercialSdkException">
    /// Se produce cuando la inicialización devuelve un código distinto de cero.
    /// </exception>
    void Iniciar();

    /// <summary>
    /// Termina la sesión global y libera los recursos del SDK. Debe ejecutarse durante el apagado
    /// después de cerrar la empresa activa.
    /// </summary>
    void Terminar();

    /// <summary>
    /// Abre la empresa ubicada en la ruta indicada y la establece como la empresa activa del SDK.
    /// </summary>
    /// <param name="ruta">Ruta completa del directorio de la empresa que se desea abrir.</param>
    /// <exception cref="ComercialSdkException">
    /// Se produce cuando la función nativa devuelve un código distinto de cero.
    /// </exception>
    void AbrirEmpresa(string ruta);

    /// <summary>
    /// Cierra la conexión con la empresa activa. La función nativa correspondiente no devuelve
    /// un código de resultado.
    /// </summary>
    void CerrarEmpresa();
}
