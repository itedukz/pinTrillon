using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ms.MainApi.Business.ServiceRegistrations;

public static class ServiceRegistration
{
    public static IServiceCollection AddServices
        (this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddDalServices(configuration, environment);
        services.AddMediatrServices(configuration, environment);
        services.AddCoreServices(configuration, environment);
        services.AddFluentValidationServices(configuration, environment);
        services.AddAutoMapServices(configuration, environment);
        services.AddHostedServices(configuration, environment);
        //services.AddAuthenticationServices(configuration, environment);
        return services;
    }
}