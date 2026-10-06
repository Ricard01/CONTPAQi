using System.Runtime.InteropServices;
using System.Text;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

// FUNCIONES PARA TIMBRADO.
internal static partial class MgwServicios
{
   
    /// <summary>Envía un XML creado por una aplicación externa para timbrarlo.</summary>
    /// <param name="aRutaXML">Ruta del archivo XML que se timbrará.</param><param name="aCodConcepto">Código del concepto CFDI que se utilizará.</param><param name="aUUID">Buffer que recibe el UUID asignado.</param><param name="aRutaDDA">Ruta y nombre del DDA con información adicional.</param><param name="aRutaResultado">Ruta donde se generan el XML, HTML e imágenes de entrega.</param><param name="aPass">Contraseña del certificado.</param><param name="aRutaFormato">Ruta y nombre del formato de impresión.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>Debe llamarse primero a <see cref="fInicializaLicenseInfo"/>. El XML debe estar sin emitir, sello ni certificado. Requiere una licencia de 5 usuarios; con licenciamiento anual.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fTimbraXML(string aRutaXML, string aCodConcepto, StringBuilder aUUID, string aRutaDDA, string aRutaResultado, string aPass, string aRutaFormato);
    
    /// <summary>Envía un XML de nómina creado por una aplicación externa para timbrarlo.</summary>
    /// <param name="aRutaXML">Ruta del XML de nómina que se timbrará.</param><param name="aCodConcepto">Código del concepto CFDI que se utilizará.</param><param name="aUUID">Buffer que recibe el UUID asignado.</param><param name="aRutaDDA">Ruta y nombre del DDA con información adicional.</param><param name="aRutaResultado">Ruta donde se generan el XML, HTML e imágenes de entrega.</param><param name="aPass">Contraseña del certificado.</param><param name="aRutaFormato">Ruta y nombre del formato de impresión.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>Debe llamarse primero a <see cref="fInicializaLicenseInfo"/>. El XML debe estar sin emitir, sello ni certificado e incluir el domicilio del emisor. Para incluir datos del complemento de nómina en la impresión, deben insertarse en el DDA. Requiere una licencia de cinco o más usuarios; con licenciamiento anual, también debe ser multiempresa.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fTimbraNominaXML(string aRutaXML, string aCodConcepto, StringBuilder aUUID, string aRutaDDA, string aRutaResultado, string aPass, string aRutaFormato);
 
}
