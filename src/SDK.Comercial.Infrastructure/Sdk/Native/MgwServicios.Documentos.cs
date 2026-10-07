using System.Runtime.InteropServices;
using System.Text;
using SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;


namespace SDK.Comercial.Infrastructure.Sdk.Native;

// FUNCIONES PARA BUSCAR, RECORRER, LEER Y MODIFICAR DOCUMENTOS. 
internal static partial class MgwServicios
{
    #region Bajo nivel – Lectura/Escritura

    /// <summary> Agrega un nuevo registro en la tabla de Documentos en modo de inserción.</summary>
    /// <returns>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks>
    /// Para que la función  pueda establecer un nuevo registro a la tabla de documentos, es necesario indicar mediante la función
    /// <see cref="fSetDatoDocumento"/>  los registros de la tabla Documentos a afectar.
    /// Después de la inserción de los valores a afectar, se utiliza la función <see cref="fGuardaDocumento"/>,
    /// la cual no lleva parámetros; si no se utiliza esta función, no se agregará el nuevo registro a la tabla de documentos.
    /// </remarks>
    [DllImport(Dll)]
    internal static extern int fInsertarDocumento();

    /// <summary>Activa el modo de edición del documento previamente localizado.</summary>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks>
    /// Para poder editar un documento, es necesario posicionarnos sobre él y esto se consigue llevando a cabo una búsqueda del documento. <see cref="fBuscaDocumento"/>
    /// </remarks>
    [DllImport(Dll)]
    internal static extern int fEditarDocumento();

    /// <summary>Guarda los cambios hechos al documento activo o confirma su inserción.</summary>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks>
    /// Se utiliza cuando un documento recibe algún tipo de edición. Si no se utiliza la función no se aplicarán las modificaciones que se hayan realizado.
    /// </remarks>
    [DllImport(Dll)]
    internal static extern int fGuardaDocumento();

    /// <summary>Cancela las modificaciones al registro actual de documentos. El registro debe estar en modo de edición o inserción.</summary>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks>
    /// Es necesario realizar una búsqueda del documento, y si lo encuentra, aplica el procedimiento de cancelación a las modificaciones al registro actual de documentos.
    /// </remarks>
    [DllImport(Dll, EntryPoint = "fCancelarModificacionDocumento")]
    internal static extern int fCancelarModificacionDocumento();

    /// <summary>Borra el documento activo; primero debe localizarse el documento.</summary>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks>
    /// Para poder borrar un documento, es necesario llevar a cabo una búsqueda del documento para posicionarse sobre él, mediante la funcion
    /// <see cref="fBuscaDocumento"/> o <see cref="fBuscarDocumento"/>
    /// </remarks>
    [DllImport(Dll)]
    internal static extern int fBorraDocumento();

    /// <summary>Cancela el documento activo.</summary>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks> Para posicionarnos sobre el documento a cancelar utilizamos la función <see cref="fBuscarDocumento"/> </remarks>
    [DllImport(Dll)]
    internal static extern int fCancelaDocumento();

    /// <summary>Borra el documento y, si está contabilizado, borra la poliza correspondiente en CONTPAQi® Contabilidad.</summary>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks> Para posicionarnos sobre el documento a borrar utilizamos la función <see cref="fBuscarDocumento"/> </remarks>
    [DllImport(Dll)]
    internal static extern int fBorraDocumento_CW();

    /// <summary>Cancela el documento y borra la poliza correspondiente en CONTPAQi® Contabilidad.</summary>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks> Para posicionarnos sobre el documento a cancelar utilizamos la función <see cref="fBuscarDocumento"/> </remarks>
    [DllImport(Dll)]
    internal static extern int fCancelaDocumento_CW();

    /// <summary>Aplica o retira la afectación de un documento identificado por concepto, serie y folio.</summary>
    /// <param name="aCodConcepto">Código del concepto.</param><param name="aSerie">Serie.</param>
    /// <param name="aFolio">Folio del documento.</param><param name="aAfecta">True para afectar; false para desafectar.</param>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks>
    /// Esta funcion utiliza <paramref name="aCodConcepto"/>, <paramref name="aSerie"/> y <paramref name="aFolio"/> como llave del documento y aAfecta para afectar o desafectarlo.
    /// </remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi, EntryPoint = "fAfectaDocto_Param")]
    internal static extern int fAfectaDocto_Param(string aCodConcepto, string aSerie, double aFolio,
        [MarshalAs(UnmanagedType.Bool)] bool aAfecta);

    /// <summary>Asocia dos documentos y aplica el importe indicado al documento por pagar.</summary>
    /// <param name="aCodConcepto_Pagar">Código del concepto del documento por pagar.</param>
    /// <param name="aSerie_Pagar">Serie del documento por pagar.</param>
    /// <param name="aFolio_Pagar">Folio del documento por pagar.</param>
    /// <param name="aCodConcepto_Pago">Código del concepto del documento que realiza el pago.</param>
    /// <param name="aSerie_Pago">Serie del documento que realiza el pago.</param>
    /// <param name="aFolio_Pago">Folio del documento que realiza el pago.</param>
    /// <param name="aImporte">Importe del pago que se asocia.</param>
    /// <param name="aIdMoneda">Identificador de la moneda del pago.</param>
    /// <param name="aFecha">Fecha del pago.</param>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks>El parametro <paramref name="aFolio_Pago"/> esta marcado como tipo CADENA, pero la situacion real es que es tipo <see cref="double"/> </remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi, EntryPoint = "fSaldarDocumento_Param")]
    internal static extern int fSaldarDocumento_Param(
        string aCodConcepto_Pagar, string aSerie_Pagar, double aFolio_Pagar,
        string aCodConcepto_Pago, string aSerie_Pago, double aFolio_Pago,
        double aImporte, int aIdMoneda, string aFecha);

    /// <summary>Elimina la asociación entre un documento pagado y el documento que lo pagó.</summary>
    /// <param name="aCodConcepto_Pagar">Código del concepto del documento pagado.</param>
    /// <param name="aSerie_Pagar">Serie del documento pagado.</param>
    /// <param name="aFolio_Pagar">Folio del documento pagado.</param>
    /// <param name="aCodConcepto_Pago">Código del concepto del documento que realizó el pago.</param>
    /// <param name="aSerie_Pago">Serie del documento que realizó el pago.</param>
    /// <param name="aFolio_Pago">Folio del documento que realizó el pago.</param>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks>El parametro <paramref name="aFolio_Pago"/> esta marcado como tipo CADENA, pero la situacion real es que es tipo <see cref="double"/> </remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi, EntryPoint = "fBorrarAsociacion_Param")]
    internal static extern int fBorrarAsociacion_Param(
        string aCodConcepto_Pagar, string aSerie_Pagar, double aFolio_Pagar,
        string aCodConcepto_Pago, string aSerie_Pago, double aFolio_Pago);
    
    /// <summary> Escribe el valor indicado en el campo correspondiente en el registro activo de la tabla de documentos.</summary>
    /// <param name="aCampo">Nombre del campo destino.</param>
    /// <param name="aValor">Valor que se escribirá.</param>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    /// <remarks>
    /// Para poder setear un documento, es necesario posicionarnos sobre él y esto es llevándose a cabo una búsqueda del documento mediante la funcion
    /// <see cref="fBuscaDocumento"/> o <see cref="fBuscarDocumento"/> Una vez encontrado, lo ponemos en modo de edición con la función fEditarDocumento para poder aplicar los cambios o actualizaciones correspondientes en modo de programación de bajo nivel.
    /// Realizado lo anterior, se guardan las modificaciones realizadas empleando la función fGuardaDocumento:
    /// <see cref="fEditarDocumento"/>
    /// <see cref="fSetDatoDocumento"/>
    /// <see cref="fGuardaDocumento"/> 
    /// </remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fSetDatoDocumento(string aCampo, string aValor);

    /// <summary> Esta función lee el valor indicado del campo correspondiente en el registro activo de la tabla de documentos.</summary>
    /// <param name="aCampo">Nombre del campo que se leerá.</param>
    /// <param name="aValor">Buffer que recibirá el valor.</param>
    /// <param name="aLongitud">Capacidad del buffer en caracteres.</param>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fLeeDatoDocumento(string aCampo, StringBuilder aValor, int aLongitud);

    /// <summary>Obtiene el siguiente folio disponible para un concepto y serie.</summary>
    /// <param name="aCodigoConcepto">Código del concepto.
    /// </param><param name="aSerie">Serie; recibe la serie resuelta por el SDK.</param>
    /// <param name="aFolio">Recibe el siguiente folio.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fSiguienteFolio(string aCodigoConcepto, StringBuilder aSerie, ref double aFolio);

    /// <summary>Aplica un filtro de documentos por fechas, concepto y cliente/proveedor.</summary>
    /// <param name="aFechaInicio">Inicio del rango.</param><param name="aFechaFin">Fin del rango.</param>
    /// <param name="aCodigoConcepto">Concepto a filtrar.</param><param name="aCodigoCteProv">Cliente o proveedor a filtrar.</param>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fSetFiltroDocumento(string aFechaInicio, string aFechaFin, string aCodigoConcepto,
        string aCodigoCteProv);

    /// <summary>Retira el último filtro aplicado a documentos.</summary>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    [DllImport(Dll)]
    internal static extern int fCancelaFiltroDocumento();
    
    /// <summary>Cambia la bandera de impreso del documento activo.</summary>
    /// <param name="aImpreso">Bandera que se asignará.</param><returns>0 si tuvo éxito; otro valor es un código de error.</returns>
    [DllImport(Dll)]
    internal static extern int fDocumentoImpreso([MarshalAs(UnmanagedType.Bool)] bool aImpreso);

    #endregion

    #region Bajo nivel - Búsqueda/Navegación

    /// <summary>Busca un documento por concepto, serie y folio y posiciona el registro activo.</summary>
    /// <param name="aCodConcepto">Código del concepto del documento.</param>
    /// <param name="aSerie">Serie del documento.</param>
    /// <param name="aFolio">Folio del documento, representado como cadena por esta función.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fBuscarDocumento(string aCodConcepto, string aSerie, string aFolio);

    /// <summary>Busca un documento por su identificador y posiciona el registro activo.</summary>
    /// <param name="aIdDocumento">Identificador del documento.</param>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    [DllImport(Dll)]
    internal static extern int fBuscarIdDocumento(int aIdDocumento);

    /// <summary> Esta función se ubica en el primer registro de la tabla de documentos.</summary>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    [DllImport(Dll)]
    internal static extern int fPosPrimerDocumento();

    /// <summary> Esta función se ubica en el último registro de la tabla de documentos. </summary>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    [DllImport(Dll)]
    internal static extern int fPosUltimoDocumento();

    /// <summary> Esta función se ubica en el siguiente registro de la posición actual de la tabla de documentos.</summary>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    [DllImport(Dll)]
    internal static extern int fPosSiguienteDocumento();

    /// <summary>Esta función se ubica en el registro anterior de la posición actual de la tabla de documentos.</summary>
    /// <returns>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// </returns>
    [DllImport(Dll)]
    internal static extern int fPosAnteriorDocumento();

    /// <summary> Informa si el registro activo se encuentra en el inicio de la tabla de Documentos.</summary>
    /// <returns>1 si está al inicio; 0 Falso. </returns>
    [DllImport(Dll)]
    internal static extern int fPosBOF();

    /// <summary>Informa si el registro activo se encuentra en el fin de la tabla de Documentos.</summary>
    /// <returns>1 si está al final; 0 Falso.</returns>
    [DllImport(Dll)]
    internal static extern int fPosEOF();

    #endregion

    #region Alto nivel – Lectura/Escritura

    /// <summary>Da de alta un documento y devuelve su identificador.</summary>
    /// <param name="aIdDocumento">Recibe el identificador asignado al documento creado.</param>
    /// <param name="aDocumento">Datos del documento que se dará de alta.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fAltaDocumento(ref int aIdDocumento, ref tDocumento aDocumento);
    
    /// <summary>Da de alta un documento de cargo o abono.</summary>
    /// <param name="aDocumento">Datos del nuevo documento.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fAltaDocumentoCargoAbono(ref tDocumento aDocumento);

    /// <summary>Aplica o retira la afectación del documento indicado por su llave.</summary>
    /// <param name="aLlaveDocto">Llave del documento.</param>
    /// <param name="aAfecta">True para afectar el documento; false para desafectarlo.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fAfectaDocto(ref tLlaveDoc aLlaveDocto, [MarshalAs(UnmanagedType.Bool)] bool aAfecta);

    /// <summary>Asocia los documentos indicados y aplica el importe del pago al documento por pagar.</summary>
    /// <param name="aDoctoaPagar">Llave del documento por pagar.</param>
    /// <param name="aDoctoPago">Llave del documento que realiza el pago.</param>
    /// <param name="aImporte">Importe del pago.</param>
    /// <param name="aIdMoneda">Identificador de la moneda del pago.</param>
    /// <param name="aFecha">Fecha del pago.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fSaldarDocumento(ref tLlaveDoc aDoctoaPagar, ref tLlaveDoc aDoctoPago, double aImporte, int aIdMoneda, [MarshalAs(UnmanagedType.LPStr)] string aFecha);

    /// <summary>Elimina la asociación entre el documento por pagar y el documento que lo pagó.</summary>
    /// <param name="aDoctoaPagar">Llave del documento por pagar.</param>
    /// <param name="aDoctoPago">Llave del documento que realiza el pago.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fBorrarAsociacion(ref tLlaveDoc aDoctoaPagar, ref tLlaveDoc aDoctoPago);

    /// <summary>Regresa el desglose de bases e IVA del documento de cargo indicado por su llave.</summary>
    /// <param name="aLlaveDocto">Llave del documento que se consultará.</param>
    /// <param name="aNetoTasa15">Recibe la base gravada al 15%.</param>
    /// <param name="aNetoTasa10">Recibe la base gravada al 10%.</param>
    /// <param name="aNetoTasaCero">Recibe la base gravada a tasa cero.</param>
    /// <param name="aNetoTasaExcenta">Recibe la base de productos exentos.</param>
    /// <param name="aNetoOtrasTasas">Recibe la base de otras tasas.</param>
    /// <param name="aIVATasa15">Recibe el IVA al 15%.</param>
    /// <param name="aIVATasa10">Recibe el IVA al 10%.</param>
    /// <param name="aIVAOtrasTasas">Recibe el IVA de otras tasas.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fRegresaIVACargo(ref tLlaveDoc aLlaveDocto,
        ref double aNetoTasa15, ref double aNetoTasa10, ref double aNetoTasaCero,
        ref double aNetoTasaExcenta, ref double aNetoOtrasTasas,
        ref double aIVATasa15, ref double aIVATasa10, ref double aIVAOtrasTasas);

    /// <summary>Obtiene los tamaños necesarios para los buffers de sello digital y cadena original.</summary>
    /// <param name="aPtrPassword">Contraseña del certificado.</param>
    /// <param name="aEspSelloDig">Recibe el tamaño requerido para el sello digital.</param>
    /// <param name="aEspCadOrig">Recibe el tamaño requerido para la cadena original.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fGetTamSelloDigitalYCadena(StringBuilder aPtrPassword, ref int aEspSelloDig, ref int aEspCadOrig);

    /// <summary>Obtiene el sello digital y la cadena original del CFD del documento previamente localizado.</summary>
    /// <param name="aPtrPassword">Contraseña del certificado.</param>
    /// <param name="aPtrSelloDigital">Buffer que recibe el sello digital.</param>
    /// <param name="aPtrCadenaOriginal">Buffer que recibe la cadena original.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// <remarks>Consulte primero <see cref="fGetTamSelloDigitalYCadena"/> para reservar buffers del tamaño requerido.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fGetSelloDigitalYCadena(StringBuilder aPtrPassword, StringBuilder aPtrSelloDigital, StringBuilder aPtrCadenaOriginal);

    /// <summary>Inicializa la consulta al servidor de licencias para el sistema indicado.</summary>
    /// <param name="aSistema">Identificador del sistema; el manual especifica 1 para CONTPAQi Factura Electrónica.</param>
    /// <returns>0 si obtuvo información del servidor; -1 si no pudo obtenerla.</returns>
    [DllImport(Dll)]
    internal static extern int fInicializaLicenseInfo(byte aSistema);

    /// <summary>Emite el CFD del documento indicado usando el certificado y complemento especificados.</summary>
    /// <param name="aCodConcepto">Código del concepto.</param><param name="aSerie">Serie del documento.</param>
    /// <param name="aFolio">Folio del documento.</param><param name="aPassword">Contraseña del certificado de sello digital.</param>
    /// <param name="aArchivoAdicional">Nombre del archivo de complemento existente en la carpeta Adicionales de la empresa.</param>
    /// <returns>0 si tuvo éxito, -1 si hubo un problema de licencia o un código positivo de error del SDK.</returns>
    /// <remarks>Debe llamarse primero a <see cref="fInicializaLicenseInfo"/>.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fEmitirDocumento(string aCodConcepto, string aSerie, double aFolio, StringBuilder aPassword, StringBuilder aArchivoAdicional);

    /// <summary>Obtiene el UUID del CFDI asociado al documento identificado por concepto, serie y folio.</summary>
    /// <param name="aCodConcepto">Código del concepto.</param><param name="aSerie">Serie del documento.</param>
    /// <param name="aFolio">Folio del documento.</param><param name="atPtrCFDIUUID">Buffer que recibe el UUID.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fDocumentoUUID(string aCodConcepto, string aSerie, double aFolio, StringBuilder atPtrCFDIUUID);

    /// <summary>Obtiene la serie del certificado utilizado por una factura electrónica.</summary>
    /// <param name="atPtrPassword">Contraseña del certificado.</param><param name="aPtrSerieCertificado">Buffer que recibe la serie del certificado.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fGetSerieCertificado(StringBuilder atPtrPassword, StringBuilder aPtrSerieCertificado);

    /// <summary>Configura si el SDK debe buscar el último precio de compra al registrar una compra con precio cero.</summary>
    /// <param name="aActivar">0 para no buscar; 1 para buscar el precio asumido.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fActivarPrecioCompra(int aActivar);

    /// <summary>Establece si el documento activo se marca como devuelto.</summary>
    /// <param name="aDevuelto">Valor entero que indica el estado devuelto.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fDocumentoDevuelto(int aDevuelto);

    /// <summary>Genera en disco la entrega del documento en XML o PDF.</summary>
    /// <param name="aCodConcepto">Código del concepto.</param><param name="aSerie">Serie del documento.</param>
    /// <param name="aFolio">Folio del documento.</param><param name="aFormato">0 para XML; 1 para PDF (también genera XML).</param>
    /// <param name="aFormatoAmig">Nombre de la plantilla de impresión.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fEntregEnDiscoXML(string aCodConcepto, string aSerie, double aFolio, int aFormato, StringBuilder aFormatoAmig);

    /// <summary>Prepara en el estado global del SDK los datos CFDI del documento previamente buscado.</summary>
    /// <param name="atPtrPassword">Contraseña del certificado.</param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    /// <remarks>Después de esta llamada, use <see cref="fLeeDatoCFDI"/> para recuperar los datos requeridos.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fObtieneDatosCFDI(StringBuilder atPtrPassword);

    /// <summary>Lee uno de los valores CFDI preparados previamente mediante <see cref="fObtieneDatosCFDI"/>.</summary>
    /// <param name="aValor">Buffer que recibirá el valor.</param>
    /// <param name="aDato">Código del dato CFDI solicitado
    /// 1 = Serie del Certificado del Emisor
    /// 2 = Folio Fiscal (UUID)
    /// 3 = Número de Serie del Certificado del SAT
    /// 4 = Fecha y Hora de Certificación
    /// 5 = Sello Digital del CFDI
    /// 6 = Sello SAT
    /// 7 = Cadena Original del Complemento de Certificación Digital del SAT
    /// 8 = Método de Pago
    /// 9 = Lugar de expedición
    /// 10 = Régimen Fiscal
    /// 11 = Folio Fiscal de origen*
    /// 12 = Serie del Folio Fiscal de origen*
    /// 13 = Fecha del Folio Fiscal de origen*
    /// 14 = Monto del Folio Fiscal de origen*
    /// * Para documentación de Deuda o Pago en Parcialidades
    /// </param>
   /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error que puede consultarse mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fLeeDatoCFDI(StringBuilder aValor, int aDato);

    #endregion

    #region Alto nivel - Búsqueda/Navegación

    /// <summary>Busca un documento por su llave y posiciona el registro correspondiente.</summary>
    /// <param name="aLlaveDocto">Llave compuesta por código del concepto, serie y folio.</param>
    /// <returns>0 si encontró el documento; otro valor es un código de error del SDK.</returns>
    [DllImport(Dll)]
    internal static extern int fBuscaDocumento(ref tLlaveDoc aLlaveDocto);

    #endregion
}