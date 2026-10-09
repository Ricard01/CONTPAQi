using System.Runtime.InteropServices;
using SDK.Comercial.Infrastructure.Sdk.Native.Constantes;

// ReSharper disable InconsistentNaming

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Datos de documento (RegDocumento – tDocumento).</summary>
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
internal struct tDocumento
{
    /// <summary>Folio del documento.</summary>
    internal double aFolio;

    /// <summary>Moneda del documento: 1 = pesos MN, 2 = moneda extranjera.</summary>
    internal int aNumMoneda;

    /// <summary>Tipo de cambio del documento.</summary>
    internal double aTipoCambio;

    /// <summary>Importe del documento; solo se usa en documentos de cargo o abono.</summary>
    internal double aImporte;

    /// <summary>No tiene uso; valor por omision 0 (cero).</summary>
    internal double aDescuentoDoc1;

    /// <summary>No tiene uso; valor por omision 0 (cero).</summary>
    internal double aDescuentoDoc2;

    /// <summary>Valor mayor que 5 para indicar una aplicación distinta de los PAQ.</summary>
    internal int aSistemaOrigen;

    /// <summary>Código del concepto del documento.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string aCodConcepto;

    /// <summary>Serie del documento.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongSerie + 1)]
    internal string aSerie;

    /// <summary>Fecha del documento, en formato mm/dd/aaaa.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongFecha + 1)]
    internal string aFecha;

    /// <summary>Código del cliente o proveedor.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string aCodigoCteProv;

    /// <summary>Código del agente.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string aCodigoAgente;

    /// <summary>Referencia del documento.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongReferencia + 1)]
    internal string aReferencia;

    /// <summary>No tiene uso; valor por omision 0 (cero)</summary>
    internal int aAfecta;

    /// <summary>Gasto 1; valor por omision 0 (cero).</summary>
    internal double aGasto1;

    /// <summary>Gasto 2; valor por omision 0 (cero).</summary>
    internal double aGasto2;

    /// <summary>Gasto 3; valor por omision 0 (cero).</summary>
    internal double aGasto3;
}