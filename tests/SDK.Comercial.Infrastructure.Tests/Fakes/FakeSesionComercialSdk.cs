using SDK.Comercial.Application.Common.Exceptions;
using SDK.Comercial.Infrastructure.Sdk;

namespace SDK.Comercial.Infrastructure.Tests.Fakes;

/// <summary>
/// Sustituye a la sesión real: no carga MGWServicios.dll, solo registra en <see cref="Llamadas"/>
/// qué se le pidió y en qué hilo, para que las pruebas verifiquen el comportamiento de la cola.
/// </summary>
internal sealed class FakeSesionComercialSdk : ISesionComercialSdk
{
    public Exception? ErrorAlIniciar { get; init; }

    public int FallosAlAbrir { get; set; }

    public int HiloInicio { get; private set; }

    public List<string> Llamadas { get; } = [];

    public void Iniciar()
    {
        HiloInicio = Environment.CurrentManagedThreadId;
        if (ErrorAlIniciar is not null)
        {
            throw ErrorAlIniciar;
        }
    }

    public void Terminar() => Llamadas.Add("terminar");

    public void AbrirEmpresa(string ruta)
    {
        Llamadas.Add($"abrir {ruta}");
        if (FallosAlAbrir > 0)
        {
            FallosAlAbrir--;
            throw new ComercialSdkException(1, "Empresa no encontrada");
        }
    }

    public void CerrarEmpresa() => Llamadas.Add("cerrar");
}
