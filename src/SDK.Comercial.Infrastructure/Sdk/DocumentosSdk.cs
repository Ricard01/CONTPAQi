using System.Text;
using SDK.Comercial.Infrastructure.Sdk.Native;
using SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

namespace SDK.Comercial.Infrastructure.Sdk;

/// <summary>Implementación de <see cref="IDocumentosSdk"/> sobre MGWServicios.dll.</summary>
internal sealed class DocumentosSdk : IDocumentosSdk
{
    /// <inheritdoc />
    public (string Serie, double Folio) SiguienteFolio(string codigoConcepto)
    {
        // La serie es solo de salida: el SDK escribe la del concepto aunque se le pase otra.
        var bufferSerie = new StringBuilder(MgwServicios.TamanoBuffer);
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
    public int AltaMovimiento(int idDocumento, tMovimientoDesc movimiento)
    {
        var id = 0;
        SdkResultado.Verificar(MgwServicios.fAltaMovimientoCDesct(idDocumento, ref id, ref movimiento));
        return id;
    }

    /// <inheritdoc />
    public void ActualizarDocumento(int idDocumento, IReadOnlyList<(string Campo, string Valor)> campos)
    {
        SdkResultado.Verificar(MgwServicios.fBuscarIdDocumento(idDocumento));
        SdkResultado.Verificar(MgwServicios.fEditarDocumento());
        try
        {
            foreach (var (campo, valor) in campos)
            {
                SdkResultado.Verificar(MgwServicios.fSetDatoDocumento(campo, valor));
            }

            SdkResultado.Verificar(MgwServicios.fGuardaDocumento());
        }
        catch
        {
            // Sin esto el documento queda en modo edición y la siguiente operación del SDK lo arrastra.
            MgwServicios.fCancelarModificacionDocumento();
            throw;
        }
    }

    /// <inheritdoc />
    public void ActualizarMovimiento(int idMovimiento, IReadOnlyList<(string Campo, string Valor)> campos)
    {
        SdkResultado.Verificar(MgwServicios.fBuscarIdMovimiento(idMovimiento));
        SdkResultado.Verificar(MgwServicios.fEditarMovimiento());
        try
        {
            foreach (var (campo, valor) in campos)
            {
                SdkResultado.Verificar(MgwServicios.fSetDatoMovimiento(campo, valor));
            }

            SdkResultado.Verificar(MgwServicios.fGuardaMovimiento());
        }
        catch
        {
            MgwServicios.fCancelaCambiosMovimiento();
            throw;
        }
    }

    /// <inheritdoc />
    public void BorrarDocumento(int idDocumento)
    {
        SdkResultado.Verificar(MgwServicios.fBuscarIdDocumento(idDocumento));
        SdkResultado.Verificar(MgwServicios.fBorraDocumento());
    }
}
