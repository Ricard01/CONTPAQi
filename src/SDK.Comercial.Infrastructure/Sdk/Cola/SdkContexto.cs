namespace SDK.Comercial.Infrastructure.Sdk.Cola;

/// <summary>
/// Estado compartido por las operaciones encoladas. Solo existe dentro del hilo consumidor.
/// La API es mono empresa: trabaja siempre con <see cref="ComercialSdkOptions.RutaEmpresa"/>.
/// </summary>
internal sealed class SdkContexto(ISesionComercialSdk sdk, string rutaEmpresa)
{
    public string RutaEmpresa { get; } = rutaEmpresa;

    public bool EmpresaAbierta { get; private set; }

    /// <summary>
    /// Garantiza que la empresa configurada esté abierta. Normalmente ya lo está desde el arranque;
    /// si en ese momento falló, aquí se reintenta y el error llega a la operación que la necesita.
    /// </summary>
    public void UsarEmpresa()
    {
        if (EmpresaAbierta)
        {
            return;
        }

        sdk.AbrirEmpresa(RutaEmpresa);
        EmpresaAbierta = true;
    }

    public void CerrarEmpresa()
    {
        if (!EmpresaAbierta)
        {
            return;
        }

        EmpresaAbierta = false;
        sdk.CerrarEmpresa();
    }
}
