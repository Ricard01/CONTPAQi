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
    /// <summary>Serie del concepto y su siguiente folio disponible (<c>fSiguienteFolio</c>).</summary>
    (string Serie, double Folio) SiguienteFolio(string codigoConcepto);

    /// <summary>Da de alta el encabezado del documento (<c>fAltaDocumento</c>) y devuelve su identificador.</summary>
    int AltaDocumento(tDocumento documento);

    /// <summary>Da de alta un movimiento con descuentos (<c>fAltaMovimientoCDesct</c>) y devuelve su identificador.</summary>
    int AltaMovimiento(int idDocumento, tMovimientoDesc movimiento);

    /// <summary>
    /// Localiza el documento, lo edita y escribe los campos indicados
    /// (<c>fBuscarIdDocumento</c> → <c>fEditarDocumento</c> → <c>fSetDatoDocumento</c> → <c>fGuardaDocumento</c>).
    /// Si algún campo falla, descarta la edición.
    /// </summary>
    void ActualizarDocumento(int idDocumento, IReadOnlyList<(string Campo, string Valor)> campos);

    /// <summary>
    /// Localiza el movimiento, lo edita y escribe los campos indicados
    /// (<c>fBuscarIdMovimiento</c> → <c>fEditarMovimiento</c> → <c>fSetDatoMovimiento</c> → <c>fGuardaMovimiento</c>).
    /// Si algún campo falla, descarta la edición.
    /// </summary>
    void ActualizarMovimiento(int idMovimiento, IReadOnlyList<(string Campo, string Valor)> campos);

    /// <summary>Localiza el documento por su identificador y lo borra (<c>fBuscarIdDocumento</c> + <c>fBorraDocumento</c>).</summary>
    void BorrarDocumento(int idDocumento);

    /// <summary>
    /// Localiza el documento por su identificador y devuelve los datos que identifican su tipo
    /// (<c>fBuscarIdDocumento</c> → <c>fLeeDatoDocumento</c> → <c>fBuscaIdConceptoDocto</c>).
    /// Devuelve <c>null</c> si no existe.
    /// </summary>
    DatosDocumentoSdk? LeerDocumento(int idDocumento);

    /// <summary>
    /// UUID del CFDI del documento (<c>fDocumentoUUID</c>); vacío si no está timbrado.
    /// Solo aplica a documentos con folio digital (facturas): para otros tipos el SDK responde error.
    /// </summary>
    string ObtenerUuid(string codigoConcepto, string serie, double folio);
}

/// <param name="TipoDocumento">Valor de <c>CIDDOCUMENTODE</c> (4 = factura).</param>
/// <param name="CodigoConcepto">Código del concepto del documento.</param>
/// <param name="Serie">Serie del documento.</param>
/// <param name="Folio">Folio del documento.</param>
internal sealed record DatosDocumentoSdk(int TipoDocumento, string CodigoConcepto, string Serie, double Folio);
