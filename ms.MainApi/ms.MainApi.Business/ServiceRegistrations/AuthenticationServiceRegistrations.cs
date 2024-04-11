using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ms.MainApi.Business.ServiceRegistrations;

public static class AuthenticationServiceRegistrations
{
    public static IServiceCollection AddAuthenticationServices
        (this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {

        services.AddAuthentication().AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
        });

        services.AddAuthentication()
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.ClientId = configuration["Authentication:Facebook:ClientId"];
                facebookOptions.ClientSecret = configuration["Authentication:Facebook:ClientSecret"];
            });
        //.AddTwitter(twitterOptions => { ... })

        return services;
    }
}
