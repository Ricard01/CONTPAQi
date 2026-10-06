using Microsoft.Extensions.Configuration;
using SDK.Comercial.Infrastructure.Sdk;

namespace SDK.Comercial.Infrastructure.IntegrationTests;

/// <summary>
/// Datos para las pruebas contra el SDK real, leídos de variables de entorno (nunca del repositorio):
/// <list type="bullet">
/// <item><c>COMERCIAL_PRUEBAS_INTEGRACION=1</c>: habilita las pruebas; escriben en la empresa indicada.</item>
/// <item><c>ComercialSdk__RutaEmpresa</c>, <c>ComercialSdk__Usuario</c>, <c>ComercialSdk__Contrasena</c>
/// y opcionalmente <c>ComercialSdk__RutaInstalacion</c>: igual que en el servicio. Usar una empresa de pruebas.</item>
/// <item><c>Integracion__Concepto</c>, <c>Integracion__Cliente</c>, <c>Integracion__Producto</c>,
/// <c>Integracion__Almacen</c>: códigos que ya existen en esa empresa.</item>
/// </list>
/// </summary>
internal sealed class ConfiguracionIntegracion
{
    private static readonly Lazy<ConfiguracionIntegracion> Instancia = new(() => new ConfiguracionIntegracion());

    private ConfiguracionIntegracion()
    {
        var configuracion = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        Sdk = configuracion.GetSection(ComercialSdkOptions.Seccion).Get<ComercialSdkOptions>() ?? new ComercialSdkOptions();
        Concepto = configuracion["Integracion:Concepto"];
        Cliente = configuracion["Integracion:Cliente"];
        Producto = configuracion["Integracion:Producto"];
        Almacen = configuracion["Integracion:Almacen"];
        MotivoOmision = CalcularMotivoOmision(configuracion["COMERCIAL_PRUEBAS_INTEGRACION"]);
    }

    public static ConfiguracionIntegracion Actual => Instancia.Value;

    public ComercialSdkOptions Sdk { get; }

    public string? Concepto { get; }

    public string? Cliente { get; }

    public string? Producto { get; }

    public string? Almacen { get; }

    /// <summary>Nulo si las pruebas pueden ejecutarse; si no, la razón por la que se omiten.</summary>
    public string? MotivoOmision { get; }

    private string? CalcularMotivoOmision(string? habilitadas)
    {
        if (habilitadas != "1")
        {
            return "Pruebas contra el SDK real deshabilitadas. Define COMERCIAL_PRUEBAS_INTEGRACION=1 para ejecutarlas.";
        }

        if (!OperatingSystem.IsWindows() || Environment.Is64BitProcess)
        {
            return "El SDK de CONTPAQi requiere Windows y un proceso x86 (instala el runtime de .NET x86).";
        }

        var faltantes = new (string Nombre, string? Valor)[]
            {
                ("ComercialSdk__RutaEmpresa", Sdk.RutaEmpresa),
                ("Integracion__Concepto", Concepto),
                ("Integracion__Cliente", Cliente),
                ("Integracion__Producto", Producto),
                ("Integracion__Almacen", Almacen),
            }
            .Where(v => string.IsNullOrWhiteSpace(v.Valor))
            .Select(v => v.Nombre)
            .ToList();

        return faltantes.Count > 0 ? $"Faltan variables de entorno: {string.Join(", ", faltantes)}." : null;
    }
}

/// <summary>Prueba que solo se ejecuta cuando <see cref="ConfiguracionIntegracion"/> está completa; si no, aparece como omitida.</summary>
public sealed class FactIntegracionAttribute : FactAttribute
{
    public FactIntegracionAttribute()
    {
        Skip = ConfiguracionIntegracion.Actual.MotivoOmision;
    }
}
