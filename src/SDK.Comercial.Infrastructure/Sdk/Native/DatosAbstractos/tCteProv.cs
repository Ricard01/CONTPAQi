using System.Runtime.InteropServices;
using SDK.Comercial.Infrastructure.Sdk.Native.Constantes;

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Datos de cliente/proveedor (RegCteProv / TcteProv) documentados por el SDK.</summary>
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
internal struct tCteProv
{
    /// <summary>Código de cliente o proveedor.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string cCodigoCliente;

    /// <summary>Razón social.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongNombre + 1)]
    internal string cRazonSocial;

    /// <summary>Fecha de alta.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongFecha + 1)]
    internal string cFechaAlta;

    /// <summary>RFC.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongRFC + 1)]
    internal string cRFC;

    /// <summary>CURP.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCURP + 1)]
    internal string cCURP;

    /// <summary>Denominación comercial.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDenComercial + 1)]
    internal string cDenComercial;

    /// <summary>Representante legal.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongRepLegal + 1)]
    internal string cRepLegal;

    /// <summary>Nombre de la moneda.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongNombre + 1)]
    internal string cNombreMoneda;

    /// <summary>Lista de precios del cliente.</summary> internal int cListaPreciosCliente;
    /// <summary>Descuento de movimiento.</summary> internal double cDescuentoMovto;
    /// <summary>Permite ventas a crédito: 0 no, 1 sí.</summary> internal int cBanVentaCredito;
    /// <summary>Código de clasificación de cliente 1.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacionCliente1;

    /// <summary>Código de clasificación de cliente 2.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacionCliente2;

    /// <summary>Código de clasificación de cliente 3.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacionCliente3;

    /// <summary>Código de clasificación de cliente 4.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacionCliente4;

    /// <summary>Código de clasificación de cliente 5.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacionCliente5;

    /// <summary>Código de clasificación de cliente 6.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacionCliente6;

    /// <summary>Tipo: 1 cliente, 2 cliente/proveedor, 3 proveedor.</summary> internal int cTipoCliente;
    /// <summary>Estado: 0 inactivo, 1 activo.</summary> internal int cEstatus;
    /// <summary>Fecha de baja.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongFecha + 1)]
    internal string cFechaBaja;

    /// <summary>Fecha de última revisión.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongFecha + 1)]
    internal string cFechaUltimaRevision;

    /// <summary>Límite de crédito del cliente.</summary> internal double cLimiteCreditoCliente;
    /// <summary>Días de crédito del cliente.</summary> internal int cDiasCreditoCliente;
    /// <summary>Permite exceder el crédito: 0 no, 1 sí.</summary> internal int cBanExcederCredito;
    /// <summary>Descuento por pronto pago.</summary> internal double cDescuentoProntoPago;
    /// <summary>Días para pronto pago.</summary> internal int cDiasProntoPago;
    /// <summary>Interés moratorio.</summary> internal double cInteresMoratorio;
    /// <summary>Día de pago.</summary> internal int cDiaPago;
    /// <summary>Días de revisión.</summary> internal int cDiasRevision;
    /// <summary>Mensajería.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDesCorta + 1)]
    internal string cMensajeria;

    /// <summary>Cuenta de mensajería.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string cCuentaMensajeria;

    /// <summary>Días de embarque del cliente.</summary> internal int cDiasEmbarqueCliente;
    /// <summary>Código del almacén.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string cCodigoAlmacen;

    /// <summary>Código del agente de venta.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string cCodigoAgenteVenta;

    /// <summary>Código del agente de cobro.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string cCodigoAgenteCobro;

    /// <summary>Restricción de agente.</summary> internal int cRestriccionAgente;
    /// <summary>Impuesto 1.</summary> internal double cImpuesto1;
    /// <summary>Impuesto 2.</summary> internal double cImpuesto2;
    /// <summary>Impuesto 3.</summary> internal double cImpuesto3;
    /// <summary>Retención aplicable al cliente 1.</summary> internal double cRetencionCliente1;
    /// <summary>Retención aplicable al cliente 2.</summary> internal double cRetencionCliente2;
    /// <summary>Código de clasificación de proveedor 1.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacionProveedor1;

    /// <summary>Código de clasificación de proveedor 2.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacionProveedor2;

    /// <summary>Código de clasificación de proveedor 3.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacionProveedor3;

    /// <summary>Código de clasificación de proveedor 4.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacionProveedor4;

    /// <summary>Código de clasificación de proveedor 5.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacionProveedor5;

    /// <summary>Código de clasificación de proveedor 6.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacionProveedor6;

    /// <summary>Límite de crédito del proveedor.</summary> internal double cLimiteCreditoProveedor;
    /// <summary>Días de crédito del proveedor.</summary> internal int cDiasCreditoProveedor;
    /// <summary>Tiempo de entrega del proveedor.</summary> internal int cTiempoEntrega;
    /// <summary>Días de embarque del proveedor.</summary> internal int cDiasEmbarqueProveedor;
    /// <summary>Impuesto de proveedor 1.</summary> internal double cImpuestoProveedor1;
    /// <summary>Impuesto de proveedor 2.</summary> internal double cImpuestoProveedor2;
    /// <summary>Impuesto de proveedor 3.</summary> internal double cImpuestoProveedor3;
    /// <summary>Retención de proveedor 1.</summary> internal double cRetencionProveedor1;
    /// <summary>Retención de proveedor 2.</summary> internal double cRetencionProveedor2;
    /// <summary>Permite calcular interés moratorio: 0 no, 1 sí.</summary> internal int cBanInteresMoratorio;
    /// <summary>Texto extra 1.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongTextoExtra + 1)]
    internal string cTextoExtra1;

    /// <summary>Texto extra 2.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongTextoExtra + 1)]
    internal string cTextoExtra2;

    /// <summary>Texto extra 3.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongTextoExtra + 1)]
    internal string cTextoExtra3;

    /// <summary>Fecha extra.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongFecha + 1)]
    internal string cFechaExtra;
    /// <summary>Importe extra 1.</summary> internal double cImporteExtra1;
    /// <summary>Importe extra 2.</summary> internal double cImporteExtra2;
    /// <summary>Importe extra 3.</summary> internal double cImporteExtra3;
    /// <summary>Importe extra 4.</summary> internal double cImporteExtra4;
}