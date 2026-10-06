using System.Runtime.InteropServices;
using System.Text;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

// Funciones para la lectura y modificación de parámetros.
internal static partial class MgwServicios
{
    #region Bajo nivel – Lectura/Escritura
    
    /// <summary>Lee el valor del parámetro indicado.</summary><param name="aCampo">Nombre del parámetro o campo.</param><param name="aValor">Buffer que recibe el valor.</param><param name="aLen">Longitud disponible del buffer.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fLeeDatoParametros(string aCampo, StringBuilder aValor, int aLen);
    
    /// <summary>Activa la edición de los parámetros.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fEditaParametros();
   
    /// <summary>Asigna un valor al parámetro indicado.</summary><param name="aCampo">Nombre del parámetro o campo.</param><param name="aValor">Valor que se asignará.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fSetDatoParametros(string aCampo, string aValor);
    
    /// <summary>Guarda los cambios realizados a los parámetros.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fGuardaParametros();
    #endregion
}
