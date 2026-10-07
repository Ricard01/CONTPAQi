using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SDK.Comercial.Application.Conexion;
using SDK.Comercial.Application.Empresas;
using SDK.Comercial.Application.Facturas;
using SDK.Comercial.Infrastructure.Conexion;
using SDK.Comercial.Infrastructure.Empresas;
using SDK.Comercial.Infrastructure.Facturas;
using SDK.Comercial.Infrastructure.Sdk;
using SDK.Comercial.Infrastructure.Sdk.Cola;

namespace SDK.Comercial.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registra los servicios de infraestructura que la aplicación necesita para trabajar con el SDK de CONTPAQi Comercial.
    /// </summary>
    /// <param name="services">Contenedor donde se registran los servicios.</param>
    /// <param name="configuration">Configuración de la aplicación, incluida la sección ComercialSdk.</param>
    /// <returns>El mismo contenedor de servicios para permitir encadenar registros.</returns>
    /// <remarks>Agrega la configuración, la sesión y la cola del SDK, el worker en segundo plano, los componentes de conexión y acceso a empresas al contenedor de servicios.</remarks>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Convierte la sección "ComercialSdk" de appsettings y de las demás fuentes de
        // configuración en ComercialSdkOptions. ValidateOnStart evita iniciar el servicio con
        // valores que inevitablemente impedirían crear la sesión o la cola.
        services.AddOptions<ComercialSdkOptions>()
            .Bind(configuration.GetSection(ComercialSdkOptions.Seccion))
            .Validate(o => !string.IsNullOrWhiteSpace(o.Usuario),
                $"{ComercialSdkOptions.Seccion}:Usuario es obligatorio.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.RutaEmpresa),
                $"{ComercialSdkOptions.Seccion}:RutaEmpresa es obligatoria.")
            .ValidateOnStart();

        // Estos objetos son Singleton porque representan un solo SDK nativo, una sola cola y
        // una sola empresa activa para toda la vida del proceso. Registrar otra instancia
        // permitiría sesiones concurrentes sobre un SDK que mantiene estado global.
        services.AddSingleton<ISesionComercialSdk, SesionComercialSdk>();
        services.AddSingleton<SdkColaTrabajo>();
        services.AddSingleton<IDocumentosSdk, DocumentosSdk>();
        services.AddSingleton<IConexionComercial, ConexionComercial>();
        services.AddSingleton<IEmpresaRepository, EmpresaRepository>();
        services.AddSingleton<IFacturaRepository, FacturaRepository>();

        // NET Core inicia este BackgroundService automáticamente al arrancar la aplicación y solicita su detención al apagarla. El worker es dueño del hilo exclusivo del SDK.
        services.AddHostedService<SdkWorker>();

        return services;
    }
}