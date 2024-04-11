using Microsoft.Extensions.FileProviders;
using ms.MainApi.Business.Middlewares;
using ms.MainApi.Business.ServiceRegistrations;
using System.Runtime.InteropServices;

//var isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
//string contentRoot = isLinux ? Directory.GetCurrentDirectory() : @"C:\publish";

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//Register all services below AddServices function
builder.Services.AddServices(builder.Configuration, builder.Environment);

var app = builder.Build();

//if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors(builder.Configuration.GetSection("CorsLabel").Value!);
app.UseAuthentication();
app.UseRouting();          //test
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();

//app.UseApplicationMiddleware();

app.MapControllerRoute(name: "default", pattern: "{controller=Avatar}/{action=Index}/{id?}");       //test

app.Run();
