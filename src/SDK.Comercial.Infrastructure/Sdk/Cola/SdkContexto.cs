using SDK.Comercial.Application.Common.Exceptions;

namespace SDK.Comercial.Infrastructure.Sdk.Cola;

/// <summary>
/// Estado compartido por las operaciones encoladas. Solo existe dentro del hilo consumidor y
/// garantiza que como máximo haya una empresa abierta en el SDK global.
/// </summary>
internal sealed class SdkContexto(IComercialSdk sdk, string? empresaPredeterminada = null)
{
    public string? EmpresaAbierta { get; private set; }

    /// <summary>
    /// Abre la ruta configurada en <see cref="ComercialSdkOptions.RutaEmpresa"/>. El valor debe ser
    /// una ruta de empresa aceptada por <c>fAbreEmpresa</c>; este método no resuelve nombres cortos.
    /// </summary>
    public void UsarEmpresa()
    {
        if (string.IsNullOrWhiteSpace(empresaPredeterminada))
        {
            throw new ComercialSdkException(
                $"No se indicó empresa y no hay una predeterminada. Configura {ComercialSdkOptions.Seccion}:Empresa.");
        }

        UsarEmpresa(empresaPredeterminada);
    }

    /// <summary>
    /// Abre la empresa indicada. Si ya está abierta evita una llamada nativa innecesaria; si es
    /// distinta, primero cierra la anterior porque el SDK solo conserva una empresa activa.
    /// </summary>
    public void UsarEmpresa(string ruta)
    {
        if (string.Equals(EmpresaAbierta, ruta, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        CerrarEmpresa();
        sdk.AbrirEmpresa(ruta);
        EmpresaAbierta = ruta;
    }

    public void CerrarEmpresa()
    {
        if (EmpresaAbierta is null)
        {
            return;
        }

        EmpresaAbierta = null;
        sdk.CerrarEmpresa();
    }
}
