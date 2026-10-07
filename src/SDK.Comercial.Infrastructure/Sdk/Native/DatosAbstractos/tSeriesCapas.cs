using System.Runtime.InteropServices;
using SDK.Comercial.Infrastructure.Sdk.Native.Constantes;

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Estructura para movimientos con series y capas (SeriesCapas / tSeriesCapas).</summary>
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
internal struct tSeriesCapas
{
    /// <summary>Unidades del movimiento.</summary>
    internal double aUnidades;
    /// <summary>Tipo de cambio del movimiento.</summary>
    internal double aTipoCambio;
    /// <summary>Series del movimiento.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string aSeries;
    /// <summary>Pedimento.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string aPedimento;
    /// <summary>Agencia aduanal.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string aAgencia;
    /// <summary>Fecha del pedimento.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongFecha + 1)]
    internal string aFechaPedimento;
    /// <summary>Número de lote.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string aNumeroLote;
    /// <summary>Fecha de fabricación.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongFecha + 1)]
    internal string aFechaFabricacion;
    /// <summary>Fecha de caducidad.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongFecha + 1)]
    internal string aFechaCaducidad;
}
