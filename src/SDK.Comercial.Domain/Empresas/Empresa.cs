namespace SDK.Comercial.Domain.Empresas;

/// <summary>
/// Empresa (base de datos) registrada en CONTPAQi Comercial Premium.
/// </summary>
public sealed record Empresa(int Id, string Nombre, string Ruta);
