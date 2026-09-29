using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SDK.Comercial.Application.Common.Exceptions;

namespace SDK.Comercial.Web.Errores;

/// <summary>
/// Convierte los errores del SDK en respuestas ProblemDetails.
/// </summary>
internal sealed class ComercialSdkExceptionHandler(IProblemDetailsService problemDetails) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ComercialSdkException sdkException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        var detalle = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Error del SDK de CONTPAQi",
            Detail = sdkException.Message,
        };
        if (sdkException.Codigo is { } codigo)
        {
            detalle.Extensions["codigo"] = codigo;
        }

        return await problemDetails.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = detalle,
            Exception = exception,
        });
    }
}
