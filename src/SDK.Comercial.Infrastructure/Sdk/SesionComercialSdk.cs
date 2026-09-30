using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using SDK.Comercial.Application.Common.Exceptions;
using SDK.Comercial.Infrastructure.Sdk.Native;

namespace SDK.Comercial.Infrastructure.Sdk;

/// <summary>
/// Adapta el ciclo de vida de MGWServicios.dll a una sesión de CONTPAQi Comercial.
/// Todas sus operaciones se invocan desde el hilo exclusivo de <see cref="Cola.SdkWorker"/>.
/// </summary>
internal sealed class SesionComercialSdk(
    IOptions<ComercialSdkOptions> opciones,
    ILogger<SesionComercialSdk> logger) : ISesionComercialSdk
{
    private const string LlaveRegistro = @"SOFTWARE\Computación en Acción, SA CV\CONTPAQ I COMERCIAL";

    /// <inheritdoc />
    public void Iniciar()
    {
        var o = opciones.Value;
        if (string.IsNullOrWhiteSpace(o.Usuario))
        {
            throw new ComercialSdkException($"Falta configurar {ComercialSdkOptions.Seccion}:Usuario.");
        }

        // Se acepta una ruta explícita para instalaciones especiales; normalmente se obtiene
        // DirectorioBase del registro de Windows de 32 bits donde CONTPAQi instala el SDK.
        var ruta = !string.IsNullOrWhiteSpace(o.RutaInstalacion) ? o.RutaInstalacion : LeerRutaDelRegistro();
        if (string.IsNullOrWhiteSpace(ruta))
        {
            throw new ComercialSdkException(
                $"No se encontró la ruta de instalación de Comercial. Configura {ComercialSdkOptions.Seccion}:RutaInstalacion.");
        }

        // MGWServicios.dll resuelve sus dependencias desde el directorio actual. Este cambio es
        // global para el proceso, por eso el resto de la aplicación no debe depender de rutas relativas.
        Directory.SetCurrentDirectory(ruta);
        logger.LogInformation("Iniciando SDK de CONTPAQi desde {Ruta}", ruta);

        // El orden es intencional: primero se proporcionan las credenciales para evitar que un
        // servicio de Windows intente mostrar el diálogo de inicio de sesión; después se llama a
        // fInicializaSDK, que es la inicialización obligatoria y predeterminada para Comercial Premium.
        // fSetNombrePAQ no se usa: el manual lo reserva como alternativa a fInicializaSDK cuando se
        // desea conectar con Factura Electrónica.
        // La licencia no se envía por esta API: la valida la instalación local de CONTPAQi.
        MgwServicios.fInicioSesionSDK(o.Usuario, o.Contrasena ?? string.Empty);
        SdkResultado.Verificar(MgwServicios.fInicializaSDK());
    }

    /// <inheritdoc />
    public void Terminar() => MgwServicios.fTerminaSDK();

    /// <inheritdoc />
    public void AbrirEmpresa(string ruta) => SdkResultado.Verificar(MgwServicios.fAbreEmpresa(ruta));

    /// <inheritdoc />
    public void CerrarEmpresa() => MgwServicios.fCierraEmpresa();

    private static string? LeerRutaDelRegistro()
    {
        if (!OperatingSystem.IsWindows())
        {
            return null;
        }

        using var raiz = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        using var llave = raiz.OpenSubKey(LlaveRegistro);
        return llave?.GetValue("DirectorioBase") as string;
    }
}
