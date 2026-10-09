using System.Runtime.InteropServices;
using System.Text;
using SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

// FUNCIONES PARA CREAR, CONSULTAR Y MODIFICAR MOVIMIENTOS.
internal static partial class MgwServicios
{
    #region Bajo nivel – Lectura/Escritura

    /// <summary>Agrega un nuevo registro a la tabla de movimientos y lo deja en modo de inserción.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// <remarks>Después de establecer los campos con <see cref="fSetDatoMovimiento"/>, guarde el registro con <see cref="fGuardaMovimiento"/>.</remarks>
    [DllImport(Dll)]
    internal static extern int fInsertarMovimiento();

    /// <summary>Activa el modo de edición del movimiento actualmente posicionado.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fEditarMovimiento();

    /// <summary>Guarda los cambios realizados al movimiento activo.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// <remarks>Se llama después de insertar o editar el movimiento y asignar sus campos.</remarks>
    [DllImport(Dll)]
    internal static extern int fGuardaMovimiento();

    /// <summary>Descarta los cambios del movimiento activo.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// <remarks>El movimiento debe encontrarse en modo de edición o inserción.</remarks>
    [DllImport(Dll)]
    internal static extern int fCancelaCambiosMovimiento();

    /// <summary>Agrega un movimiento con características a un movimiento existente.</summary>
    /// <param name="aIdMovimiento">Identificador del movimiento al que se asociarán las características.</param>
    /// <param name="aIdMovtoCaracteristicas">Recibe el identificador del nuevo registro con características.</param>
    /// <param name="aUnidades">Unidades del movimiento.</param>
    /// <param name="aValorCaracteristica1">Valor de la característica 1.</param>
    /// <param name="aValorCaracteristica2">Valor de la característica 2.</param>
    /// <param name="aValorCaracteristica3">Valor de la característica 3.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fAltaMovimientoCaracteristicas_Param(string aIdMovimiento,
        string aIdMovtoCaracteristicas, string aUnidades, string aValorCaracteristica1,
        string aValorCaracteristica2, string aValorCaracteristica3);

    /// <summary>Agrega características a un movimiento con unidades de compra/venta.</summary>
    /// <param name="aIdMovimiento">Identificador del movimiento.</param>
    /// <param name="aIdMovtoCaracteristicas">Identificador del registro de características.</param>
    /// <param name="aUnidad">Abreviatura de la unidad de compra/venta.</param>
    /// <param name="aUnidades">Unidades del movimiento de características.</param>
    /// <param name="aUnidadesNC">Abreviatura de la unidad de compra/venta no convertible.</param>
    /// <param name="aValorCaracteristica1">Valor de la característica 1.</param>
    /// <param name="aValorCaracteristica2">Valor de la característica 2.</param>
    /// <param name="aValorCaracteristica3">Valor de la característica 3.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fAltaMovtoCaracteristicasUnidades_Param(string aIdMovimiento,
        string aIdMovtoCaracteristicas, string aUnidad, string aUnidades, string aUnidadesNC,
        string aValorCaracteristica1, string aValorCaracteristica2, string aValorCaracteristica3);

    /// <summary>Agrega series, lotes o pedimentos a un movimiento.</summary>
    /// <param name="aIdMovimiento">Identificador del movimiento.</param>
    /// <param name="aUnidades">Unidades asociadas a las series, lotes o pedimentos.</param>
    /// <param name="aTipoCambio">Tipo de cambio.</param>
    /// <param name="aSeries">Números de serie.</param>
    /// <param name="aPedimento">Referencia del pedimento.</param>
    /// <param name="aAgencia">Referencia de la agencia aduanal.</param>
    /// <param name="aFechaPedimento">Fecha del pedimento.</param>
    /// <param name="aNumeroLote">Número de lote.</param>
    /// <param name="aFechaFabricacion">Fecha de fabricación.</param>
    /// <param name="aFechaCaducidad">Fecha de caducidad.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fAltaMovimientoSeriesCapas_Param(string aIdMovimiento, string aUnidades,
        string aTipoCambio, string aSeries, string aPedimento, string aAgencia, string aFechaPedimento,
        string aNumeroLote, string aFechaFabricacion, string aFechaCaducidad);

    /// <summary>Recalcula un movimiento asociado a un producto con series, lotes o pedimentos.</summary>
    /// <param name="aIdMovimiento">Identificador del movimiento que se recalculará.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fCalculaMovtoSerieCapa(int aIdMovimiento);

    /// <summary>Obtiene las unidades pendientes de un producto para un concepto y almacén.</summary>
    /// <param name="aConceptoDocto">Código del concepto del documento.</param>
    /// <param name="aCodigoProducto">Código del producto.</param>
    /// <param name="aCodigoAlmacen">Código del almacén; <c>0</c> busca en todos los almacenes.</param>
    /// <param name="aUnidades">Buffer que recibe las unidades pendientes como cadena.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// <remarks>El manual indica que el resultado considera el historial completo del sistema.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fObtieneUnidadesPendientes(string aConceptoDocto, string aCodigoProducto, string aCodigoAlmacen, StringBuilder aUnidades);

    /// <summary>Obtiene las unidades pendientes de un producto considerando los valores de sus características.</summary>
    /// <param name="aConceptoDocto">Código del concepto del documento.</param>
    /// <param name="aCodigoProducto">Código del producto.</param>
    /// <param name="aCodigoAlmacen">Código del almacén; <c>0</c> busca en todos los almacenes.</param>
    /// <param name="aValorCaracteristica1">Valor de la característica 1.</param>
    /// <param name="aValorCaracteristica2">Valor de la característica 2.</param>
    /// <param name="aValorCaracteristica3">Valor de la característica 3.</param>
    /// <param name="aUnidades">Buffer que recibe las unidades pendientes como cadena.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// <remarks>El manual indica que el resultado considera el historial completo del sistema.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fObtieneUnidadesPendientesCarac(string aConceptoDocto,
        string aCodigoProducto, string aCodigoAlmacen, string aValorCaracteristica1,
        string aValorCaracteristica2, string aValorCaracteristica3, StringBuilder aUnidades);

    /// <summary>Modifica el costo de una entrada de inventario.</summary>
    /// <param name="aIdMovimiento">Identificador del movimiento que se modificará.</param>
    /// <param name="aCostoEntrada">Costo que se asignará al movimiento.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fModificaCostoEntrada(string aIdMovimiento, string aCostoEntrada);

    /// <summary>Escribe un valor en el campo especificado del movimiento activo.</summary>
    /// <param name="aCampo">Nombre del campo destino.</param>
    /// <param name="aValor">Valor que se escribirá.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fSetDatoMovimiento(string aCampo, string aValor);

    /// <summary>Lee el valor del campo especificado en el movimiento activo.</summary>
    /// <param name="aCampo">Nombre del campo que se leerá.</param>
    /// <param name="aValor">Buffer que recibe el valor leído.</param>
    /// <param name="aLen">Capacidad del buffer de salida.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fLeeDatoMovimiento(string aCampo, StringBuilder aValor, int aLen);

    #endregion

    #region Bajo nivel - Búsqueda/Navegación

    /// <summary>Aplica un filtro a los movimientos del documento indicado.</summary>
    /// <param name="aIdDocumento">Identificador del documento cuyos movimientos se filtrarán.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fSetFiltroMovimiento(int aIdDocumento);

    /// <summary>Cancela el filtro activo de movimientos.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// <remarks>El texto descriptivo del PDF repite la explicación de <see cref="fSetFiltroMovimiento"/>; el nombre de la función y el ejemplo la usan para cancelar el filtro.</remarks>
    [DllImport(Dll)]
    internal static extern int fCancelaFiltroMovimiento();

    /// <summary>Busca un movimiento por su identificador y posiciona el registro encontrado.</summary>
    /// <param name="aIdMovimiento">Identificador del movimiento.</param>
    /// <returns><c>0</c> si encontró el movimiento; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fBuscarIdMovimiento(int aIdMovimiento);

    /// <summary>Posiciona el registro activo en el primer movimiento del documento filtrado.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fPosPrimerMovimiento();

    /// <summary>Posiciona el registro activo en el último movimiento del documento filtrado.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fPosUltimoMovimiento();

    /// <summary>Avanza el registro activo al movimiento siguiente.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fPosSiguienteMovimiento();

    /// <summary>Retrocede el registro activo al movimiento anterior.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fPosAnteriorMovimiento();

    /// <summary>Indica si el registro activo está al inicio de la tabla de movimientos.</summary>
    /// <returns>1 si está al inicio; 0 si no.</returns>
    [DllImport(Dll)]
    internal static extern int fPosMovimientoBOF();

    /// <summary>Indica si el registro activo está al final de la tabla de movimientos.</summary>
    /// <returns>1 si está al final; 0 si no.</returns>
    [DllImport(Dll)]
    internal static extern int fPosMovimientoEOF();

    #endregion

    #region Alto nivel – Lectura/Escritura

    /// <summary>Da de alta un movimiento en el documento indicado.</summary>
    /// <param name="aIdDocumento">Identificador del documento al que pertenece el movimiento.</param>
    /// <param name="aIdMovimiento">Recibe el identificador del nuevo movimiento.</param>
    /// <param name="astMovimiento">Datos del movimiento.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// <remarks>En la tabla de parámetros del manual parecen intercambiadas las descripciones de los dos identificadores; la firma y el ejemplo indican que <paramref name="aIdDocumento"/> es de entrada y <paramref name="aIdMovimiento"/> recibe el nuevo identificador.</remarks>
    [DllImport(Dll)]
    internal static extern int fAltaMovimiento(int aIdDocumento, ref int aIdMovimiento, ref tMovimiento astMovimiento);

    /// <summary>Agrega datos adicionales de series, lotes, pedimentos o características a un movimiento.</summary>
    /// <param name="aIdMovimiento">Identificador del movimiento.</param>
    /// <param name="aTipoProducto">Datos adicionales del producto asociados al movimiento.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fAltaMovimientoEx(ref int aIdMovimiento, ref tTipoProducto aTipoProducto);

    /// <summary>Da de alta un movimiento con importes y porcentajes de descuento.</summary>
    /// <param name="aIdDocumento">Identificador del documento al que pertenece el movimiento.</param>
    /// <param name="aIdMovimiento">Recibe el identificador del nuevo movimiento.</param>
    /// <param name="astMovimiento">Datos del movimiento y sus descuentos.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// <remarks>Incluye importes y porcentajes de descuentos, a diferencia de <see cref="fAltaMovimiento"/>. La tabla del manual parece intercambiar las descripciones de los identificadores; la sintaxis y el ejemplo indican que el documento entra por valor y el identificador del movimiento se recibe por referencia.</remarks>
    [DllImport(Dll)]
    internal static extern int fAltaMovimientoCDesct(int aIdDocumento, ref int aIdMovimiento, ref tMovimientoDesc astMovimiento);

    /// <summary>Inserta un registro de características para el movimiento indicado.</summary>
    /// <param name="aIdMovimiento">Identificador del movimiento.</param>
    /// <param name="aIdMovtoCaracteristicas">Recibe el identificador del nuevo registro de características.</param>
    /// <param name="aCaracteristicas">Unidades y valores de características.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// <remarks><paramref name="aIdMovtoCaracteristicas"/> Al finalizar la función este parámetro contiene el identificador del nuevo movimiento.
    /// Esta función da de alta movimiento de características con unidades de compra venta. </remarks>
    [DllImport(Dll)]
    internal static extern int fAltaMovimientoCaracteristicas(int aIdMovimiento, ref int aIdMovtoCaracteristicas, ref tCaracteristicas aCaracteristicas);

    /// <summary>Da de alta características y unidades de compra/venta para el movimiento indicado.</summary>
    /// <param name="aIdMovimiento">Identificador del movimiento.</param>
    /// <param name="aIdMovtoCaracteristicas">Recibe el identificador del nuevo registro de características.</param>
    /// <param name="aCaracteristicasUnidades">Unidades, abreviaturas y valores de características.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// <remarks><paramref name="aIdMovtoCaracteristicas"/>  Al finalizar la función este parámetro contiene el identificador del nuevo movimiento.
    /// Esta función da de alta movimiento de características con unidades de compra venta. TODO: Preguntar por la estructura del SDK ya que no viene incluido en el manual.</remarks>
    [DllImport(Dll)]
    internal static extern int fAltaMovtoCaracteristicasUnidades(int aIdMovimiento, ref int aIdMovtoCaracteristicas, ref tCaracteristicasUnidades aCaracteristicasUnidades);

    /// <summary>Agrega los datos de series, lotes o pedimentos al movimiento indicado.</summary>
    /// <param name="aIdMovimiento">Identificador del movimiento.</param>
    /// <param name="aSeriesCapas">Unidades, tipo de cambio, series, pedimento, agencia, lote y fechas.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fAltaMovimientoSeriesCapas(int aIdMovimiento, ref tSeriesCapas aSeriesCapas);

    #endregion
}