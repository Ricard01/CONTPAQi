using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SDK.Comercial.Application.Common.Exceptions;
using SDK.Comercial.Infrastructure.Conexion;
using SDK.Comercial.Infrastructure.Sdk;
using SDK.Comercial.Infrastructure.Sdk.Cola;
using SDK.Comercial.Infrastructure.Tests.Fakes;

namespace SDK.Comercial.Infrastructure.Tests;

public sealed class SdkColaTrabajoTests : IAsyncLifetime
{
    private const string Empresa = @"C:\Empresas\A";

    private readonly FakeSesionComercialSdk _sdk = new();
    private readonly SdkColaTrabajo _cola = new();
    private readonly SdkWorker _worker;

    public SdkColaTrabajoTests()
    {
        _worker = new SdkWorker(_cola, _sdk, Opciones(), NullLogger<SdkWorker>.Instance);
    }

    private static IOptions<ComercialSdkOptions> Opciones() =>
        Options.Create(new ComercialSdkOptions { RutaEmpresa = Empresa });

    public Task InitializeAsync() => _worker.StartAsync(CancellationToken.None);

    public async Task DisposeAsync()
    {
        await _worker.StopAsync(CancellationToken.None);
        _worker.Dispose();
    }

    [Fact]
    public async Task Ejecuta_todas_las_operaciones_en_un_solo_hilo_sin_concurrencia()
    {
        var hilos = new System.Collections.Concurrent.ConcurrentBag<int>();
        var enEjecucion = 0;
        var maximoSimultaneo = 0;

        var tareas = Enumerable.Range(0, 50).Select(i => Task.Run(() => _cola.EncolarAsync(_ =>
        {
            var actuales = Interlocked.Increment(ref enEjecucion);
            maximoSimultaneo = Math.Max(maximoSimultaneo, actuales);
            hilos.Add(Environment.CurrentManagedThreadId);
            Thread.Sleep(1);
            Interlocked.Decrement(ref enEjecucion);
            return i;
        })));

        var resultados = await Task.WhenAll(tareas);

        Assert.Equal(Enumerable.Range(0, 50), resultados.Order());
        Assert.Single(hilos.Distinct());
        Assert.Equal(1, maximoSimultaneo);
        Assert.Equal(_sdk.HiloInicio, hilos.First());
    }

    [Fact]
    public async Task Una_excepcion_llega_al_llamador_y_el_hilo_sigue_procesando()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _cola.EncolarAsync<int>(_ => throw new InvalidOperationException()));

        Assert.Equal(42, await _cola.EncolarAsync(_ => 42));
    }

    [Fact]
    public async Task Cancelar_mientras_espera_en_la_cola_descarta_la_operacion()
    {
        using var liberar = new ManualResetEventSlim();
        var bloqueo = _cola.EncolarAsync(_ => liberar.Wait(TimeSpan.FromSeconds(10)));

        using var cts = new CancellationTokenSource();
        var ejecutada = false;
        var enEspera = _cola.EncolarAsync(_ => ejecutada = true, cts.Token);

        await cts.CancelAsync();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => enEspera);

        liberar.Set();
        await bloqueo;
        await _cola.EncolarAsync(_ => 0);
        Assert.False(ejecutada);
    }

    [Fact]
    public async Task UsarEmpresa_no_reabre_la_empresa_ya_abierta_al_iniciar()
    {
        await _cola.EncolarAsync(c => { c.UsarEmpresa(); return 0; });
        await _cola.EncolarAsync(c => { c.UsarEmpresa(); return 0; });

        Assert.Equal([$"abrir {Empresa}"], _sdk.Llamadas);
    }

    [Fact]
    public async Task Si_el_SDK_no_inicia_las_operaciones_fallan_con_ComercialSdkException()
    {
        var sdk = new FakeSesionComercialSdk { ErrorAlIniciar = new InvalidOperationException("sin licencia") };
        var cola = new SdkColaTrabajo();
        using var worker = new SdkWorker(cola, sdk, Opciones(), NullLogger<SdkWorker>.Instance);
        await worker.StartAsync(CancellationToken.None);

        var error = await Assert.ThrowsAsync<ComercialSdkException>(() => cola.EncolarAsync(_ => 0));
        Assert.IsType<InvalidOperationException>(error.InnerException);

        await worker.StopAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Abre_la_empresa_configurada_al_iniciar_y_la_cierra_al_terminar()
    {
        var sdk = new FakeSesionComercialSdk();
        var cola = new SdkColaTrabajo();
        using var worker = new SdkWorker(cola, sdk, Opciones(), NullLogger<SdkWorker>.Instance);
        await worker.StartAsync(CancellationToken.None);

        var estado = await new ConexionComercial(cola).VerificarAsync();
        await worker.StopAsync(CancellationToken.None);

        Assert.Equal(Empresa, estado.Empresa);
        Assert.Equal([$"abrir {Empresa}", "cerrar", "terminar"], sdk.Llamadas);
    }

    [Fact]
    public async Task Si_la_empresa_no_abre_al_iniciar_se_reintenta_en_la_siguiente_operacion()
    {
        var sdk = new FakeSesionComercialSdk { FallosAlAbrir = 1 };
        var cola = new SdkColaTrabajo();
        using var worker = new SdkWorker(cola, sdk, Opciones(), NullLogger<SdkWorker>.Instance);
        await worker.StartAsync(CancellationToken.None);

        var estado = await new ConexionComercial(cola).VerificarAsync();
        await worker.StopAsync(CancellationToken.None);

        Assert.Equal(Empresa, estado.Empresa);
        Assert.Equal([$"abrir {Empresa}", $"abrir {Empresa}", "cerrar", "terminar"], sdk.Llamadas);
    }
}
