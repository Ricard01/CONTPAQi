# CLAUDE.md — SDK.Comercial.Web

## Endpoints (Minimal APIs)
- Cada grupo de endpoints es una clase pública en `Endpoints/` que implementa `IEndpointGroup`.
- `WebApplicationExtensions.MapEndpoints` los descubre por reflexión y los registra en `/api/{ClassName}` (salvo que se sobrescriba `RoutePrefix`) con tag OpenAPI `{ClassName}`.
- Usar los extensores de `Infrastructure/EndpointRouteBuilderExtensions.cs` (`MapGet`, `MapPost`, `MapPut`, `MapPatch`, `MapDelete`): exigen métodos con nombre (prohíben lambdas anónimas vía `Guard.Against.AnonymousMethod`) para generar el `operationId` de OpenAPI.
- Documentar endpoints con `[EndpointSummary]` y `[EndpointDescription]` y retornar `TypedResults.*`.

## Errores y configuración
- `ProblemDetailsExceptionHandler` (`IExceptionHandler`) mapea:
  - `ValidationException` → `400 Bad Request` (`ValidationProblemDetails`)
  - `NotFoundException` (de `Ardalis.GuardClauses`) → `404 Not Found`
  - `UnauthorizedAccessException` → `401 Unauthorized`
  - `ForbiddenAccessException` → `403 Forbidden`
  - `ComercialSdkException` → `500 Internal Server Error` (incluye `codigo` si existe)
- `Program.cs` fija `ContentRootPath = AppContext.BaseDirectory` y registra `AddWindowsService` (`"CONTPAQi.SDK.Comercial"`).
- Swagger UI (`/swagger`) y `/openapi/v1.json` se exponen solo en `Development`. Puertos locales en `launchSettings.json`: `http://localhost:5087` y `https://localhost:7088`.
