namespace SDK.Comercial.Application.Common.Exceptions;

/// <summary>
/// Error devuelto por el SDK de CONTPAQi Comercial, o fallo al comunicarse con él.
/// </summary>
public sealed class ComercialSdkException : Exception
{
    public ComercialSdkException(string message) : base(message)
    {
    }

    public ComercialSdkException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ComercialSdkException(int codigo, string message) : base(message)
    {
        Codigo = codigo;
    }

    /// <summary>Código de error del SDK, cuando el error viene de una función del SDK.</summary>
    public int? Codigo { get; }
}
