using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using SDK.Comercial.Application.Common.Exceptions;
using SDK.Comercial.Infrastructure.Sdk.Native;

namespace SDK.Comercial.Infrastructure.Sdk;

internal sealed class ComercialSdk(IOptions<ComercialSdkOptions> opciones, ILogger<ComercialSdk> logger) : IComercialSdk
{
    private const string LlaveRegistro = @"SOFTWARE\Computación en Acción, SA CV\CONTPAQ I COMERCIAL";

    public void Iniciar()
    {
        var o = opciones.Value;
        if (string.IsNullOrWhiteSpace(o.Usuario))
        {
            throw new ComercialSdkException($"Falta configurar {ComercialSdkOptions.Seccion}:Usuario.");
        }

        var ruta = !string.IsNullOrWhiteSpace(o.RutaInstalacion) ? o.RutaInstalacion : LeerRutaDelRegistro();
        if (string.IsNullOrWhiteSpace(ruta))
        {
            throw new ComercialSdkException(
                $"No se encontró la ruta de instalación de Comercial. Configura {ComercialSdkOptions.Seccion}:RutaInstalacion.");
        }

        // MGWServicios.dll resuelve sus dependencias desde el directorio actual.
        Directory.SetCurrentDirectory(ruta);
        logger.LogInformation("Iniciando SDK de CONTPAQi desde {Ruta}", ruta);

        ComercialSdkNative.fInicioSesionSDK(o.Usuario, o.Contrasena ?? string.Empty);
        SdkErrores.Verificar(ComercialSdkNative.fSetNombrePAQ(o.NombrePaq));
    }

    public void Terminar() => ComercialSdkNative.fTerminaSDK();

    public void AbrirEmpresa(string ruta) => SdkErrores.Verificar(ComercialSdkNative.fAbreEmpresa(ruta));

    public void CerrarEmpresa() => ComercialSdkNative.fCierraEmpresa();

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
