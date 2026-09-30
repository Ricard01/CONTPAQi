namespace SDK.Comercial.Infrastructure.Sdk;

public sealed class ComercialSdkOptions
{
    /// <summary>Nombre de la sección que se lee desde appsettings, variables de entorno u otra fuente.</summary>
    public const string Seccion = "ComercialSdk";

    /// <summary>
    /// Carpeta de instalación de Comercial Premium (donde está MGWServicios.dll).
    /// Si se deja vacía se lee del registro de Windows.
    /// </summary>
    public string? RutaInstalacion { get; set; }

    /// <summary>
    /// Usuario de CONTPAQi Comercial que se entrega a <c>fInicioSesionSDK</c> antes de llamar a
    /// <c>fInicializaSDK</c>. Se usa SUPERVISOR por omisión para permitir una ejecución sin ventanas interactivas.
    /// Si SUPERVISOR tiene contraseña, se debe proporcionar mediante una fuente segura.
    /// </summary>
    public string? Usuario { get; set; } = "SUPERVISOR";

    /// <summary>
    /// Contraseña del usuario de Comercial. Una cadena vacía es válida si el usuario no tiene
    /// contraseña. No se debe guardar una contraseña real en el repositorio.
    /// </summary>
    public string? Contrasena { get; set; }

    /// <summary>
    /// Ruta completa del directorio de la empresa que el servicio abre al iniciar.
    /// Debe contener un valor válido para <c>fAbreEmpresa</c> y se obtiene de
    /// <c>ComercialSdk:RutaEmpresa</c> en la configuración de la aplicación.
    /// </summary>
    public string? RutaEmpresa { get; set; }

}
