using System.Runtime.InteropServices;
using System.Text;
using SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

// FUNCIONES PARA CONSULTAR Y MANTENER DIRECCIONES DE EMPRESA, CLIENTES Y DOCUMENTOS.
internal static partial class MgwServicios
{
    #region Bajo nivel – Lectura/Escritura

    /// <summary>Inicia la inserción de una dirección.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fInsertaDireccion();

    /// <summary>Activa la edición de la dirección posicionada.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fEditaDireccion();

    /// <summary>Guarda los cambios realizados a la dirección activa.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fGuardaDireccion();

    /// <summary>Descarta las modificaciones de la dirección activa.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, EntryPoint = "fCancelarModificacionDireccion")]
    internal static extern int fCancelarModificacionDireccion();

    /// <summary>Lee un campo de la dirección activa.</summary>
    /// <param name="aCampo">Nombre del campo.</param><param name="aValor">Buffer que recibe el valor.</param>
    /// <param name="aLen">Longitud disponible del buffer.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fLeeDatoDireccion(string aCampo, StringBuilder aValor, int aLen);

    /// <summary>Escribe un valor en un campo de la dirección activa.</summary>
    /// <param name="aCampo">Nombre del campo.</param><param name="aValor">Valor que se escribirá.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fSetDatoDireccion(string aCampo, string aValor);

    #endregion

    #region Bajo nivel – Búsqueda/Navegación

    /// <summary>Busca y posiciona la dirección de la empresa.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fBuscaDireccionEmpresa();

    /// <summary>Busca la dirección de un cliente o proveedor.</summary>
    /// <param name="aCodCteProv">Código del cliente o proveedor.</param>
    /// <param name="aTipoDireccion">Tipo de dirección: 0 fiscal, 1 envío.</param>
    /// <returns><c>0</c> si encontró la dirección; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>La tabla de parámetros del PDF contiene nombres genéricos y tipos inconsistentes; se usan los nombres de la sintaxis y el tipo entero para la opción 0/1.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fBuscaDireccionCteProv(string aCodCteProv, int aTipoDireccion);

    /// <summary>Busca la dirección asociada al documento indicado.</summary>
    /// <param name="aIdDocumento">Identificador del documento.</param>
    /// <param name="aTipoDireccion">Tipo de dirección: 0 fiscal, 1 envío.</param>
    /// <returns><c>0</c> si encontró la dirección; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>El PDF documenta el selector de dirección como valor 0/1, aunque la fila de parámetros lo etiqueta como cadena.</remarks>
    [DllImport(Dll)]
    internal static extern int fBuscaDireccionDocumento(int aIdDocumento, int aTipoDireccion);

    /// <summary>Posiciona la tabla de direcciones en el primer registro.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosPrimerDireccion();
    
    /// <summary>Posiciona la tabla de direcciones en el último registro.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosUltimaDireccion();
    
    /// <summary>Avanza al siguiente registro de dirección.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosSiguienteDireccion();
    
    /// <summary>Retrocede al registro de dirección anterior.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosAnteriorDireccion();
    
    /// <summary>Indica si el registro activo está al inicio de la tabla.</summary>
    /// <returns>1 si está al inicio; 0 si no.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosBOFDireccion();
    
    /// <summary>Indica si el registro activo está al final de la tabla.</summary>
    /// <returns>1 si está al final; 0 si no.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosEOFDireccion();

    #endregion

    #region Alto nivel – Lectura/Escritura

    /// <summary>Da de alta una dirección a partir de la estructura indicada.</summary>
    /// <param name="aIdDireccion">Recibe el identificador asignado a la nueva dirección.</param>
    /// <param name="astDireccion">Datos de la dirección.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks><see cref="tDireccion.cTipoDireccion"/> debe ser 1 para domicilio fiscal o 2 para domicilio de envío.</remarks>
    [DllImport(Dll)]
    internal static extern int fAltaDireccion(ref int aIdDireccion, tDireccion astDireccion);

    /// <summary>Actualiza la dirección asociada al registro activo de cliente o proveedor.</summary>
    /// <param name="astDireccion">Datos actualizados de la dirección.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>La sintaxis del PDF repite por error el nombre de otra función; se usa el encabezado y la descripción de <c>fActualizaDireccion</c>. El campo <see cref="tDireccion.cTipoDireccion"/> debe ser 1 fiscal o 2 envío.</remarks>
    [DllImport(Dll)]
    internal static extern int fActualizaDireccion(tDireccion astDireccion);

    /// <summary>Copia los campos de la estructura al registro de dirección activo.</summary>
    /// <param name="astDireccion">Estructura con los datos de la dirección.</param>
    /// <param name="aEsAlta">1 para alta; 2 para actualización.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks><see cref="tDireccion.cTipoDireccion"/> debe ser 1 para domicilio fiscal o 2 para domicilio de envío.</remarks>
    [DllImport(Dll)]
    internal static extern int fLlenaRegistroDireccion(tDireccion astDireccion, int aEsAlta);

    #endregion
}
