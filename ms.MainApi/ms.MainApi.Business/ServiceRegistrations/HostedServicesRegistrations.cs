using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ms.MainApi.Business.Services.RabbitMqConsumers;

namespace ms.MainApi.Business.ServiceRegistrations;

public static class HostedServicesRegistrations
{
    public static IServiceCollection AddHostedServices
        (this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        #region Asset Category
        //services.AddHostedService<ProductPictureSendService>();

        #endregion

        

        return services;
    }
}