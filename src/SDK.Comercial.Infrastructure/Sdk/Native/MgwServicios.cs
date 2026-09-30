namespace SDK.Comercial.Infrastructure.Sdk.Native;

/// <summary>
/// Frontera de interoperabilidad directa con MGWServicios.dll.
/// La clase es parcial para organizar las funciones por área sin representar DLL distintas.
/// No contiene reglas de aplicación ni traduce códigos de retorno.
/// </summary>
internal static partial class MgwServicios
{
    private const string Dll = "MGWServicios.dll";

    /// <summary>Tamaño compartido de los buffers ANSI utilizados para leer cadenas del SDK.</summary>
    internal const int TamanoBuffer = 512;
}
