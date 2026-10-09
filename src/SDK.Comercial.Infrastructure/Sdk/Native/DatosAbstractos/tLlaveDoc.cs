using System.Runtime.InteropServices;
using SDK.Comercial.Infrastructure.Sdk.Native.Constantes;

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Llave de documento (Llave del Documento – RegLlaveDoc – tLlaveDoc).</summary>
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
internal struct tLlaveDoc
{
    /// <summary>Código del concepto del documento.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string aConsepto;

    /// <summary>Serie del documento.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongSerie + 1)]
    internal string aSerie;

    /// <summary>Folio del documento.</summary>
    internal double aFolio;
}