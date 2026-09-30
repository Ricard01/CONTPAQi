using System.Runtime.InteropServices;
using System.Text;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

/// <summary>
/// Funciones nativas relacionadas con la apertura, cierre y enumeración de empresas.
/// Estas declaraciones no deciden qué empresa usar ni interpretan los códigos devueltos.
/// </summary>
internal static partial class MgwServicios
{
    /// <summary>
    /// Abre la empresa ubicada en el directorio indicado y la establece como la empresa activa
    /// para las operaciones posteriores del SDK.
    /// </summary>
    /// <param name="aDirectorioEmpresa">
    /// Ruta completa del directorio de la empresa, por ejemplo
    /// <c>C:\Compacw\Empresas\EmpresaEjemplo</c>.
    /// </param>
    /// <returns>
    /// <c>0</c> si la empresa se abrió correctamente; cualquier otro valor es un código de error
    /// del SDK que puede consultarse mediante <see cref="fError"/>.
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

    /// <summary>
    /// Se posiciona en el primer registro del catálogo de empresas y escribe sus datos
    /// en los parámetros proporcionados por el llamador.
    /// </summary>
    /// <param name="aIdEmpresa">Recibe el identificador de la primera empresa.</param>
    /// <param name="aNombreEmpresa">Buffer que recibe el nombre de la primera empresa.</param>
    /// <param name="aDirectorioEmpresa">Buffer que recibe el directorio de la primera empresa.</param>
    /// <returns>
    /// <c>0</c> si logró posicionarse y recuperar la empresa; cualquier otro valor es un código
    /// devuelto por el SDK.
    /// </returns>
    /// <remarks>
    /// El manual define los textos como parámetros por referencia. En la interoperabilidad con
    /// .NET se usa <see cref="StringBuilder"/> para proporcionar buffers modificables al código nativo.
    /// </remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fPosPrimerEmpresa(ref int aIdEmpresa, StringBuilder aNombreEmpresa, StringBuilder aDirectorioEmpresa);

    /// <summary>
    /// Avanza al siguiente registro del catálogo de empresas y escribe sus datos en los parámetros
    /// proporcionados por el llamador.
    /// </summary>
    /// <param name="aIdEmpresa">Recibe el identificador de la siguiente empresa.</param>
    /// <param name="aNombreEmpresa">Buffer que recibe el nombre de la siguiente empresa.</param>
    /// <param name="aDirectorioEmpresa">Buffer que recibe el directorio de la siguiente empresa.</param>
    /// <returns>
    /// <c>0</c> si avanzó y recuperó la siguiente empresa; un valor distinto de cero cuando no
    /// existe otro registro o cuando el SDK devuelve un error.
    /// </returns>
    /// <remarks>
    /// El manual define los textos como parámetros por referencia. En la interoperabilidad con
    /// .NET se usa <see cref="StringBuilder"/> para proporcionar buffers modificables al código nativo.
    /// </remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fPosSiguienteEmpresa(ref int aIdEmpresa, StringBuilder aNombreEmpresa, StringBuilder aDirectorioEmpresa);
}
