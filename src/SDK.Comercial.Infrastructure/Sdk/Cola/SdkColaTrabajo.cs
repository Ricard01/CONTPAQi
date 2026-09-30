using System.Threading.Channels;
using Microsoft.Extensions.Options;
using SDK.Comercial.Application.Common.Exceptions;

namespace SDK.Comercial.Infrastructure.Sdk.Cola;

/// <summary>
/// Cola por la que pasan todas las operaciones al SDK. Un único hilo (<see cref="SdkWorker"/>) las consume en orden.
/// </summary>
internal sealed class SdkColaTrabajo(IOptions<ComercialSdkOptions> opciones)
{
    private readonly Channel<SdkTrabajo> _canal = Channel.CreateBounded<SdkTrabajo>(
        new BoundedChannelOptions(opciones.Value.CapacidadCola)
        {
            SingleReader = true,
            FullMode = BoundedChannelFullMode.Wait,
        });

    public ChannelReader<SdkTrabajo> Lector => _canal.Reader;

    public async Task<T> EncolarAsync<T>(Func<SdkContexto, T> operacion, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // El endpoint entrega la operación, pero no la ejecuta en su hilo HTTP. SdkWorker la
        // consumirá en el hilo dedicado y completará Resultado con el valor o la excepción.
        var trabajo = new SdkTrabajo<T>(operacion, cancellationToken);
        trabajo.RegistrarCancelacion();
        try
        {
            await _canal.Writer.WriteAsync(trabajo, cancellationToken).ConfigureAwait(false);
        }
        catch (ChannelClosedException ex)
        {
            throw new ComercialSdkException("El servicio del SDK se está deteniendo.", ex);
        }

        // La espera es asíncrona: no ocupa un hilo web mientras el trabajo aguarda su turno.
        return await trabajo.Resultado.ConfigureAwait(false);
    }

    public void Completar() => _canal.Writer.TryComplete();
}
