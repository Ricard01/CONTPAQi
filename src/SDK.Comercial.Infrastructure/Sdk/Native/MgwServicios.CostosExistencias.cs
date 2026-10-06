using System.Runtime.InteropServices;
using System.Text;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

// FUNCIONES PARA COSTOS Y EXISTENCIAS. 
internal static partial class MgwServicios
{
    #region Bajo nivel – Lectura/Escritura
    /// <summary>Obtiene la existencia de un producto en un almacén y fecha.</summary>
    /// <param name="aCodigoProducto">Código del producto.</param><param name="aCodigoAlmacen">Código del almacén.</param><param name="aAnio">Año de consulta.</param><param name="aMes">Mes de consulta.</param><param name="aDia">Día de consulta.</param><param name="aExistencia">Recibe la existencia.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>El manual tipa los componentes de fecha como cadena; se representan como <see cref="string"/> para conservar esa firma.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fRegresaExistencia(string aCodigoProducto, string aCodigoAlmacen, string aAnio, string aMes, string aDia, ref double aExistencia);

    /// <summary>Obtiene la existencia de un producto con características en un almacén y fecha.</summary>
    /// <param name="aCodigoProducto">Código del producto.</param><param name="aCodigoAlmacen">Código del almacén.</param><param name="aAnio">Año de consulta.</param><param name="aMes">Mes de consulta.</param><param name="aDia">Día de consulta.</param><param name="aCaracteristica1">Valor de la característica 1.</param><param name="aCaracteristica2">Valor de la característica 2.</param><param name="aCaracteristica3">Valor de la característica 3.</param><param name="aExistencia">Recibe la existencia.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fRegresaExistenciaCaracteristicas(string aCodigoProducto, string aCodigoAlmacen, string aAnio, string aMes, string aDia, string aCaracteristica1, string aCaracteristica2, string aCaracteristica3, ref double aExistencia);

    /// <summary>Obtiene el costo promedio histórico de un producto.</summary>
    /// <param name="aCodigoProducto">Código del producto.</param><param name="aCodigoAlmacen">Código del almacén; 0 consulta todos los almacenes.</param><param name="aAnio">Año de consulta.</param><param name="aMes">Mes de consulta.</param><param name="aDia">Día de consulta.</param><param name="aCostoPromedio">Buffer que recibe el costo promedio.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fRegresaCostoPromedio(string aCodigoProducto, string aCodigoAlmacen, string aAnio, string aMes, string aDia, StringBuilder aCostoPromedio);
    
    /// <summary>Obtiene el último costo histórico de un producto.</summary>
    /// <param name="aCodigoProducto">Código del producto.</param><param name="aCodigoAlmacen">Código del almacén; 0 consulta todos los almacenes.</param><param name="aAnio">Año de consulta.</param><param name="aMes">Mes de consulta.</param><param name="aDia">Día de consulta.</param><param name="aUltimoCosto">Buffer que recibe el último costo.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fRegresaUltimoCosto(string aCodigoProducto, string aCodigoAlmacen, string aAnio, string aMes, string aDia, StringBuilder aUltimoCosto);
    
    /// <summary>Obtiene el costo estándar de un producto.</summary><param name="aCodigoProducto">Código del producto.</param><param name="aCostoEstandar">Buffer que recibe el costo estándar.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fRegresaCostoEstandar(string aCodigoProducto, StringBuilder aCostoEstandar);
    
    /// <summary>Obtiene el costo de las capas de un producto en almacén por unidades indicadas.</summary>
    /// <param name="aCodigoProducto">Código del producto.</param><param name="aCodigoAlmacen">Código del almacén.</param><param name="aUnidades">Número de unidades a costear.</param><param name="aImporteCosto">Buffer que recibe el importe de las capas.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fRegresaCostoCapa(string aCodigoProducto, string aCodigoAlmacen, double aUnidades, StringBuilder aImporteCosto);
    #endregion
}
