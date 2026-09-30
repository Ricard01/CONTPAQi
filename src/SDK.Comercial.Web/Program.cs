using Microsoft.Extensions.Hosting.WindowsServices;
using SDK.Comercial.Infrastructure;
using SDK.Comercial.Web.Errores;
using SDK.Comercial.Web.Infrastructure;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    // Como servicio el directorio actual es System32; appsettings.json se busca junto al ejecutable.
    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default,
});

builder.Services.AddWindowsService(options => options.ServiceName = "CONTPAQi.SDK.Comercial");

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ComercialSdkExceptionHandler>();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapEndpoints(typeof(Program).Assembly);

app.Run();
