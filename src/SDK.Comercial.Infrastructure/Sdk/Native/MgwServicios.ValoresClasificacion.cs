using System.Runtime.InteropServices;
using System.Text;
using SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

// FUNCIONES PARA VALORES DE CLASIFICACIÓN
internal static partial class MgwServicios
{
    #region Bajo nivel – Lectura/Escritura
    
    /// <summary>Inicia la inserción del valor de clasificación posicionado.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fInsertaValorClasif();
    
    /// <summary>Activa la edición del valor de clasificación posicionado.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fEditaValorClasif();
    
    /// <summary>Guarda los cambios del valor de clasificación activo.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fGuardaValorClasif();
    
    /// <summary>Borra el valor de clasificación activo.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fBorraValorClasif();
    
    /// <summary>Descarta cambios del valor de clasificación en edición o inserción.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, EntryPoint = "fCancelarModificacionValorClasif")] 
    internal static extern int fCancelarModificacionValorClasif();
    
    /// <summary>Elimina un valor de clasificación por clasificación, número y código.</summary><param name="aClasificacionDe">Tipo de entidad al que pertenece.</param><param name="aNumClasificacion">Número de clasificación.</param><param name="aCodValorClasif">Código del valor.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fEliminarValorClasif(int aClasificacionDe, int aNumClasificacion, string aCodValorClasif);
    
    /// <summary>Escribe un campo del valor de clasificación activo.</summary><param name="aCampo">Nombre del campo.</param><param name="aValor">Valor que se escribirá.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fSetDatoValorClasif(string aCampo, string aValor);
    
    /// <summary>Lee un campo del valor de clasificación activo.</summary><param name="aCampo">Nombre del campo.</param><param name="aValor">Buffer que recibe el campo.</param><param name="aLen">Longitud disponible.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)] 
    internal static extern int fLeeDatoValorClasif(string aCampo, StringBuilder aValor, int aLen);
    #endregion

    #region Bajo nivel – Búsqueda/Navegación
    
    /// <summary>Busca un valor por clasificación, número y código.</summary><param name="aClasificacionDe">Tipo de entidad al que pertenece.</param><param name="aNumClasificacion">Número de clasificación.</param><param name="aCodValorClasif">Código del valor.</param>
    /// <returns><c>0</c> si lo encontró; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>El PDF presenta la firma de <c>fBuscaClasificacion</c> en esta sección; por el número y código de valor documentados se declara como búsqueda de valor de clasificación.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi, EntryPoint = "fBuscaValorClasif")] 
    internal static extern int fBuscaValorClasif(int aClasificacionDe, int aNumClasificacion, string aCodValorClasif);
    
    /// <summary>Busca un valor de clasificación por identificador.</summary><param name="aIdValorClasif">Identificador del valor.</param>
    /// <returns><c>0</c> si lo encontró; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fBuscaIdValorClasif(int aIdValorClasif);
    
    /// <summary>Posiciona el primer valor de clasificación.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosPrimerValorClasif();
    
    /// <summary>Posiciona el último valor de clasificación.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosUltimoValorClasif();
    
    /// <summary>Avanza al valor de clasificación siguiente.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosSiguienteValorClasif();
    
    /// <summary>Retrocede al valor de clasificación anterior.</summary><returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosAnteriorValorClasif();
    
    /// <summary>Indica si el registro activo está al inicio del catálogo.</summary><returns>1 si está al inicio; 0 si no.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosBOFValorClasif();
    
    /// <summary>Indica si el registro activo está al final del catálogo.</summary><returns>1 si está al final; 0 si no.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosEOFValorClasif();
    #endregion

    #region Alto nivel – Lectura/Escritura
    
    /// <summary>Da de alta un valor de clasificación.</summary><param name="aIdValorClasif">Recibe el identificador asignado.</param><param name="astValorClasif">Datos del valor.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fAltaValorClasif(ref int aIdValorClasif, ref TValorClasificacion astValorClasif);
    
    /// <summary>Actualiza un valor de clasificación identificado por código.</summary><param name="aCodigoValorClasif">Código del valor que se actualizará.</param><param name="astValorClasif">Nuevos datos del valor.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>El nombre de función de la sintaxis del PDF parece copiado de la búsqueda de clasificaciones; se conserva aquí la operación descrita como actualización de valor.</remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi, EntryPoint = "fActualizaValorClasif")] 
    internal static extern int fActualizaValorClasif(string aCodigoValorClasif, ref TValorClasificacion astValorClasif);
    
    /// <summary>Copia los campos de la estructura al registro activo del valor de clasificación.</summary><param name="astValorClasif">Datos que se asignarán.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fLlenaRegistroValorClasif(ref TValorClasificacion astValorClasif);
    #endregion
}
