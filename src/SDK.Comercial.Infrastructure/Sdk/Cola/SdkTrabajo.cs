namespace SDK.Comercial.Infrastructure.Sdk.Cola;

internal abstract class SdkTrabajo
{
    public abstract void Ejecutar(SdkContexto contexto);

    public abstract void Fallar(Exception error);
}

internal sealed class SdkTrabajo<T>(Func<SdkContexto, T> operacion, CancellationToken cancellationToken) : SdkTrabajo
{
    private readonly TaskCompletionSource<T> _resultado = new(TaskCreationOptions.RunContinuationsAsynchronously);
    private CancellationTokenRegistration _registro;

    public Task<T> Resultado => _resultado.Task;

    /// <summary>Permite cancelar el trabajo mientras espera en la cola.</summary>
    public void RegistrarCancelacion() =>
        _registro = cancellationToken.Register(() => _resultado.TrySetCanceled(cancellationToken));

    public override void Ejecutar(SdkContexto contexto)
    {
        // Una vez que empieza en el SDK ya no se cancela: se deja terminar.
        _registro.Dispose();
        if (_resultado.Task.IsCompleted)
        {
            return;
        }

        try
        {
            _resultado.TrySetResult(operacion(contexto));
        }
        catch (Exception ex)
        {
            _resultado.TrySetException(ex);
        }
    }

    public override void Fallar(Exception error)
    {
        _registro.Dispose();
        _resultado.TrySetException(error);
    }
}
