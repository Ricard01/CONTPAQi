using SDK.Comercial.Application.Common.Exceptions;
using SDK.Comercial.Infrastructure.Sdk.Native.DatosAbstractos;

namespace SDK.Comercial.Infrastructure.Sdk;

/// <summary>
/// Funciones nativas de documentos y movimientos que usan los repositorios. Existe para poder probar
/// la lógica de los repositorios con un SDK falso; la implementación real es <see cref="DocumentosSdk"/>.
/// Solo debe invocarse dentro de una operación de <see cref="Cola.SdkColaTrabajo"/>.
/// Todos los métodos lanzan <see cref="ComercialSdkException"/> si la función nativa devuelve error.
/// </summary>
internal interface IDocumentosSdk
{
    /// <summary>Siguiente folio disponible del concepto (<c>fSiguienteFolio</c>), con la serie resuelta por el SDK.</summary>
    (string Serie, double Folio) SiguienteFolio(string codigoConcepto, string serie);

    /// <summary>Da de alta el encabezado del documento (<c>fAltaDocumento</c>) y devuelve su identificador.</summary>
    int AltaDocumento(tDocumento documento);

    /// <summary>Da de alta un movimiento del documento (<c>fAltaMovimiento</c>) y devuelve su identificador.</summary>
    int AltaMovimiento(int idDocumento, tMovimiento movimiento);

    /// <summary>Localiza el documento por su identificador y lo borra (<c>fBuscarIdDocumento</c> + <c>fBorraDocumento</c>).</summary>
    void BorrarDocumento(int idDocumento);
}
