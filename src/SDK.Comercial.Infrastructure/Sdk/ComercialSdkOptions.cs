namespace SDK.Comercial.Infrastructure.Sdk;

public sealed class ComercialSdkOptions
{
    public const string Seccion = "ComercialSdk";

    /// <summary>
    /// Carpeta de instalación de Comercial Premium (donde está MGWServicios.dll).
    /// Si se deja vacía se lee del registro de Windows.
    /// </summary>
    public string? RutaInstalacion { get; set; }

    public string NombrePaq { get; set; } = "CONTPAQ I COMERCIAL";

    /// <summary>Usuario de Comercial. Obligatorio: sin él el SDK muestra la ventana de inicio de sesión.</summary>
    public string? Usuario { get; set; }

    public string? Contrasena { get; set; }

    /// <summary>Número máximo de operaciones en espera en la cola del SDK.</summary>
    public int CapacidadCola { get; set; } = 100;
}
