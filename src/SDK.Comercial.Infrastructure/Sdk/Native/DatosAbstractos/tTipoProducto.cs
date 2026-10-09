using System.Runtime.InteropServices;

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Datos adicionales de producto en un movimiento (RegTipoProducto / tTipoProducto).</summary>
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
internal struct tTipoProducto
{
    /// <summary>Datos de series y capas (estructura <see cref="tSeriesCapas"/>).</summary>
    internal tSeriesCapas aSeriesCapas;

    /// <summary>Datos de características (estructura <see cref="tCaracteristicas"/>).</summary>
    internal tCaracteristicas aCaracteristicas;
}