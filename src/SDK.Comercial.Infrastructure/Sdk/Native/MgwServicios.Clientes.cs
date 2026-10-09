using System.Runtime.InteropServices;
using System.Text;
using SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

namespace SDK.Comercial.Infrastructure.Sdk.Native;

// FUNCIONES PARA CLIENTES Y PROVEEDORES.
internal static partial class MgwServicios
{
    #region Bajo nivel – Lectura/Escritura

    /// <summary>Inicia la inserción de un cliente o proveedor.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks> Se puede consultar el nombre de cada campo utilizable para las funciones<see cref="fLeeDatoCteProv"/>  y <see cref="fSetDatoCteProv"/> en el documento estructura de la BDD comercial tabla admClientes.
    /// Se puede asignar un valor a la gran mayoría de campos, algunos tienen restricciones que hay que cumplir y otros tantos como el ID no son editables </remarks>
    [DllImport(Dll)] 
    internal static extern int fInsertaCteProv();

    /// <summary>Activa la edición del cliente o proveedor posicionado.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks> Se puede consultar el nombre de cada campo utilizable para las funciones<see cref="fLeeDatoCteProv"/>  y <see cref="fSetDatoCteProv"/> en el documento estructura de la BDD comercial tabla admClientes.
    /// Se puede asignar un valor a la gran mayoría de campos, algunos tienen restricciones que hay que cumplir y otros tantos como el ID no son editables </remarks>
    [DllImport(Dll)] 
    internal static extern int fEditaCteProv();

    /// <summary>Guarda los cambios del cliente o proveedor activo.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    ///     /// <remarks> Se puede consultar el nombre de cada campo utilizable para las funciones<see cref="fLeeDatoCteProv"/>  y <see cref="fSetDatoCteProv"/> en el documento estructura de la BDD comercial tabla admClientes.
    /// Se puede asignar un valor a la gran mayoría de campos, algunos tienen restricciones que hay que cumplir y otros tantos como el ID no son editables </remarks>
    [DllImport(Dll)] 
    internal static extern int fGuardaCteProv();

    /// <summary>Borra el registro activo de cliente o proveedor.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks>Primero debe posicionarse el catálogo en el registro que se desea borrar.</remarks>
    [DllImport(Dll)] 
    internal static extern int fBorraCteProv();

    /// <summary>Descarta los cambios del cliente o proveedor en edición o inserción.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks> Se puede consultar el nombre de cada campo utilizable para las funciones<see cref="fLeeDatoCteProv"/>  y <see cref="fSetDatoCteProv"/> en el documento estructura de la BDD comercial tabla admClientes.
    /// Se puede asignar un valor a la gran mayoría de campos, algunos tienen restricciones que hay que cumplir y otros tantos como el ID no son editables </remarks>
    [DllImport(Dll, EntryPoint = "fCancelarModificacionCteProv")]
    internal static extern int fCancelarModificacionCteProv();

    /// <summary>Elimina un cliente o proveedor por su código.</summary>
    /// <param name="aCodigoCteProv">Código del cliente o proveedor.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fEliminarCteProv(string aCodigoCteProv);

    /// <summary>Escribe un valor en el campo indicado del cliente o proveedor activo.</summary>
    /// <param name="aCampo">Nombre del campo destino.</param><param name="aValor">Valor que se escribirá.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks> Se puede consultar el nombre de cada campo utilizable para las funciones<see cref="fLeeDatoCteProv"/>  y <see cref="fSetDatoCteProv"/> en el documento estructura de la BDD comercial tabla admClientes.
    /// Se puede asignar un valor a la gran mayoría de campos, algunos tienen restricciones que hay que cumplir y otros tantos como el ID no son editables </remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fSetDatoCteProv(string aCampo, string aValor);

    /// <summary>Lee el campo indicado del cliente o proveedor activo.</summary>
    /// <param name="aCampo">Nombre del campo que se leerá.</param><param name="aValor">Buffer que recibe el valor.</param>
    /// <param name="aLen">Longitud disponible en el buffer.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks> Se puede consultar el nombre de cada campo utilizable para las funciones<see cref="fLeeDatoCteProv"/>  y <see cref="fSetDatoCteProv"/> en el documento estructura de la BDD comercial tabla admClientes.
    /// Se puede asignar un valor a la gran mayoría de campos, algunos tienen restricciones que hay que cumplir y otros tantos como el ID no son editables </remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fLeeDatoCteProv(string aCampo, StringBuilder aValor, int aLen);

    #endregion

    #region Bajo nivel – Búsqueda/Navegación

    /// <summary>Busca un cliente o proveedor por código y posiciona el registro encontrado.</summary>
    /// <param name="aCodCteProv">Código del cliente o proveedor.</param>
    /// <returns><c>0</c> si lo encontró; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    /// <remarks> Se puede consultar el nombre de cada campo utilizable para las funciones<see cref="fLeeDatoCteProv"/>  y <see cref="fSetDatoCteProv"/> en el documento estructura de la BDD comercial tabla admClientes.
    /// Se puede asignar un valor a la gran mayoría de campos, algunos tienen restricciones que hay que cumplir y otros tantos como el ID no son editables </remarks>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fBuscaCteProv(string aCodCteProv);

    /// <summary>Busca un cliente o proveedor por identificador y posiciona el registro encontrado.</summary>
    /// <param name="aIdCteProv">Identificador del cliente o proveedor.</param>
    /// <returns><c>0</c> si lo encontró; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fBuscaIdCteProv(int aIdCteProv);

    /// <summary>Posiciona el registro activo en el primer cliente o proveedor.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosPrimerCteProv();
    
    /// <summary>Posiciona el registro activo en el último cliente o proveedor.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosUltimoCteProv();
    
    /// <summary>Avanza al cliente o proveedor siguiente.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosSiguienteCteProv();
    
    /// <summary>Retrocede al cliente o proveedor anterior.</summary>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosAnteriorCteProv();
    
    /// <summary>Indica si el registro activo está al inicio del catálogo.</summary>
    /// <returns>1 si está al inicio; 0 si no.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosBOFCteProv();
    
    /// <summary>Indica si el registro activo está al final del catálogo.</summary>
    /// <returns>1 si está al final; 0 si no.</returns>
    [DllImport(Dll)] 
    internal static extern int fPosEOFCteProv();

    #endregion

    #region Alto nivel – Lectura/Escritura

    /// <summary>Da de alta un cliente o proveedor a partir de su estructura.</summary>
    /// <param name="aIdCteProv">Recibe el identificador asignado al nuevo registro.</param>
    /// <param name="astTCteProv">Datos del cliente o proveedor.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fAltaCteProv(ref int aIdCteProv, ref tCteProv astTCteProv);

    /// <summary>Actualiza el cliente o proveedor identificado por su código.</summary>
    /// <param name="aCodigoCteProv">Código del registro que se actualizará.</param>
    /// <param name="astTCteProv">Nuevos datos del cliente o proveedor.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll, CharSet = CharSet.Ansi)]
    internal static extern int fActualizaCteProv(StringBuilder aCodigoCteProv, ref tCteProv astTCteProv);

    /// <summary>Copia los campos de la estructura al registro activo de cliente o proveedor.</summary>
    /// <param name="astTCteProv">Estructura con los datos que se asignarán.</param>
    /// <param name="aEsAlta">1 para un registro nuevo; 2 para actualizar uno existente.</param>
    /// <returns><c>0</c> si tuvo éxito; otro valor es un código de error consultable mediante <see cref="fError"/>.</returns>
    [DllImport(Dll)]
    internal static extern int fLlenaRegistroCteProv(ref tCteProv astTCteProv, int aEsAlta);

    #endregion
}
