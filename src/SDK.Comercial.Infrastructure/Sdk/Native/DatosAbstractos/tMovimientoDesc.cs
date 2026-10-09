using System.Runtime.InteropServices;
using SDK.Comercial.Infrastructure.Sdk.Native.Constantes;

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Estructura de movimiento con descuentos (Movimientos – RegMovimiento – tMovimientoDesc).</summary>
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
internal struct tMovimientoDesc
{
    /// <summary>Consecutivo del movimiento.</summary>
    internal int aConsecutivo;

    /// <summary>Unidades del movimiento.</summary>
    internal double aUnidades;

    /// <summary>Precio del movimiento para documentos de venta.</summary>
    internal double aPrecio;

    /// <summary>Costo del movimiento para documentos de compra.</summary>
    internal double aCosto;

    /// <summary>Porcentaje del descuento 1.</summary>
    internal double aPorcDescto1;

    /// <summary>Importe del descuento 1.</summary>
    internal double aImporteDescto1;

    /// <summary>Porcentaje del descuento 2.</summary>
    internal double aPorcDescto2;

    /// <summary>Importe del descuento 2.</summary>
    internal double aImporteDescto2;

    /// <summary>Porcentaje del descuento 3.</summary>
    internal double aPorcDescto3;

    /// <summary>Importe del descuento 3.</summary>
    internal double aImporteDescto3;

    /// <summary>Porcentaje del descuento 4.</summary>
    internal double aPorcDescto4;

    /// <summary>Importe del descuento 4.</summary>
    internal double aImporteDescto4;

    /// <summary>Porcentaje del descuento 5.</summary>
    internal double aPorcDescto5;

    /// <summary>Importe del descuento 5.</summary>
    internal double aImporteDescto5;

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