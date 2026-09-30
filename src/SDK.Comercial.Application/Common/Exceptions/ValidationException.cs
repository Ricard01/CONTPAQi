namespace SDK.Comercial.Application.Common.Exceptions;

/// <summary>
/// Indica que uno o más datos de entrada no cumplen las reglas de validación.
/// </summary>
public sealed class ValidationException : Exception
{
    public ValidationException(IDictionary<string, string[]> errors)
        : base("Ocurrieron uno o más errores de validación.")
    {
        Errors = new Dictionary<string, string[]>(errors);
    }

    public IDictionary<string, string[]> Errors { get; }
}
