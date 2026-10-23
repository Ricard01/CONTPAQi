using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SDK.Comercial.Application.Common.Exceptions;
using SDK.Comercial.Application.Facturas;
using SDK.Comercial.Infrastructure.Facturas;
using SDK.Comercial.Infrastructure.Sdk;
using SDK.Comercial.Infrastructure.Sdk.Cola;
using SDK.Comercial.Infrastructure.Tests.Fakes;

namespace SDK.Comercial.Infrastructure.Tests;

/// <summary>
/// Pruebas del alta de facturas con un SDK falso: la cola y el hilo consumidor son los reales, pero
/// ninguna llamada llega a MGWServicios.dll ni se escribe nada en una empresa de Comercial.
/// </summary>
public sealed class FacturaRepositoryTests : IAsyncLifetime
{
    private const string Empresa = @"C:\Empresas\A";

    private readonly FakeSesionComercialSdk _sesion = new();
    private readonly FakeDocumentosSdk _documentos;
    private readonly SdkColaTrabajo _cola = new();
    private readonly SdkWorker _worker;
    private readonly FacturaRepository _repositorio;

    public FacturaRepositoryTests()
    {
        _documentos = new FakeDocumentosSdk(_sesion);
        var opciones = Options.Create(new ComercialSdkOptions { RutaEmpresa = Empresa });
        _worker = new SdkWorker(_cola, _sesion, opciones, NullLogger<SdkWorker>.Instance);
        _repositorio = new FacturaRepository(_cola, _documentos, NullLogger<FacturaRepository>.Instance);
    }

    public Task InitializeAsync() => _worker.StartAsync(CancellationToken.None);

    public async Task DisposeAsync()
    {
        await _worker.StopAsync(CancellationToken.None);
        _worker.Dispose();
    }

    private static NuevaFactura Factura(params NuevoMovimiento[] movimientos) => new(
        CodigoConcepto: "4",
        Fecha: new DateOnly(2026, 3, 7),
        CodigoCliente: "CTE001",
        Referencia: "Pedido 123",
        Movimientos: movimientos.Length > 0
            ? movimientos
            : [new("PROD01", "1", 2, 150.5), new("SERV01", "1", 1, 99, "Instalación")]);

    [Fact]
    public async Task Crea_folio_encabezado_y_movimientos_en_orden_con_la_empresa_abierta()
    {
        _documentos.Folio = ("FA", 57);

        var creada = await _repositorio.CrearAsync(Factura());

        Assert.Equal(
            [$"abrir {Empresa}", "siguienteFolio 4", "altaDocumento FA-57", "altaMovimiento 10#1", "altaMovimiento 10#2", "actualizarMovimiento 101"],
            _sesion.Llamadas);
        Assert.Equal(10, creada.IdDocumento);
        Assert.Equal("4", creada.CodigoConcepto);
        Assert.Equal("FA", creada.Serie);
        Assert.Equal(57, creada.Folio);
        Assert.Equal([100, 101], creada.IdMovimientos);
    }

    [Fact]
    public async Task Envia_al_SDK_las_estructuras_con_el_formato_que_espera()
    {
        _documentos.Folio = ("FA", 57);

        await _repositorio.CrearAsync(Factura());

        var documento = Assert.Single(_documentos.Documentos);
        Assert.Equal("4", documento.aCodConcepto);
        Assert.Equal("FA", documento.aSerie);
        Assert.Equal(57, documento.aFolio);
        Assert.Equal("03/07/2026", documento.aFecha); // mm/dd/aaaa, independiente de la cultura
        Assert.Equal("CTE001", documento.aCodigoCteProv);
        Assert.Equal(string.Empty, documento.aCodigoAgente);
        Assert.Equal("Pedido 123", documento.aReferencia);
        Assert.Equal(1, documento.aNumMoneda);
        Assert.Equal(1, documento.aTipoCambio);
        Assert.True(documento.aSistemaOrigen > 5);

        Assert.Collection(_documentos.Movimientos,
            m =>
            {
                Assert.Equal(1, m.aConsecutivo);
                Assert.Equal("PROD01", m.aCodProdSer);
                Assert.Equal("1", m.aCodAlmacen);
                Assert.Equal(2, m.aUnidades);
                Assert.Equal(150.5, m.aPrecio);
                Assert.Equal(0, m.aPorcDescto1);
                Assert.Equal(0, m.aImporteDescto1);
            },
            m =>
            {
                Assert.Equal(2, m.aConsecutivo);
                Assert.Equal("SERV01", m.aCodProdSer);
            });

        // Solo el movimiento con observaciones se edita; el documento no, porque no trae campos extra.
        Assert.Equal([101], _documentos.CamposMovimiento.Keys);
        Assert.Equal([("COBSERVAMOV", "Instalación")], _documentos.CamposMovimiento[101]);
        Assert.Empty(_documentos.CamposDocumento);
    }

    [Fact]
    public async Task Envia_los_descuentos_del_movimiento_sin_editarlo_despues()
    {
        await _repositorio.CrearAsync(Factura(
            new("SERV01", "1", 2, 1000, PorcentajeDescuento: 10),
            new("SERV02", "1", 1, 333.33, ImporteDescuento: 33.33)));

        Assert.Collection(_documentos.Movimientos,
            m =>
            {
                Assert.Equal(10, m.aPorcDescto1);
                Assert.Equal(0, m.aImporteDescto1);
            },
            m =>
            {
                Assert.Equal(0, m.aPorcDescto1);
                Assert.Equal(33.33, m.aImporteDescto1);
            });

        // Sin observaciones no hay nada que escribir después del alta; impuestos y retenciones los calcula el concepto.
        Assert.Empty(_documentos.CamposMovimiento);
    }

    [Fact]
    public async Task Escribe_observaciones_y_textos_extra_del_documento_despues_de_los_movimientos()
    {
        await _repositorio.CrearAsync(Factura() with
        {
            Observaciones = "Entregar en almacén",
            TextoExtra1 = "OC-991",
            TextoExtra3 = "Ruta 4",
        });

        Assert.Equal("actualizarDocumento 10", _sesion.Llamadas[^1]);
        Assert.Equal(
            [("COBSERVACIONES", "Entregar en almacén"), ("CTEXTOEXTRA1", "OC-991"), ("CTEXTOEXTRA3", "Ruta 4")],
            _documentos.CamposDocumento[10]);
    }

    [Fact]
    public async Task Si_falla_la_edicion_del_documento_lo_borra()
    {
        _documentos.FallarActualizarDocumento = true;

        var error = await Assert.ThrowsAsync<ComercialSdkException>(() =>
            _repositorio.CrearAsync(Factura() with { Observaciones = "x" }));

        Assert.Equal(5, error.Codigo);
        Assert.Equal("borrarDocumento 10", _sesion.Llamadas[^1]);
    }

    [Fact]
    public async Task Descuentos_invalidos_se_rechazan()
    {
        var factura = Factura(
            new("P1", "1", 1, 100, PorcentajeDescuento: 5, ImporteDescuento: 5),
            new("P2", "1", 1, 100, PorcentajeDescuento: 101),
            new("P3", "1", 1, 100, ImporteDescuento: 150));

        var error = await Assert.ThrowsAsync<ValidationException>(() => _repositorio.CrearAsync(factura));

        Assert.Equal(
            ["Movimientos[0]", "Movimientos[1]", "Movimientos[2]"],
            error.Errors.Keys.Order(StringComparer.Ordinal));
    }

    [Fact]
    public async Task Envia_el_agente_indicado_en_el_encabezado()
    {
        await _repositorio.CrearAsync(Factura() with { CodigoAgente = "AG01" });

        Assert.Equal("AG01", Assert.Single(_documentos.Documentos).aCodigoAgente);
    }

    [Fact]
    public async Task Todas_las_llamadas_al_SDK_ocurren_en_el_hilo_de_la_cola()
    {
        await _repositorio.CrearAsync(Factura());

        Assert.Equal([_sesion.HiloInicio], _documentos.Hilos);
    }

    [Fact]
    public async Task Si_un_movimiento_falla_borra_el_documento_y_devuelve_el_error_del_SDK()
    {
        _documentos.FallarEnMovimiento = 2;

        var error = await Assert.ThrowsAsync<ComercialSdkException>(() => _repositorio.CrearAsync(Factura()));

        Assert.Equal(3, error.Codigo);
        Assert.Equal(
            [$"abrir {Empresa}", "siguienteFolio 4", "altaDocumento FA-1", "altaMovimiento 10#1", "altaMovimiento 10#2", "borrarDocumento 10"],
            _sesion.Llamadas);
    }

    [Fact]
    public async Task Si_el_borrado_tambien_falla_se_conserva_el_error_original()
    {
        _documentos.FallarEnMovimiento = 1;
        _documentos.FallarBorrado = true;

        var error = await Assert.ThrowsAsync<ComercialSdkException>(() => _repositorio.CrearAsync(Factura()));

        Assert.Equal(3, error.Codigo);
        Assert.Contains("borrarDocumento 10", _sesion.Llamadas);
    }

    [Fact]
    public async Task Si_falla_el_encabezado_no_intenta_movimientos_ni_borrado()
    {
        _documentos.FallarAltaDocumento = true;

        var error = await Assert.ThrowsAsync<ComercialSdkException>(() => _repositorio.CrearAsync(Factura()));

        Assert.Equal(2, error.Codigo);
        Assert.Equal([$"abrir {Empresa}", "siguienteFolio 4", "altaDocumento FA-1"], _sesion.Llamadas);
    }

    [Fact]
    public async Task La_cola_sigue_funcionando_despues_de_una_factura_fallida()
    {
        _documentos.FallarEnMovimiento = 1;
        await Assert.ThrowsAsync<ComercialSdkException>(() => _repositorio.CrearAsync(Factura()));

        _documentos.FallarEnMovimiento = null;
        var creada = await _repositorio.CrearAsync(Factura());

        Assert.Equal(10, creada.IdDocumento);
    }

    [Fact]
    public async Task Datos_invalidos_se_rechazan_sin_llegar_al_SDK()
    {
        var factura = new NuevaFactura(" ", new DateOnly(2026, 3, 7), "", null,
            [new("PROD01", "1", 0, 10), new("", "", 1, -1)]);

        var error = await Assert.ThrowsAsync<ValidationException>(() => _repositorio.CrearAsync(factura));

        Assert.Equal(
            ["CodigoCliente", "CodigoConcepto", "Movimientos[0]", "Movimientos[1]"],
            error.Errors.Keys.Order(StringComparer.Ordinal));
        Assert.Equal(3, error.Errors["Movimientos[1]"].Length);
        // Solo la apertura de la empresa predeterminada al arrancar el worker.
        Assert.Equal([$"abrir {Empresa}"], _sesion.Llamadas);
    }

    [Fact]
    public async Task Factura_sin_movimientos_se_rechaza()
    {
        var factura = Factura() with { Movimientos = [] };

        var error = await Assert.ThrowsAsync<ValidationException>(() => _repositorio.CrearAsync(factura));

        Assert.Contains("Movimientos", error.Errors.Keys);
    }
}
