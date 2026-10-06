using System.Runtime.InteropServices;
using System.Text;
using SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

// FUNCIONES PARA CREAR, CONSULTAR Y MODIFICAR PRODUCTOS.
internal static partial class MgwServicios
{
    #region Bajo nivel – Lectura/Escritura

    /// <summary>Inserta un nuevo registro en la tabla de productos.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks> Se puede consultar el nombre de cada campo utilizable para la función fSetDatoProducto en el documento de base de datos,
    /// tabla Productos del sistema CONTPAQi Factura Electrónica® y tabla admProductos del sistema CONTPAQi Comercial Premium®.
    /// Se puede asignar un valor a la gran mayoría de campos, algunos tienen restricciones que hay que cumplir y otros tantos como el ID no son editables</remarks>
    [DllImport(Dll)]
    internal static extern int fInsertaProducto();

    /// <summary>Activa la edición del producto posicionado.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks> Se puede consultar el nombre de cada campo utilizable para la función fSetDatoProducto en el documento de base de datos,
    /// tabla Productos del sistema CONTPAQi Factura Electrónica® y tabla admProductos del sistema CONTPAQi Comercial Premium®.
    /// Se puede asignar un valor a la gran mayoría de campos, algunos tienen restricciones que hay que cumplir y otros tantos como el ID no son editables</remarks>
    [DllImport(Dll)]
    internal static extern int fEditaProducto();

    /// <summary>Guarda los cambios del producto activo.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks> Se puede consultar el nombre de cada campo utilizable para la función fSetDatoProducto en el documento de base de datos,
    /// tabla Productos del sistema CONTPAQi Factura Electrónica® y tabla admProductos del sistema CONTPAQi Comercial Premium®.
    /// Se puede asignar un valor a la gran mayoría de campos, algunos tienen restricciones que hay que cumplir y otros tantos como el ID no son editables</remarks>
    [DllImport(Dll)]
    internal static extern int fGuardaProducto();

    /// <summary>Borra el producto activo.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fBorraProducto();

    /// <summary>Descarta los cambios del producto en edición o inserción.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, EntryPoint = "fCancelarModificacionProducto")]
    internal static extern int fCancelarModificacionProducto();

    /// <summary>Elimina un producto por su código.</summary>
    /// <param name="aCodigoProducto">Código del producto.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fEliminarProducto(string aCodigoProducto);

    /// <summary>Escribe un valor en un campo del producto activo.</summary>
    /// <param name="aCampo">Nombre del campo destino.</param><param name="aValor">Valor que se escribirá.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks> Se puede consultar el nombre de cada campo utilizable para la función fSetDatoProducto en el documento de base de datos,
    /// tabla Productos del sistema CONTPAQi Factura Electrónica® y tabla admProductos del sistema CONTPAQi Comercial Premium®.
    /// Se puede asignar un valor a la gran mayoría de campos, algunos tienen restricciones que hay que cumplir y otros tantos como el ID no son editables</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fSetDatoProducto(string aCampo, string aValor);

    /// <summary>Lee un campo del producto activo.</summary>
    /// <param name="aCampo">Nombre del campo.</param><param name="aValor">Buffer que recibe el valor.</param><param name="aLen">Longitud disponible.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fLeeDatoProducto(string aCampo, StringBuilder aValor, int aLen);

    /// <summary>Recupera los indicadores de manejo del tipo de producto.</summary>
    /// <param name="aUnidades">Indica si maneja unidades.</param><param name="aSerie">Indica si maneja series.</param><param name="aLote">Indica si maneja lotes.</param><param name="aPedimento">Indica si maneja pedimentos.</param><param name="aCaracteristicas">Indica si maneja características.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks> Esta función define el tipo de producto, indicando si maneja series, lotes, pedimentos, unidades y/o características.</remarks>
    [DllImport(Dll)]
    internal static extern int fRecuperaTipoProducto(ref bool aUnidades, ref bool aSerie, ref bool aLote, ref bool aPedimento, ref bool aCaracteristicas);

    /// <summary>Obtiene el precio de venta de un producto para un cliente y concepto.</summary>
    /// <param name="aCodigoConcepto">Código del concepto de documento.</param><param name="aCodigoCliente">Código del cliente.</param><param name="aCodigoProducto">Código del producto.</param><param name="aPrecioVenta">Buffer que recibe el precio de venta.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>Al finalizar la función este parámetro contiene el precio de venta del producto solicitado.
    /// Esta función obtiene el precio de venta de un producto de un determinado cliente para un concepto de documento en específico.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fRegresaPrecioVenta(string aCodigoConcepto, string aCodigoCliente, string aCodigoProducto, StringBuilder aPrecioVenta);

    /// <summary>Recostea productos para el ejercicio, período y clasificaciones indicados.</summary>
    /// <param name="aCodigoProducto">Código del producto.</param><param name="aEjercicio">Ejercicio del recosteo.</param><param name="aPeriodo">Período del recosteo.</param><param name="aCodigoClasificacion1">Código de clasificación 1.</param><param name="aCodigoClasificacion2">Código de clasificación 2.</param><param name="aCodigoClasificacion3">Código de clasificación 3.</param><param name="aCodigoClasificacion4">Código de clasificación 4.</param><param name="aCodigoClasificacion5">Código de clasificación 5.</param><param name="aCodigoClasificacion6">Código de clasificación 6.</param><param name="aNombreBitacora">Nombre de la bitácora.</param><param name="aSobreEscribirBitacora">Indica si se sobrescribe la bitácora.</param><param name="aEsCalculoAritmetico">Indica si se usa cálculo aritmético.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>El PDF incluye la lista de parámetros en la sintaxis de la sección de precio, pero no especifica sus tipos en una tabla. Los tipos de los indicadores se representan como booleanos, según su significado.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fRecosteoProducto(string aCodigoProducto, int aEjercicio, int aPeriodo,
        string aCodigoClasificacion1, string aCodigoClasificacion2, string aCodigoClasificacion3,
        string aCodigoClasificacion4, string aCodigoClasificacion5, string aCodigoClasificacion6,
        string aNombreBitacora, bool aSobreEscribirBitacora, bool aEsCalculoAritmetico);

    #endregion

    #region Bajo nivel – Búsqueda/Navegación

    /// <summary>Busca un producto por código.</summary><param name="aCodProducto">Código del producto.</param>
    /// <returns><c>0</c> si lo encontró; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fBuscaProducto(string aCodProducto);

    /// <summary>Busca un producto por identificador.</summary><param name="aIdProducto">Identificador del producto.</param>
    /// <returns><c>0</c> si lo encontró; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fBuscaIdProducto(int aIdProducto);

    /// <summary>Posiciona el primer producto.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fPosPrimerProducto();

    /// <summary>Posiciona el último producto.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fPosUltimoProducto();

    /// <summary>Avanza al producto siguiente.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fPosSiguienteProducto();

    /// <summary>Retrocede al producto anterior.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fPosAnteriorProducto();

    /// <summary>Indica si el registro activo está al inicio del catálogo.</summary><returns>1 si está al inicio; 0 si no.</returns>
    [DllImport(Dll)]
    internal static extern int fPosBOFProducto();

    /// <summary>Indica si el registro activo está al final del catálogo.</summary><returns>1 si está al final; 0 si no.</returns>
    [DllImport(Dll)]
    internal static extern int fPosEOFProducto();

    #endregion

    #region Alto nivel – Lectura/Escritura

    /// <summary>Da de alta un producto a partir de su estructura.</summary>
    /// <param name="aIdProducto">Recibe el identificador asignado.</param><param name="astProducto">Datos del producto.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fAltaProducto(ref int aIdProducto, tProducto astProducto);

    /// <summary>Actualiza el producto identificado por código.</summary><param name="aCodigoProducto">Código del producto.</param><param name="astProducto">Nuevos datos.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>La tabla de parámetros del PDF contiene <c>astCteProv</c>, aparentemente copiado de otro apartado; aquí se usa la estructura de producto indicada por la función.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fActualizaProducto(string aCodigoProducto, tProducto astProducto);

    /// <summary>Copia los campos de la estructura al registro activo del producto.</summary><param name="astProducto">Estructura con los datos.</param><param name="aEsAlta">1 para alta; 2 para actualización.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fLlenaRegistroProducto(tProducto astProducto, int aEsAlta);

    #endregion
}