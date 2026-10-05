namespace SDK.Comercial.Infrastructure.Sdk.Native;

/// <summary>
/// Es la interfase del SDK con Comercial Premium. Libreria de encadenado, aquí se encuentran las funciones del SDK.
/// </summary>
internal static partial class MgwServicios
{
    private const string Dll = "MGWServicios.dll";

    /// <summary>Tamaño compartido de los buffers ANSI utilizados para leer cadenas del SDK.</summary>
    internal const int TamanoBuffer = 512;
}
