using System.Runtime.InteropServices;
using System.Text;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

/// <summary>
/// Funciones generales para autenticación, inicialización, terminación y lectura de errores.
/// Deben ejecutarse desde el hilo exclusivo del SDK y respetando el orden definido por SesionComercialSdk.
/// </summary>
internal static partial class MgwServicios
{
    /// <summary>
    /// Inicializa el SDK y establece la conexión de la aplicación con la base de datos de
    /// CONTPAQi Comercial Premium®.
    /// </summary>
    /// <returns>
    /// <c>0</c> si la inicialización se realizó correctamente; cualquier otro valor es un código
    /// de error que puede consultarse mediante <see cref="fError"/>.
    /// </returns>
    /// <remarks>
    /// El manual indica que no recibe parámetros y que debe llamarse obligatoriamente al inicio
    /// de toda aplicación que utilice el SDK de Comercial Premium.
    /// </remarks>
    [DllImport(Dll)]
    internal static extern int fInicializaSDK();

    /// <summary>
    /// Proporciona las credenciales de CONTPAQi antes de inicializar el SDK, evitando que el
    /// proceso solicite el inicio de sesión de manera interactiva.
    /// </summary>
    /// <param name="aUsuario">Nombre del usuario de CONTPAQi.</param>
    /// <param name="aContrasenia">Contraseña del usuario; puede estar vacía si el usuario no tiene una.</param>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern void fInicioSesionSDK(string aUsuario, string aContrasenia);

    /// <summary>
    /// Libera todos los recursos solicitados por el SDK y termina la sesión global.
    /// </summary>
    /// <remarks>
    /// No recibe parámetros ni devuelve un resultado. Debe llamarse al terminar de utilizar el
    /// SDK para evitar que el servicio quede bloqueado y afecte inicios de sesión posteriores.
    /// </remarks>
    [DllImport(Dll)]
    internal static extern void fTerminaSDK();

    /// <summary>
    /// Recupera la descripción correspondiente a un código de error devuelto por el SDK.
    /// </summary>
    /// <param name="aNumError">Código de error que se desea consultar.</param>
    /// <param name="aMensaje">Buffer en el que la función escribe la descripción del error.</param>
    /// <param name="aLen">Longitud disponible en <paramref name="aMensaje"/>.</param>
    /// <remarks>La función no devuelve un valor; el mensaje se recibe mediante <paramref name="aMensaje"/>.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern void fError(int aNumError, StringBuilder aMensaje, int aLen);
}
