using System.Diagnostics.CodeAnalysis;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

/// <summary>
/// Interfaz del SDK con CONTPAQi Comercial Premium (Librería <c>MGWServicios.dll</c>).
/// <list type="bullet">
/// <item> <description> Funciones Generales para autenticación, inicialización, terminación y lectura de errores.</description> </item>
/// <item> <description> Funciones de Empresas para navegación, apertura y cierre de empresas.</description> </item>
/// <item> <description> Funciones de Documentos para crear, consultar, modificar y eliminar.</description> </item>
/// <item> <description> Funciones de Movimientos para crear, consultar, modificar y eliminar. </description> </item>
/// <item> <description> Funciones de Timbrado. </description> </item>
/// <item> <description> Funciones de Clientes y Proveedores para crear, consultar, modificar y eliminar. </description> </item>
/// <item> <description> Funciones de Productos para crear, consultar, modificar y eliminar. </description> </item>
/// <item> <description> Funciones de Addendas. </description> </item>
/// <item> <description> Funciones de Direcciones para crear, consultar y modificar. </description> </item>
/// <item> <description> Funciones de Existencias. </description> </item>
/// <item> <description> Funciones de Costo Historico. </description> </item>
/// <item> <description> Funciones de Conceptos de documentos para su búsqueda, navegación y lectura/escritura. </description> </item>
/// <item> <description> Funciones de Parametros para su lectura y modificación.  </description> </item>
/// <item> <description> Funciones del catálogo de clasificaciones. </description> </item>
/// <item> <description> Funciones del catálogo de valores de clasificaciones. </description> </item>
/// </list>
/// </summary>
[SuppressMessage("Interoperability", "SYSLIB1054:Use LibraryImportAttribute en lugar de DllImportAttribute")]
[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("Globalization", "CA2101:Especificar cálculo de referencias para argumentos de cadena P/Invoke")]
internal static partial class MgwServicios
{
    private const string Dll = "MGWServicios.dll";

    /// <summary>Tamaño compartido de los buffers ANSI utilizados para leer cadenas del SDK.</summary>
    internal const int TamanoBuffer = 512;
}