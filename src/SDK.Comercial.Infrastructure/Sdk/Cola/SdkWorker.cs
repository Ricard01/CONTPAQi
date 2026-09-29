using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SDK.Comercial.Application.Common.Exceptions;

namespace SDK.Comercial.Infrastructure.Sdk.Cola;

/// <summary>
/// Hilo consumidor único: inicia el SDK, abre la empresa predeterminada, ejecuta los trabajos de la cola uno por uno
/// y termina el SDK al detenerse.
/// </summary>
internal sealed class SdkWorker(
    SdkColaTrabajo cola,
    IComercialSdk sdk,
    IOptions<ComercialSdkOptions> opciones,
    ILogger<SdkWorker> logger) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var terminado = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var hilo = new Thread(() =>
        {
            try
            {
                Procesar(stoppingToken);
                terminado.SetResult();
            }
            catch (Exception ex)
            {
                terminado.SetException(ex);
            }
        })
        {
            IsBackground = true,
            Name = "CONTPAQi SDK",
        };
        hilo.Start();
        return terminado.Task;
    }

    private void Procesar(CancellationToken stoppingToken)
    {
        Exception? errorInicio = null;
        try
        {
            sdk.Iniciar();
            logger.LogInformation("SDK de CONTPAQi iniciado");
        }
        catch (Exception ex)
        {
            errorInicio = ex;
            logger.LogError(ex, "No se pudo iniciar el SDK de CONTPAQi");
        }

        var contexto = new SdkContexto(sdk, opciones.Value.Empresa);
        if (errorInicio is null)
        {
            AbrirEmpresaPredeterminada(contexto);
        }

        try
        {
            while (cola.Lector.WaitToReadAsync(stoppingToken).AsTask().GetAwaiter().GetResult())
            {
                while (cola.Lector.TryRead(out var trabajo))
                {
                    if (errorInicio is not null)
                    {
                        trabajo.Fallar(new ComercialSdkException("El SDK de CONTPAQi no se pudo iniciar.", errorInicio));
                        continue;
                    }

                    trabajo.Ejecutar(contexto);
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
        }
        finally
        {
            cola.Completar();
            while (cola.Lector.TryRead(out var pendiente))
            {
                pendiente.Fallar(new ComercialSdkException("El servicio del SDK se detuvo antes de ejecutar la operación."));
            }

            if (errorInicio is null)
            {
                Terminar(contexto);
            }
        }
    }

    private void AbrirEmpresaPredeterminada(SdkContexto contexto)
    {
        var empresa = opciones.Value.Empresa;
        if (string.IsNullOrWhiteSpace(empresa))
        {
            return;
        }

        try
        {
            contexto.UsarEmpresa();
            logger.LogInformation("Empresa {Empresa} abierta", empresa);
        }
        catch (Exception ex)
        {
            // No es fatal: la siguiente operación que la use vuelve a intentar abrirla y recibe el error.
            logger.LogError(ex, "No se pudo abrir la empresa {Empresa}", empresa);
        }
    }

    private void Terminar(SdkContexto contexto)
    {
        try
        {
            contexto.CerrarEmpresa();
            sdk.Terminar();
            logger.LogInformation("SDK de CONTPAQi terminado");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error al terminar el SDK de CONTPAQi");
        }
    }
}
