using System.Text;
using SDK.Comercial.Infrastructure.Sdk.Native;
using SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

namespace SDK.Comercial.Infrastructure.Sdk;

/// <summary>Implementación de <see cref="IDocumentosSdk"/> sobre MGWServicios.dll.</summary>
internal sealed class DocumentosSdk : IDocumentosSdk
{
    /// <inheritdoc />
    public (string Serie, double Folio) SiguienteFolio(string codigoConcepto, string serie)
    {
        // La serie es de entrada y salida: el SDK escribe en el buffer la serie que usará.
        var bufferSerie = new StringBuilder(serie, MgwServicios.TamanoBuffer);
        var folio = 0d;
        SdkResultado.Verificar(MgwServicios.fSiguienteFolio(codigoConcepto, bufferSerie, ref folio));
        return (bufferSerie.ToString().Trim(), folio);
    }

    /// <inheritdoc />
    public int AltaDocumento(tDocumento documento)
    {
        var id = 0;
        SdkResultado.Verificar(MgwServicios.fAltaDocumento(ref id, ref documento));
        return id;
    }

    /// <inheritdoc />
    public int AltaMovimiento(int idDocumento, tMovimiento movimiento)
    {
        var id = 0;
        SdkResultado.Verificar(MgwServicios.fAltaMovimiento(idDocumento, ref id, movimiento));
        return id;
    }

    /// <inheritdoc />
    public void BorrarDocumento(int idDocumento)
    {
        SdkResultado.Verificar(MgwServicios.fBuscarIdDocumento(idDocumento));
        SdkResultado.Verificar(MgwServicios.fBorraDocumento());
    }
}
