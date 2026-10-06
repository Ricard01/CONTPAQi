using System.Runtime.InteropServices;
using System.Text;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

// FUNCIONES GENERALES PARA AUTENTICACIÓN, INICIALIZACIÓN, TERMINACIÓN Y LECTURA DE ERRORES.
internal static partial class MgwServicios
{
    /// <summary>
    /// Inicializa el SDK y establece la conexión de la aplicación con la base de datos de
    /// CONTPAQi Comercial Premium®.
    /// </summary>
    /// <returns>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks>
    /// Debe llamarse obligatoriamente al inicio de toda aplicación que utilice el SDK de Comercial Premium.
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
    /// Esta función define el sistema al que se conectará el SDK.
    /// Sino se usa esta función la conexión por omisión será al sistema CONTPAQi Comercial Premium®
    /// </summary>
    /// <param name="aSistema">
    /// Nombre del sistema al que se conectará el SDK.
    /// </param>
    /// <returns>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks>
    /// Si se desea establecer una conexión a CONTPAQi Factura Electrónica® el parámetro aSistema deberá ser "CONTPAQ I Facturacion" y se deberá utilizar en vez de la función fInicializaSDK().
    /// </remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fSetNombrePAQ(string aSistema);


    /// <summary>
    /// Libera todos los recursos solicitados por el SDK, se requiere llamar al terminar de utilizar el SDK
    /// </summary>
    /// <remarks>
    /// No recibe parámetros ni devuelve un resultado. Debe llamarse al terminar de utilizar el
    /// SDK para evitar que el servicio quede bloqueado y afecte inicios de sesión posteriores.
    /// </remarks>
    [DllImport(Dll)]
    internal static extern void fTerminaSDK();

    /// <summary>
    /// Esta función recupera el mensaje de error del SDK.
    /// </summary>
    /// <param name="aNumError">Número del error.</param>
    /// <param name="aMensaje">Descripción del error.</param>
    /// <param name="aLen">Longitud del mensaje de error.</param>
    /// <returns>
    /// <paramref name="aMensaje"/>: Al finalizar la función este parámetro contiene el mensaje de error correspondiente al número de error especificado en aNumError. 
    /// </returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern void fError(int aNumError, StringBuilder aMensaje, int aLen);
}