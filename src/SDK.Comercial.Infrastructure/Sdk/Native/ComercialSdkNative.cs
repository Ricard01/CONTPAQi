using System.Runtime.InteropServices;
using System.Text;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

/// <summary>
/// Funciones de MGWServicios.dll. Solo se deben invocar desde el hilo consumidor de la cola del SDK.
/// </summary>
internal static class ComercialSdkNative
{
    private const string Dll = "MGWServicios.dll";

    /// <summary>Tamaño de los buffers de salida para cadenas del SDK.</summary>
    public const int TamanoBuffer = 512;

    [DllImport(Dll, CharSet = CharSet.Ansi)]
    public static extern void fInicioSesionSDK(string aUsuario, string aContrasenia);

    [DllImport(Dll, CharSet = CharSet.Ansi)]
    public static extern int fSetNombrePAQ(string aNombrePAQ);

    [DllImport(Dll)]
    public static extern void fTerminaSDK();

    [DllImport(Dll, CharSet = CharSet.Ansi)]
    public static extern void fError(int aNumError, StringBuilder aMensaje, int aLen);

    [DllImport(Dll, CharSet = CharSet.Ansi)]
    public static extern int fAbreEmpresa(string aDirectorioEmpresa);

    [DllImport(Dll)]
    public static extern void fCierraEmpresa();

    [DllImport(Dll, CharSet = CharSet.Ansi)]
    public static extern int fPosPrimerEmpresa(ref int aIdEmpresa, StringBuilder aNombreEmpresa, StringBuilder aDirectorioEmpresa);

    [DllImport(Dll, CharSet = CharSet.Ansi)]
    public static extern int fPosSiguienteEmpresa(ref int aIdEmpresa, StringBuilder aNombreEmpresa, StringBuilder aDirectorioEmpresa);
}
