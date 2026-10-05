using System.Runtime.InteropServices;
using SDK.Comercial.Infrastructure.Sdk.Native.Constantes;

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Valores de características de un movimiento (Caracteristicas / tCaracteristicas).</summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
internal struct tCaracteristicas
{
    /// <summary>Unidades del movimiento.</summary>
    internal double aUnidades;
    /// <summary>Valor de la característica 1.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string aValorCaracteristica1;
    /// <summary>Valor de la característica 2.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string aValorCaracteristica2;
    /// <summary>Valor de la característica 3.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string aValorCaracteristica3;
}
