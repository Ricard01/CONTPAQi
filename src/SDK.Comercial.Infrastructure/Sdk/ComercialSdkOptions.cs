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
    /// Nombre que recibe <c>fSetNombrePAQ</c> para seleccionar CONTPAQi Comercial Premium.
    /// </summary>
    public string NombrePaq { get; set; } = "CONTPAQ I COMERCIAL";

    /// <summary>
    /// Usuario de CONTPAQi Comercial que se entrega a <c>fInicioSesionSDK</c> antes de inicializar
    /// el SDK. Se usa SUPERVISOR por omisión para permitir una ejecución sin ventanas interactivas.
    /// Si SUPERVISOR tiene contraseña, se debe proporcionar mediante una fuente segura.
    /// </summary>
    public string? Usuario { get; set; } = "SUPERVISOR";

    /// <summary>
    /// Contraseña del usuario de Comercial. Una cadena vacía es válida si el usuario no tiene
    /// contraseña. No se debe guardar una contraseña real en el repositorio.
    /// </summary>
    public string? Contrasena { get; set; }

    /// <summary>
    /// Ruta completa de la empresa que se abre al iniciar el SDK y que usan las operaciones que
    /// TODO: PORQUE ES OPCIONAL? Es opcional para poder iniciar el SDK y consultar las rutas disponibles con <c>GET /api/empresas</c>.
    /// </summary>
    public string? RutaEmpresa { get; set; }

    /// <summary>
    /// Número máximo de operaciones que pueden esperar al hilo exclusivo del SDK. Cuando se
    /// alcanza, las nuevas solicitudes esperan espacio en vez de consumir memoria sin límite.
    /// </summary>
    public int CapacidadCola { get; set; } = 100;
}
