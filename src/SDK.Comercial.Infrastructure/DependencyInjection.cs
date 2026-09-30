using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SDK.Comercial.Application.Conexion;
using SDK.Comercial.Application.Empresas;
using SDK.Comercial.Infrastructure.Conexion;
using SDK.Comercial.Infrastructure.Empresas;
using SDK.Comercial.Infrastructure.Sdk;
using SDK.Comercial.Infrastructure.Sdk.Cola;

namespace SDK.Comercial.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Convierte la sección "ComercialSdk" de appsettings y de las demás fuentes de configuración en un objeto ComercialSdkOptions inyectable mediante IOptions<T>.
        services.Configure<ComercialSdkOptions>(configuration.GetSection(ComercialSdkOptions.Seccion));

        // Estos objetos son Singleton porque representan un solo SDK nativo, una sola cola y una sola empresa activa para toda la vida del proceso. Registrar otra instancia
        // permitiría sesiones concurrentes sobre un SDK que mantiene estado global.
        services.AddSingleton<IComercialSdk, ComercialSdk>();
        services.AddSingleton<SdkColaTrabajo>();

        // ASP.NET Core inicia este BackgroundService automáticamente al arrancar la aplicación y solicita su detención al apagarla. El worker es dueño del hilo exclusivo del SDK.
        services.AddHostedService<SdkWorker>();

        // Los endpoints reciben estas interfaces; sus implementaciones no llaman al SDK desde el hilo HTTP, sino que colocan el trabajo en SdkColaTrabajo.
        services.AddSingleton<IConexionComercial, ConexionComercial>();
        services.AddSingleton<IEmpresaRepository, EmpresaRepository>();

        return services;
    }
}
