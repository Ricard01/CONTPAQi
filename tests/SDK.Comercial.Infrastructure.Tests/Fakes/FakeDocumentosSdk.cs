using SDK.Comercial.Application.Common.Exceptions;
using SDK.Comercial.Infrastructure.Sdk;
using SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

namespace SDK.Comercial.Infrastructure.Tests.Fakes;

/// <summary>
/// Sustituye las funciones nativas de documentos. No escribe nada en Comercial: guarda las estructuras
/// que recibiría la DLL y anota cada llamada en la misma bitácora que <see cref="FakeSesionComercialSdk"/>,
/// para poder comprobar el orden completo (abrir empresa → folio → documento → movimientos).
/// </summary>
internal sealed class FakeDocumentosSdk(FakeSesionComercialSdk sesion) : IDocumentosSdk
{
    private int _siguienteIdMovimiento = 100;

    /// <summary>Serie y folio que devolverá <see cref="SiguienteFolio"/>.</summary>
    public (string Serie, double Folio) Folio { get; set; } = ("FA", 1);

    public int IdDocumentoAsignado { get; set; } = 10;

    /// <summary>Si se indica, el alta del movimiento con este consecutivo falla como lo haría el SDK.</summary>
    public int? FallarEnMovimiento { get; set; }

    public bool FallarAltaDocumento { get; set; }

    public bool FallarBorrado { get; set; }

    public List<tDocumento> Documentos { get; } = [];

    public List<tMovimiento> Movimientos { get; } = [];

    public HashSet<int> Hilos { get; } = [];

    public (string Serie, double Folio) SiguienteFolio(string codigoConcepto, string serie)
    {
        Registrar($"siguienteFolio {codigoConcepto} '{serie}'");
        return Folio;
    }

    public int AltaDocumento(tDocumento documento)
    {
        Registrar($"altaDocumento {documento.aSerie}-{documento.aFolio}");
        if (FallarAltaDocumento)
        {
            throw new ComercialSdkException(2, "El cliente no existe");
        }

        Documentos.Add(documento);
        return IdDocumentoAsignado;
    }

    public int AltaMovimiento(int idDocumento, tMovimiento movimiento)
    {
        Registrar($"altaMovimiento {idDocumento}#{movimiento.aConsecutivo}");
        if (FallarEnMovimiento == movimiento.aConsecutivo)
        {
            throw new ComercialSdkException(3, "El producto no existe");
        }

        Movimientos.Add(movimiento);
        return _siguienteIdMovimiento++;
    }

    public void BorrarDocumento(int idDocumento)
    {
        Registrar($"borrarDocumento {idDocumento}");
        if (FallarBorrado)
        {
            throw new ComercialSdkException(4, "No se pudo borrar");
        }
    }

    private void Registrar(string llamada)
    {
        Hilos.Add(Environment.CurrentManagedThreadId);
        sesion.Llamadas.Add(llamada);
    }
}
