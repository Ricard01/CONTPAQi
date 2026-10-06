using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SDK.Comercial.Application.Common.Exceptions;
using SDK.Comercial.Application.Facturas;
using SDK.Comercial.Infrastructure.Facturas;
using SDK.Comercial.Infrastructure.Sdk;
using SDK.Comercial.Infrastructure.Sdk.Cola;
using SDK.Comercial.Infrastructure.Sdk.Native;

namespace SDK.Comercial.Infrastructure.IntegrationTests;

/// <summary>
/// Inicia el SDK real (MGWServicios.dll), la cola y el hilo consumidor reales una sola vez para todas
/// las pruebas de la clase: el SDK mantiene estado global y no conviene iniciarlo y terminarlo por prueba.
/// </summary>
public sealed class SdkRealFixture : IAsyncLifetime
{
    private SdkWorker? _worker;

    internal SdkColaTrabajo Cola { get; } = new();

    internal DocumentosSdk Documentos { get; } = new();

    internal FacturaRepository Repositorio { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var configuracion = ConfiguracionIntegracion.Actual;
        if (configuracion.MotivoOmision is not null)
        {
            return;
        }

        var opciones = Options.Create(configuracion.Sdk);
        var sesion = new SesionComercialSdk(opciones, NullLogger<SesionComercialSdk>.Instance);
        _worker = new SdkWorker(Cola, sesion, opciones, NullLogger<SdkWorker>.Instance);
        Repositorio = new FacturaRepository(Cola, Documentos, NullLogger<FacturaRepository>.Instance);
        await _worker.StartAsync(CancellationToken.None);
    }

    public async Task DisposeAsync()
    {
        if (_worker is not null)
        {
            await _worker.StopAsync(CancellationToken.None);
            _worker.Dispose();
        }
    }
}

/// <summary>
/// Pruebas de extremo a extremo contra Comercial Premium: insertan documentos de verdad en la empresa
/// configurada, los leen de vuelta con el SDK y los borran al terminar. Solo corren en Windows x86 con
/// <c>COMERCIAL_PRUEBAS_INTEGRACION=1</c> (ver <see cref="ConfiguracionIntegracion"/>).
/// </summary>
public sealed class FacturaRepositoryIntegracionTests(SdkRealFixture sdk) : IClassFixture<SdkRealFixture>
{
    private static readonly ConfiguracionIntegracion Configuracion = ConfiguracionIntegracion.Actual;

    private static NuevaFactura Factura(string producto) => new(
        Configuracion.Concepto!,
        Serie: null,
        Fecha: DateOnly.FromDateTime(DateTime.Today),
        Configuracion.Cliente!,
        Referencia: "Prueba integración",
        [new(producto, Configuracion.Almacen!, 2, 10.5)]);

    [FactIntegracion]
    public async Task Crea_la_factura_en_Comercial_y_se_puede_leer_de_vuelta()
    {
        var creada = await sdk.Repositorio.CrearAsync(Factura(Configuracion.Producto!));

        try
        {
            Assert.True(creada.IdDocumento > 0);
            Assert.True(creada.Folio > 0);
            Assert.Single(creada.IdMovimientos);

            // Lectura independiente del repositorio: se localiza el documento por su id en la empresa.
            var (folio, serie, total) = await sdk.Cola.EncolarAsync(_ =>
            {
                SdkResultado.Verificar(MgwServicios.fBuscarIdDocumento(creada.IdDocumento));
                return (LeerDato("CFOLIO"), LeerDato("CSERIEDOCUMENTO"), LeerDato("CTOTAL"));
            });

            Assert.Equal(creada.Folio, double.Parse(folio, CultureInfo.InvariantCulture));
            Assert.Equal(creada.Serie, serie);
            Assert.True(double.Parse(total, CultureInfo.InvariantCulture) > 0, $"Total leído: {total}");
        }
        finally
        {
            // Limpieza: la empresa de pruebas no acumula documentos de cada ejecución.
            await sdk.Cola.EncolarAsync(_ =>
            {
                sdk.Documentos.BorrarDocumento(creada.IdDocumento);
                return 0;
            });
        }
    }

    [FactIntegracion]
    public async Task Un_producto_inexistente_devuelve_el_error_traducido_del_SDK()
    {
        var error = await Assert.ThrowsAsync<ComercialSdkException>(() =>
            sdk.Repositorio.CrearAsync(Factura($"NOEXISTE{Guid.NewGuid():N}"[..20])));

        Assert.NotNull(error.Codigo);
        Assert.False(string.IsNullOrWhiteSpace(error.Message));
    }

    private static string LeerDato(string campo)
    {
        var valor = new StringBuilder(MgwServicios.TamanoBuffer);
        SdkResultado.Verificar(MgwServicios.fLeeDatoDocumento(campo, valor, valor.Capacity));
        return valor.ToString().Trim();
    }
}
