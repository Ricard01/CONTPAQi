using System.Runtime.InteropServices;
using SDK.Comercial.Infrastructure.Sdk.Native.Constantes;

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Valor de clasificación (RegValorClasificacion / TValorClasificacion).</summary>
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
internal struct TValorClasificacion
{
    /// <summary>Clasificación.</summary> internal int cClasificacionDe;
    /// <summary>Número de clasificación.</summary> internal int cNumClasificacion;
    /// <summary>Código del valor de clasificación.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodValorClasif + 1)]
    internal string cCodigoValorClasificacion;

    /// <summary>Descripción del valor de clasificación.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string cValorClasificacion;
}