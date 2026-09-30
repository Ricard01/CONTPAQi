using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SDK.Comercial.Application.Common.Exceptions;

namespace SDK.Comercial.Web.Infrastructure;

/// <summary>
/// Convierte las excepciones conocidas de la aplicación  y del SDK de CONTPAQi
/// en respuestas ProblemDetails conformes con RFC 9110.
/// </summary>
internal sealed class ProblemDetailsExceptionHandler(IProblemDetailsService problemDetailsService, ILogger<ProblemDetailsExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Traduce la excepción a un ProblemDetails solo cuando es un error conocido.
        // Si devuelve null, este manejador no se hace responsable y ASP.NET Core puede
        // ofrecer la excepción al siguiente manejador registrado.
        var detalle = CrearProblemDetails(exception);
        if (detalle is null)
        {
            return false;
        }

        // El código se guarda dentro de ProblemDetails y se reutiliza para la respuesta HTTP.
        // Así existe una sola fuente de verdad y no hay que mantener por separado una tupla
        // como (statusCode, problemDetails).
        detalle.Instance = httpContext.Request.Path;
        detalle.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;
        httpContext.Response.StatusCode = detalle.Status!.Value;

        Registrar(exception, detalle.Status.Value);

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = detalle,
            Exception = exception,
        });
    }

    /// <summary>
    /// Relaciona cada tipo de excepción conocida con el ProblemDetails que debe recibir el cliente.
    /// El tipo de retorno sirve como destino común para ProblemDetails y ValidationProblemDetails,
    /// por lo que no es necesario convertir este último explícitamente a ProblemDetails.
    /// </summary>
    private static ProblemDetails? CrearProblemDetails(Exception exception) => exception switch
    {
        ValidationException validacion => new ValidationProblemDetails(validacion.Errors)
        {
            Status = StatusCodes.Status400BadRequest,
            Type = "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.1",
            Title = "Uno o más datos no son válidos.",
            Detail = validacion.Message,
        },
        NotFoundException noEncontrado => new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Type = "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.5",
            Title = "No se encontró el recurso solicitado.",
            Detail = noEncontrado.Message,
        },
        UnauthorizedAccessException => new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Type = "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.2",
            Title = "No autenticado.",
        },
        ForbiddenAccessException => new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Type = "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.5.4",
            Title = "Acceso denegado.",
        },
        ComercialSdkException sdkException => CrearDetalleSdk(sdkException),
        _ => null,
    };

    /// <summary>
    /// Construye el detalle específico de CONTPAQi. Está separado del switch porque el código
    /// nativo es opcional y agregarlo condicionalmente no cabe limpiamente en un inicializador.
    /// </summary>
    private static ProblemDetails CrearDetalleSdk(ComercialSdkException exception)
    {
        var detalle = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://www.rfc-editor.org/rfc/rfc9110.html#section-15.6.1",
            Title = "Error del SDK de CONTPAQi Comercial.",
            Detail = exception.Message,
        };

        if (exception.Codigo is { } codigo)
        {
            detalle.Extensions["codigo"] = codigo;
        }

        return detalle;
    }

    /// <summary>Registra los errores conocidos con una severidad acorde con su origen.</summary>
    private void Registrar(Exception exception, int statusCode)
    {
        if (exception is ComercialSdkException)
        {
            logger.LogError(exception, "El SDK de CONTPAQi devolvió un error HTTP {StatusCode}", statusCode);
            return;
        }

        logger.LogWarning(exception, "La solicitud terminó con un error controlado HTTP {StatusCode}", statusCode);
    }
}
