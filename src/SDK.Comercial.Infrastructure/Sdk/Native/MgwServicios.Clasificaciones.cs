using System.Runtime.InteropServices;
using System.Text;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

// FUNCIONES PARA CONSULTAR Y ACTUALIZAR CLASIFICACIONES.
internal static partial class MgwServicios
{
    #region Bajo nivel – Lectura/Escritura

    /// <summary>Activa la edición de la clasificación posicionada.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fEditaClasificacion();
    
    /// <summary>Guarda las modificaciones de la clasificación activa.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fGuardaClasificacion();
    
    /// <summary>Descarta los cambios de la clasificación en edición o inserción.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, EntryPoint = "fCancelarModificacionClasificacion")]
    internal static extern int fCancelarModificacionClasificacion();

    /// <summary>Actualiza el nombre de la clasificación identificada por tipo y número.</summary>
    /// <param name="aClasificacionDe">Tipo: 1 agente, 2 cliente, 3 proveedor, 4 almacén, 5 producto.</param>
    /// <param name="aNumClasificacion">Número de clasificación, del 1 al 6.</param>
    /// <param name="aNombreClasificacion">Nombre que se asignará.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>El texto descriptivo del PDF menciona por error una dirección de cliente/proveedor; la sintaxis y los parámetros describen una actualización de clasificación.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fActualizaClasificacion(int aClasificacionDe, int aNumClasificacion, string aNombreClasificacion);

    /// <summary>Lee el campo indicado de la clasificación activa.</summary>
    /// <param name="aCampo">Nombre del campo.</param><param name="aValor">Buffer que recibirá el valor.</param>
    /// <param name="aLen">Longitud disponible del buffer.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fLeeDatoClasificacion(string aCampo, StringBuilder aValor, int aLen);

    /// <summary>Escribe un valor en el campo indicado de la clasificación activa.</summary>
    /// <param name="aCampo">Nombre del campo.</param><param name="aValor">Valor que se escribirá.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fSetDatoClasificacion(string aCampo, string aValor);

    #endregion

    #region Bajo nivel – Búsqueda/Navegación

    /// <summary>Busca la clasificación por tipo y número, y posiciona el registro encontrado.</summary>
    /// <param name="aClasificacionDe">Tipo: 1 agente, 2 cliente, 3 proveedor, 4 almacén, 5 producto.</param>
    /// <param name="aNumClasificacion">Número de clasificación, del 1 al 6.</param>
    /// <returns><c>0</c> si encontró el registro; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fBuscaClasificacion(int aClasificacionDe, int aNumClasificacion);

    /// <summary>Posiciona la tabla en la primera clasificación.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosPrimerClasificacion();
    
    /// <summary>Posiciona la tabla en la última clasificación.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosUltimoClasificacion();
    
    /// <summary>Avanza a la clasificación siguiente.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosSiguienteClasificacion();
    
    /// <summary>Retrocede a la clasificación anterior.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosAnteriorClasificacion();
    
    /// <summary>Indica si el registro activo está al inicio de la tabla.</summary>
    /// <returns>1 si está al inicio; 0 si no.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosBOFClasificacion();
    
    /// <summary>Indica si el registro activo está al final de la tabla.</summary>
    /// <returns>1 si está al final; 0 si no.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosEOFClasificacion();

    #endregion
}
