using SDK.Comercial.Infrastructure;
using SDK.Comercial.Web.Infrastructure;


var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    // Como servicio de Windows, el directorio actual es System32. Sin esto no se encuentra appsettings.json y la configuración llega vacía (falla la validación de ComercialSdkOptions).
    ContentRootPath = AppContext.BaseDirectory,
});

builder.Services.AddWindowsService(options => options.ServiceName = "CONTPAQi.SDK.Comercial");

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ProblemDetailsExceptionHandler>();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "CONTPAQi SDK Comercial v1");

        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

// Descubre automáticamente cada clase pública que implementa IEndpointGroup.
app.MapEndpoints(typeof(Program).Assembly);

app.Run();
