namespace SDK.Comercial.Infrastructure.Sdk;

/// <summary>
/// Ciclo de vida del SDK. Separado de la cola para poder probarla sin la DLL nativa.
/// </summary>
internal interface IComercialSdk
{
    void Iniciar();

    void Terminar();

    void AbrirEmpresa(string ruta);

    void CerrarEmpresa();
}
