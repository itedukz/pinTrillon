using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ms.MainApi.Business.ExpressionParser;
using ms.MainApi.Business.Services;
using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Core.GeneralHelpers.RabbitMq;
using Newtonsoft.Json;
using System.Text;

namespace ms.MainApi.Business.ServiceRegistrations;

public static class CoreServiceRegistrations
{
    public static IServiceCollection AddCoreServices
        (this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        //services.AddGrpc(x =>
        //{
        //    x.MaxReceiveMessageSize = 1024 * 1024 * 1024;
        //    x.MaxSendMessageSize = 1024 * 1024 * 1024;
        //});


        //services.AddControllers();
        services.AddControllersWithViews(); //test
        services.AddEndpointsApiExplorer();

        #region Authorizations Information
        services.AddAuthentication(x => x.DefaultAuthenticateScheme = "grcAuth")
            .AddJwtBearer("grcAuth", x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            Success = false,
                            Message = "Invalid access token."
                        }));
                    }
                };
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.ASCII.GetBytes(configuration.GetSection("AuthInformation").GetSection("SigningKey").Value!)),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidIssuer = configuration.GetSection("AuthInformation").GetSection("Issuer").Value,
                    ValidateAudience = true,
                    ValidAudience = configuration.GetSection("AuthInformation").GetSection("Audience").Value,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        //services.AddAuthentication(x => x.DefaultAuthenticateScheme = "googleAuth")
        //    .AddCookie()
        //    .AddGoogle(googleOptions =>
        //    {
        //        googleOptions.ClientId = "773076537519-sakt0asbcdbd3c41jtakh4cmh1ks8kch.apps.googleusercontent.com"; //configuration["Authentication:Google:ClientId"];
        //        googleOptions.ClientSecret = "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"; // configuration["Authentication:Google:ClientSecret"];
        //        googleOptions.SaveTokens = true;
        //    });
        services.AddAuthorization();
        #endregion

        services.AddSwaggerGen(s =>
        {
            var xmlPath = Path.Combine(AppContext.BaseDirectory, "ms.MainApi.xml");
            s.IncludeXmlComments(xmlPath);

            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ms.MainApi",
            });
            s.EnableAnnotations();
            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"Enter 'Bearer' [space] and your token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "grcAuth",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });

        #region Cors register origins and method types from appsettings.json
        services.AddCors(options =>
        {
            options.AddPolicy(name: configuration.GetSection("CorsLabel").Value!,
                builder =>
                {
                    builder.WithMethods(
                        configuration.GetSection("Methods").GetChildren().Select(x => x.Value).ToArray()!);
                    builder.AllowAnyHeader();
                    builder.AllowCredentials();
                    builder.WithOrigins(
                        configuration.GetSection("Origins").GetChildren().Select(x => x.Value).ToArray()!);
                    builder.Build();
                });
        });

        #endregion

        #region Core Registrations
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddSingleton<IAuthInformationRepository, AuthInformationRepository>();
        services.AddSingleton<IMessagesRepository, MessagesRepository>();
        services.AddSingleton<IRabbitMqHelperRepository, RabbitMqHelperRepository>();
        services.AddSingleton<IImageRepository, ImageRepository>();
        services.AddSingleton<ISearchProductExpressionParser, SearchProductExpressionParser>();
        services.AddSingleton<ISearchProjectExpressionParser, SearchProjectExpressionParser>();

        services.AddScoped<ITokenCacheService, TokenCacheService>();
        services.AddScoped<IPermissionCheck, PermissionCheck>();

        #endregion

        return services;
    }
}