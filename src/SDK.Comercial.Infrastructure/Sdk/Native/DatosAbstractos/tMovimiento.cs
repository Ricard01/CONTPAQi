using System.Runtime.InteropServices;
using SDK.Comercial.Infrastructure.Sdk.Native.Constantes;

// ReSharper disable InconsistentNaming

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Estructura base de un movimiento (Movimientos – RegMovimiento – tMovimiento).</summary>
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
internal struct tMovimiento
{
    /// <summary>Consecutivo del movimiento.</summary>
    internal int aConsecutivo;

    /// <summary>Unidades del movimiento.</summary>
    internal double aUnidades;

    /// <summary>Precio del movimiento para documentos de venta.</summary>
    internal double aPrecio;

    /// <summary>Costo del movimiento para documentos de compra.</summary>
    internal double aCosto;

    /// <summary>Código del producto o servicio.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string aCodProdSer;

    /// <summary>Código del almacén.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string aCodAlmacen;

    /// <summary>Referencia del movimiento.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongReferencia + 1)]
    internal string aReferencia;

    /// <summary>Código de clasificación.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string aCodClasificacion;
}