using System.Runtime.InteropServices;
using SDK.Comercial.Infrastructure.Sdk.Native.Constantes;

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Datos de características vinculados a unidades de compra/venta.</summary>
/// <remarks>
/// <see cref="MgwServicios.fAltaMovimientoCaracteristicas"/> implimenta esta estructura, pero no incluye una tabla de tipos y longitudes para esta estructura.
/// La representación usa los tipos homólogos de <see cref="tMovimientoCaract"/> y <see cref="tUnidad"/>.
/// Confirme el layout con los headers del SDK instalado antes de usar esta firma en producción.
/// </remarks>
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
internal struct tCaracteristicasUnidades
{
    /// <summary>Abreviatura de la unidad de compra/venta.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongAbreviatura + 1)]
    internal string aUnidad;

    /// <summary>Unidades del movimiento con características.</summary>
    internal double aUnidades;

    /// <summary>Abreviatura de la unidad de compra/venta no convertible.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongAbreviatura + 1)]
    internal string aUnidadesNC;

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