using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SDK.Comercial.Application.Empresas;
using SDK.Comercial.Infrastructure.Empresas;
using SDK.Comercial.Infrastructure.Sdk;
using SDK.Comercial.Infrastructure.Sdk.Cola;

namespace SDK.Comercial.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ComercialSdkOptions>(configuration.GetSection(ComercialSdkOptions.Seccion));

        services.AddSingleton<IComercialSdk, ComercialSdk>();
        services.AddSingleton<SdkColaTrabajo>();
        services.AddHostedService<SdkWorker>();

        services.AddSingleton<IEmpresaRepository, EmpresaRepository>();

        return services;
    }
}
