using System.Runtime.InteropServices;
using SDK.Comercial.Infrastructure.Sdk.Native.Constantes;

namespace SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

/// <summary>Dirección de cliente/proveedor (RegDireccion / tDireccion).</summary>
[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]
internal struct tDireccion
{
    /// <summary>Código de cliente o proveedor.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigo + 1)]
    internal string cCodCteProv;

    /// <summary>Tipo de catálogo.</summary> internal int cTipoCatalogo;
    /// <summary>Tipo de dirección.</summary> internal int cTipoDireccion;
    /// <summary>Nombre de calle.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string cNombreCalle;

    /// <summary>Número exterior.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongNumeroExtInt + 1)]
    internal string cNumeroExterior;

    /// <summary>Número interior.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongNumeroExtInt + 1)]
    internal string cNumeroInterior;

    /// <summary>Colonia.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string cColonia;

    /// <summary>Código postal.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongCodigoPostal + 1)]
    internal string cCodigoPostal;

    /// <summary>Teléfono 1.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongTelefono + 1)]
    internal string cTelefono1;

    /// <summary>Teléfono 2.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongTelefono + 1)]
    internal string cTelefono2;

    /// <summary>Teléfono 3.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongTelefono + 1)]
    internal string cTelefono3;

    /// <summary>Teléfono 4.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongTelefono + 1)]
    internal string cTelefono4;

    /// <summary>Correo electrónico.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongEmailWeb + 1)]
    internal string cEmail;

    /// <summary>Dirección web.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongEmailWeb + 1)]
    internal string cDireccionWeb;

    /// <summary>Ciudad.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string cCiudad;

    /// <summary>Estado.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string cEstado;

    /// <summary>País.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string cPais;

    /// <summary>Texto extra.</summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MgwConstantes.kLongDescripcion + 1)]
    internal string cTextoExtra;
}