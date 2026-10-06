using System.Runtime.InteropServices;
using System.Text;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

// FUNCIONES PARA DATOS DE ADDENDA, LICENCIA Y PROXY
internal static partial class MgwServicios
{
    #region Bajo nivel – Lectura/Escritura
    
    /// <summary>Inserta un dato del complemento educativo en el campo indicado.</summary><param name="aIdServicio">Identificador del servicio.</param><param name="aNumCampo">Número de campo.</param><param name="aDato">Dato que se insertará.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fInsertaDatoCompEducativo(int aIdServicio, int aNumCampo, string aDato);
    
    /// <summary>Inserta un dato de addenda del documento en el campo indicado.</summary><param name="aIdAddenda">Identificador de la addenda.</param><param name="aIdCatalogo">Identificador del catálogo.</param><param name="aNumCampo">Número de campo.</param><param name="aDato">Dato que se insertará.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fInsertaDatoAddendaDocto(int aIdAddenda, int aIdCatalogo, int aNumCampo, string aDato);
   
    /// <summary>Obtiene los datos de licencia de activación.</summary><param name="aCodActiva">Buffer que recibe el código de activación.</param><param name="aCodSitio">Buffer que recibe el código de sitio.</param><param name="aSerie">Buffer que recibe la serie.</param><param name="aTagVersion">Buffer que recibe la etiqueta de versión.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>El manual indica que debe invocarse primero <see cref="fInicializaLicenseInfo"/>. Esta consulta solo aplica a CONTPAQi Factura Electrónica.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fObtieneLicencia(StringBuilder aCodActiva, StringBuilder aCodSitio, StringBuilder aSerie, StringBuilder aTagVersion);
    
    /// <summary>Obtiene la contraseña configurada para el proxy.</summary><param name="aPassProxy">Buffer que recibe la contraseña del proxy.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fObtienePassProxy(StringBuilder aPassProxy);

    #endregion
}
