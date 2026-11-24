# CLAUDE.md

Guía del repositorio **CONTPAQi** (`CONTPAQi.slnx`). Todo el código, comentarios XML y mensajes al usuario van en **español**; los identificadores pueden ser en inglés o español de forma consistente por archivo.

## Propósito y arquitectura
Web API en **ASP.NET Core (.NET 10)** ejecutada como Servicio de Windows (o consola en desarrollo) en el servidor de **CONTPAQi Comercial Premium**, envolviendo `MGWServicios.dll`.

Clean Architecture (`Web → Infrastructure → Application → Domain`):
- `src/SDK.Comercial.Domain`: entidades puras sin dependencias.
- `src/SDK.Comercial.Application`: puertos, DTOs con validación propia y excepciones de aplicación.
- `src/SDK.Comercial.Infrastructure`: único lugar con P/Invoke (`MGWServicios.dll`), cola de un solo hilo y repositorios.
- `src/SDK.Comercial.Web`: Minimal APIs (`IEndpointGroup`), `ProblemDetails`, OpenAPI y host `WindowsService`.
- `tests/SDK.Comercial.Infrastructure.Tests`: pruebas unitarias xUnit usando `Fakes/` (sin DLL real).
- `tests/SDK.Comercial.Infrastructure.IntegrationTests`: pruebas x86 contra SDK real (omitidas sin env vars).

## Reglas no negociables
1. **Solo x86 (32 bits):** `MGWServicios.dll` falla con `BadImageFormatException` en x64/AnyCPU. `Web` (`win-x86`, `SelfContained=true`) e `IntegrationTests` usan `PlatformTarget=x86`.
2. **Hilo único del SDK:** ninguna llamada a `MGWServicios` fuera de `SdkColaTrabajo.EncolarAsync(...)` (`Channel` acotado + `Thread` dedicado en `SdkWorker`).
3. **API mono-empresa:** se usa siempre `ComercialSdk:RutaEmpresa`. No agregar cambio de empresa por petición.
4. **Servicio de Windows:** arranca en `System32`; nunca usar rutas relativas (usar `AppContext.BaseDirectory` o configuración).
5. **Sin secretos en el repo:** credenciales (`ComercialSdk:*`) solo vía user-secrets o variables de entorno.

## Convenciones de build (.NET 10)
- SDK fijado en `global.json` (`10.0.401`).
- `Directory.Build.props` activa `net10.0`, `TreatWarningsAsErrors`, `Nullable`, `ImplicitUsings` y salida en `artifacts/`.
- `Directory.Packages.props` centraliza versiones NuGet: en los `.csproj` usar `<PackageReference Include="..." />` sin `Version`.

## Comandos exactos
```powershell
dotnet build CONTPAQi.slnx
dotnet test tests/SDK.Comercial.Infrastructure.Tests/SDK.Comercial.Infrastructure.Tests.csproj
dotnet test CONTPAQi.slnx
dotnet run --project src/SDK.Comercial.Web
dotnet publish src/SDK.Comercial.Web -c Release -r win-x86
```

## Guías por subproyecto
- `src/SDK.Comercial.Infrastructure/CLAUDE.md` — P/Invoke, cola del SDK, repositorios y `IDocumentosSdk`.
- `src/SDK.Comercial.Web/CLAUDE.md` — endpoints `IEndpointGroup`, `ProblemDetails` y configuración.
- `tests/CLAUDE.md` — pruebas unitarias con fakes y pruebas de integración x86.
