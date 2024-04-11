using Microsoft.AspNetCore.Builder;

namespace ms.MainApi.Business.Middlewares;

public static class MiddlewareRegistrations
{
    public static IApplicationBuilder UseApplicationMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}
