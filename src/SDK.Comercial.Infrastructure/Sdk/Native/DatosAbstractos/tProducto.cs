using System.Runtime.InteropServices;
using SDK.Comercial.Infrastructure.Sdk.Native.Constantes;

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Datos de producto (RegProducto / tProducto) según el manual del SDK.</summary>
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
internal struct tProducto
{
    /// <summary>Código del producto.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)] internal string cCodigoProducto;
    /// <summary>Nombre del producto.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongNombre + 1)] internal string cNombreProducto;
    /// <summary>Descripción del producto.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongNombreProducto + 1)] internal string cDescripcionProducto;
    /// <summary>Tipo: 1 producto, 2 paquete, 3 servicio.</summary>
    internal int cTipoProducto;
    /// <summary>Fecha de alta.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongFecha + 1)] internal string cFechaAltaProducto;
    /// <summary>Fecha de baja.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongFecha + 1)] internal string cFechaBaja;
    /// <summary>Estado: 0 baja lógica, 1 alta.</summary>
    internal int cStatusProducto;
    /// <summary>Control de existencia.</summary>
    internal int cControlExistencia;
    /// <summary>Método de costeo: 1 promedio entradas; 2 promedio entradas por almacén; 3 último costo; 4 UEPS; 5 PEPS; 6 específico; 7 estándar.</summary>
    internal int cMetodoCosteo;
    /// <summary>Código de unidad base.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)] internal string cCodigoUnidadBase;
    /// <summary>Código de unidad no convertible.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)] internal string cCodigoUnidadNoConvertible;
    /// <summary>Precio de lista 1.</summary> internal double cPrecio1;
    /// <summary>Precio de lista 2.</summary> internal double cPrecio2;
    /// <summary>Precio de lista 3.</summary> internal double cPrecio3;
    /// <summary>Precio de lista 4.</summary> internal double cPrecio4;
    /// <summary>Precio de lista 5.</summary> internal double cPrecio5;
    /// <summary>Precio de lista 6.</summary> internal double cPrecio6;
    /// <summary>Precio de lista 7.</summary> internal double cPrecio7;
    /// <summary>Precio de lista 8.</summary> internal double cPrecio8;
    /// <summary>Precio de lista 9.</summary> internal double cPrecio9;
    /// <summary>Precio de lista 10.</summary> internal double cPrecio10;
    /// <summary>Impuesto 1.</summary> internal double cImpuesto1;
    /// <summary>Impuesto 2.</summary> internal double cImpuesto2;
    /// <summary>Impuesto 3.</summary> internal double cImpuesto3;
    /// <summary>Retención 1.</summary> internal double cRetencion1;
    /// <summary>Retención 2.</summary> internal double cRetencion2;
    /// <summary>Nombre de la característica 1.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongAbreviatura + 1)] internal string cNombreCaracteristica1;
    /// <summary>Nombre de la característica 2.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongAbreviatura + 1)] internal string cNombreCaracteristica2;
    /// <summary>Nombre de la característica 3.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongAbreviatura + 1)] internal string cNombreCaracteristica3;
    /// <summary>Código de valor de clasificación 1.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)] internal string cCodigoValorClasificacion1;
    /// <summary>Código de valor de clasificación 2.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)] internal string cCodigoValorClasificacion2;
    /// <summary>Código de valor de clasificación 3.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)] internal string cCodigoValorClasificacion3;
    /// <summary>Código de valor de clasificación 4.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)] internal string cCodigoValorClasificacion4;
    /// <summary>Código de valor de clasificación 5.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)] internal string cCodigoValorClasificacion5;
    /// <summary>Código de valor de clasificación 6.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)] internal string cCodigoValorClasificacion6;
    /// <summary>Texto extra 1.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongTextoExtra + 1)] internal string cTextoExtra1;
    /// <summary>Texto extra 2.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongTextoExtra + 1)] internal string cTextoExtra2;
    /// <summary>Texto extra 3.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongTextoExtra + 1)] internal string cTextoExtra3;
    /// <summary>Fecha extra.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongFecha + 1)] internal string cFechaExtra;
    /// <summary>Importe extra 1.</summary> internal double cImporteExtra1;
    /// <summary>Importe extra 2.</summary> internal double cImporteExtra2;
    /// <summary>Importe extra 3.</summary> internal double cImporteExtra3;
    /// <summary>Importe extra 4.</summary> internal double cImporteExtra4;
}
