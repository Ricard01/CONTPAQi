using SDK.Comercial.Application.Common.Exceptions;

namespace SDK.Comercial.Infrastructure.Sdk.Cola;

/// <summary>
/// Estado del SDK visible para las operaciones encoladas. Solo existe dentro del hilo consumidor.
/// </summary>
internal sealed class SdkContexto(IComercialSdk sdk, string? empresaPredeterminada = null)
{
    public string? EmpresaAbierta { get; private set; }

    /// <summary>Abre la empresa configurada en <see cref="ComercialSdkOptions.Empresa"/>.</summary>
    public void UsarEmpresa()
    {
        if (string.IsNullOrWhiteSpace(empresaPredeterminada))
        {
            throw new ComercialSdkException(
                $"No se indicó empresa y no hay una predeterminada. Configura {ComercialSdkOptions.Seccion}:Empresa.");
        }

        UsarEmpresa(empresaPredeterminada);
    }

    /// <summary>Abre la empresa indicada, cerrando la anterior si era otra.</summary>
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
