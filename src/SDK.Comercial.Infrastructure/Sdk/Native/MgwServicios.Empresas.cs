using System.Runtime.InteropServices;
using System.Text;

namespace SDK.Comercial.Infrastructure.Sdk.Native;


// FUNCIONES RELACIONADAS CON LA APERTURA, CIERRE Y NAVEGACION DE EMPRESAS.

internal static partial class MgwServicios
{
    /// <summary>
    /// Esta función se posiciona en el primer registro de la base de datos de empresas de CONTPAQi Comercial Premium®, modifica los parámetros aNombreEmpresa y aDirectorioEmpresa,
    /// en los cuales guarda el nombre de la primera empresa y su ruta, correspondientemente.
    /// </summary>
    /// <param name="aIdEmpresa">Recibe el identificador de la primera empresa.</param>
    /// <param name="aNombreEmpresa">Recibe el nombre de la primera empresa.</param>
    /// <param name="aDirectorioEmpresa">Recibe el directorio de la primera empresa.</param>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fPosPrimerEmpresa(ref int aIdEmpresa, StringBuilder aNombreEmpresa,
        StringBuilder aDirectorioEmpresa);

    /// <summary>
    /// Esta función avanza al siguiente registro en la tabla de Empresas de CONTPAQi Comercial Premium®
    /// </summary>
    /// <param name="aIdEmpresa">Recibe el identificador de la siguiente empresa.</param>
    /// <param name="aNombreEmpresa">Recibe el nombre de la siguiente empresa.</param>
    /// <param name="aDirectorioEmpresa">Recibe el directorio de la siguiente empresa.</param>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fPosSiguienteEmpresa(ref int aIdEmpresa, StringBuilder aNombreEmpresa, StringBuilder aDirectorioEmpresa);

    /// <summary>
    /// Abre la empresa ubicada en el directorio indicado y la establece como la empresa activa
    /// para las operaciones posteriores del SDK.
    /// </summary>
    /// <param name="aDirectorioEmpresa">
    /// Ruta completa del directorio de la empresa, por ejemplo
    /// <c>C:\Compac\Empresas\AdEmpresaEjemplo</c>.
    /// </param>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fAbreEmpresa(string aDirectorioEmpresa);

    /// <summary>
    /// Cierra la conexión con la empresa que se encuentra activa en el SDK.
    /// </summary>
    /// <remarks>
    /// La función nativa no recibe parámetros ni devuelve un código de resultado.
    /// </remarks>
    [DllImport(Dll)]
    internal static extern void fCierraEmpresa();
}