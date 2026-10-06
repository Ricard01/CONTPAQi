using System.Runtime.InteropServices;
using System.Text;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

// FUNCIONES DE BÚSQUEDA, NAVEGACIÓN Y LECTURA/ESCRITURA DE CONCEPTOS DE DOCUMENTO.
internal static partial class MgwServicios
{
    #region Bajo nivel – Lectura/Escritura

    /// <summary>Lee un campo del registro de concepto activo.</summary>
    /// <param name="aCampo">Nombre del campo.</param><param name="aValor">Buffer que recibe el valor.</param>
    /// <param name="aLen">Longitud disponible del buffer.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fLeeDatoConceptoDocto(string aCampo, StringBuilder aValor, int aLen);

    /// <summary>Obtiene el porcentaje de impuesto para el concepto, cliente/proveedor y producto indicados.</summary>
    /// <param name="aIdConceptoDocumento">Identificador del concepto de documento.</param>
    /// <param name="aIdClienteProveedor">Identificador del cliente o proveedor.</param>
    /// <param name="aIdProducto">Identificador del producto.</param>
    /// <param name="aPorcentajeImpuesto">Recibe el porcentaje calculado.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>El SDK obtiene la configuración del concepto y consulta el porcentaje en Clientes/Proveedores, Productos o Parámetros generales.</remarks>
    [DllImport(Dll)]
    internal static extern int fRegresPorcentajeImpuesto(int aIdConceptoDocumento, int aIdClienteProveedor, int aIdProducto, ref double aPorcentajeImpuesto);

    /// <summary>Activa el modo de edición del concepto posicionado.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fEditaConceptoDocto();

    /// <summary>Escribe un valor en un campo del concepto activo.</summary>
    /// <param name="aCampo">Nombre del campo.</param><param name="aValor">Valor que se escribirá.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fSetDatoConceptoDocto(string aCampo, string aValor);

    /// <summary>Guarda los cambios hechos al concepto activo.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fGuardaConceptoDocto();

    #endregion

    #region Bajo nivel – Búsqueda/Navegación

    /// <summary>Busca un concepto de documento por su código y posiciona el registro encontrado.</summary>
    /// <param name="aCodConcepto">Código del concepto.</param>
    /// <returns><c>0</c> si lo encontró; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fBuscaConceptoDocto(string aCodConcepto);

    /// <summary>Busca un concepto de documento por su identificador.</summary>
    /// <param name="aIdConcepto">Identificador del concepto.</param>
    /// <returns><c>0</c> si lo encontró; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fBuscaIdConceptoDocto(int aIdConcepto);

    /// <summary>Posiciona la tabla en el primer concepto de documento.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosPrimerConceptoDocto();
    
    /// <summary>Posiciona la tabla en el último concepto de documento.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosUltimaConceptoDocto();
    
    /// <summary>Avanza al concepto de documento siguiente.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosSiguienteConceptoDocto();
    
    /// <summary>Retrocede al concepto de documento anterior.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosAnteriorConceptoDocto();
    
    /// <summary>Indica si el registro activo está al inicio de la tabla de conceptos.</summary>
    /// <returns>1 si está al inicio; 0 si no.</returns>
    /// <remarks>La sintaxis de esta función aparece omitida en el PDF, pero su encabezado y la función EOF confirman que no recibe parámetros.</remarks>
    [DllImport(Dll)] 
    internal static extern int fPosBOFConceptoDocto();
    
    /// <summary>Indica si el registro activo está al final de la tabla de conceptos.</summary>
    /// <returns>1 si está al final; 0 si no.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosEOFConceptoDocto();

    #endregion
}
