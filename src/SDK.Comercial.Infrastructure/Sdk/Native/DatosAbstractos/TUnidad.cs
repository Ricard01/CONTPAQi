using System.Runtime.InteropServices;
using SDK.Comercial.Infrastructure.Sdk.Native.Constantes;

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Unidad de medida (RegUnidad / TUnidad).</summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
internal struct TUnidad
{
    /// <summary>Nombre de la unidad.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongNombre + 1)] internal string cNombreUnidad;
    /// <summary>Abreviatura de la unidad.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongAbreviatura + 1)] internal string cAbreviatura;
    /// <summary>Texto de despliegue de la unidad.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongAbreviatura + 1)] internal string cDespliegue;
}
