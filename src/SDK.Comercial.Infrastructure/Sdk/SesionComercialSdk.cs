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
internal sealed class SesionComercialSdk(IOptions<ComercialSdkOptions> opciones,
    ILogger<SesionComercialSdk> logger) : ISesionComercialSdk
{
    private const string LlaveRegistro = @"SOFTWARE\Computación en Acción, SA CV\CONTPAQ I COMERCIAL";

    /// <inheritdoc />
    public void Iniciar()
    {
        var opt = opciones.Value;
        

        var ruta = !string.IsNullOrWhiteSpace(opt.RutaInstalacion) ? opt.RutaInstalacion : LeerRutaDelRegistro();
        if (string.IsNullOrWhiteSpace(ruta))
        {
            throw new ComercialSdkException(
                $"No se encontró la ruta de instalación de Comercial. Configura {ComercialSdkOptions.Seccion}:RutaInstalacion.");
        }

        // MGWServicios.dll resuelve sus dependencias desde el directorio actual. 
        Directory.SetCurrentDirectory(ruta);
        logger.LogInformation("Iniciando SDK de CONTPAQi desde {Ruta}", ruta);
        
        if (string.IsNullOrWhiteSpace(opt.Usuario))
        {
            throw new ComercialSdkException($"Falta definir {ComercialSdkOptions.Seccion}:Usuario.");
        }

        // El orden es intencional: primero se proporcionan las credenciales para evitar que un
        // servicio de Windows intente mostrar el diálogo de inicio de sesión; después se llama a
        // fInicializaSDK, que es la inicialización obligatoria y predeterminada para Comercial Premium.
        // fSetNombrePAQ no se usa: el manual lo reserva como alternativa a fInicializaSDK cuando se
        // desea conectar con Factura Electrónica.
        MgwServicios.fInicioSesionSDK(opt.Usuario, opt.Contrasena ?? string.Empty);
        // Al parecer no lo necesito segun ejemplos  SdkResultado.Verificar(MgwServicios.fInicializaSDK());
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
