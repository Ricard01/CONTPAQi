# CLAUDE.md

Guía para Claude (y cualquier colaborador) al trabajar en este repositorio. Todo el código, comentarios de documentación y mensajes al usuario van en **español**; los identificadores de código pueden ir en inglés o español, pero de forma consistente dentro de cada archivo.

## Propósito

Web API que envuelve el SDK de **CONTPAQi Comercial Premium** para exponer sus operaciones (catálogos, documentos, timbrado, etc.) por HTTP, centralizando la conexión, el manejo de errores y la concurrencia contra el SDK.

## Arquitectura decidida

Estas decisiones son fijas; no proponer alternativas salvo que el usuario lo pida.

1. **Web API ASP.NET Core (.NET 10) ejecutándose como Servicio de Windows.**
   - Se usa `Microsoft.Extensions.Hosting.WindowsServices` (`builder.Services.AddWindowsService(...)`).
   - Debe poder ejecutarse también como consola para desarrollo (el mismo ejecutable detecta si corre como servicio).
   - Recordar que un servicio arranca con el directorio actual en `C:\Windows\System32`: nunca depender de rutas relativas; usar `AppContext.BaseDirectory` o rutas absolutas desde configuración.

2. **Se instala en el mismo servidor que Comercial Premium.**
   - El SDK (`MGWServicios.dll`) se carga desde la instalación local de Comercial Premium; no se redistribuye en el repositorio.
   - Las empresas (bases de datos) y la licencia son las del servidor local.

3. **Todas las operaciones al SDK se serializan con una cola (`System.Threading.Channels`) y un único hilo consumidor.**
   - El SDK no es seguro para múltiples hilos y mantiene estado global (empresa abierta, sesión, errores). Ninguna llamada al SDK se hace fuera de ese hilo.
   - Los endpoints/handlers **no** llaman al SDK directamente: encolan un trabajo y esperan su resultado (`TaskCompletionSource`) de forma asíncrona.
   - El consumidor es un hilo dedicado (`Thread` propio, no del `ThreadPool`) creado por un `BackgroundService`/`IHostedService`, que:
     - inicializa el SDK una sola vez al arrancar y lo termina al detenerse el servicio;
     - procesa los trabajos uno por uno, en orden;
     - abre/cierra la empresa según lo requiera cada trabajo (o la mantiene abierta si es la misma);
     - captura cualquier excepción del trabajo y la devuelve al llamador sin tumbar el hilo.
   - Soportar `CancellationToken`: si el llamador cancela antes de que el trabajo empiece, se descarta; una vez iniciado en el SDK, se deja terminar.
   - Preferir un canal acotado (`Channel.CreateBounded`) para no acumular trabajo sin límite; el tamaño va en configuración.

4. **El MGWSDK de CONTPAQi solo funciona en x86 (32 bits).**
   - Todos los proyectos que terminan cargando la DLL (Web y cualquier proyecto de pruebas de integración) deben compilar y ejecutarse como **x86**: `<PlatformTarget>x86</PlatformTarget>` y `<RuntimeIdentifier>win-x86</RuntimeIdentifier>` al publicar.
   - Para depurar/ejecutar localmente se necesita el runtime de .NET 10 **x86** instalado.
   - Nunca cambiar a `AnyCPU`/x64: la carga de la DLL fallará con `BadImageFormatException`.

## Estructura de la solución

Clean architecture. Las dependencias apuntan hacia adentro: `Web → Infrastructure → Application → Domain`.

| Proyecto | Responsabilidad |
|---|---|
| `src/SDK.Comercial.Domain` | Entidades y value objects del negocio (clientes, productos, documentos, movimientos). Sin dependencias externas. |
| `src/SDK.Comercial.Application` | Casos de uso, DTOs, interfaces (puertos) como el ejecutor de operaciones del SDK y los repositorios/servicios de CONTPAQi. No conoce el SDK nativo. |
| `src/SDK.Comercial.Infrastructure` | Interop con `MGWServicios.dll` (P/Invoke), la cola de Channels con su hilo consumidor, implementación de los puertos, lectura de configuración del SDK. **Único lugar donde existe código nativo.** |
| `src/SDK.Comercial.Web` | Host ASP.NET Core: endpoints, registro de dependencias, configuración como Servicio de Windows, manejo global de errores (ProblemDetails), OpenAPI. |
| `tests/SDK.Comercial.Infrastructure.Tests` | Pruebas (xUnit) de la cola y el hilo consumidor usando un SDK falso; no requieren la DLL. |

Piezas clave en Infrastructure:
- `Sdk/Native/ComercialSdkNative.cs`: declaraciones P/Invoke de `MGWServicios.dll`.
- `Sdk/ComercialSdk.cs` (`IComercialSdk`): inicio/término del SDK y apertura/cierre de empresa.
- `Sdk/Cola/SdkColaTrabajo.cs`: la cola (`EncolarAsync(contexto => ...)`), única puerta de entrada al SDK.
- `Sdk/Cola/SdkWorker.cs`: `BackgroundService` que crea el hilo consumidor dedicado.
- `Sdk/Cola/SdkContexto.cs`: estado dentro del hilo (`UsarEmpresa(ruta)` abre la empresa solo si cambió).
- Cada repositorio (p. ej. `Empresas/EmpresaRepository.cs`) implementa un puerto de Application encolando su trabajo.

Para agregar una operación nueva: definir el puerto en Application, implementarlo en Infrastructure con `cola.EncolarAsync(...)` llamando a `ComercialSdkNative` dentro de la lambda, registrarlo en `DependencyInjection.cs` y exponerlo con un endpoint en Web.

Reglas:
- El P/Invoke vive en una clase `static` interna de Infrastructure (p. ej. `ComercialSdkNative`); ningún otro proyecto la ve.
- Los códigos de error del SDK se traducen a mensajes con `fError` dentro del hilo consumidor y se convierten a excepciones/resultados propios antes de salir de Infrastructure.
- Las cadenas del SDK son ANSI con buffers de longitud fija; definir las constantes de longitud en un solo lugar.

## Notas del SDK

- Antes de inicializar, el directorio actual del proceso debe ser el de instalación de Comercial (`Directory.SetCurrentDirectory`) para que la DLL encuentre sus dependencias; la ruta se toma de configuración (con respaldo en el registro de Windows de la instalación de Comercial).
- Ciclo típico en el hilo consumidor: `fSetNombrePAQ` → `fAbreEmpresa` → operaciones → `fCierraEmpresa` → (al apagar) `fTerminaSDK`.
- El timbrado de documentos requiere licencia de 5 usuarios.

## Configuración

- `appsettings.json` → sección `ComercialSdk` (ruta de instalación, nombre del sistema, tamaño de la cola, etc.).
- Credenciales (usuario/contraseña de Comercial, contraseña de CSD) **nunca** en el repositorio: usar user-secrets en desarrollo y variables de entorno o configuración del servidor en producción.

## Convenciones de build

- .NET SDK fijado en `global.json` (10.0.x).
- Gestión central de paquetes: las versiones van **solo** en `Directory.Packages.props`; en los `.csproj` se usa `<PackageReference Include="..." />` sin versión.
- `Directory.Build.props` activa `TreatWarningsAsErrors`, `Nullable` e `ImplicitUsings`: el código debe compilar sin advertencias.
- Salida de compilación en `artifacts/` (`ArtifactsPath`).

## Comandos

```powershell
dotnet build CONTPAQi.slnx
dotnet test CONTPAQi.slnx
dotnet run --project src/SDK.Comercial.Web            # ejecuta como consola (x86)
dotnet publish src/SDK.Comercial.Web -c Release -r win-x86 --self-contained false
# Instalar como servicio (PowerShell como administrador):
sc.exe create "CONTPAQi.SDK.Comercial" binPath= "C:\ruta\publicada\SDK.Comercial.Web.exe" start= auto
```

## Limitaciones del entorno

- El SDK es una DLL nativa de Windows y requiere Comercial Premium instalado y licenciado. En Linux o en entornos de CI sin Comercial solo se puede compilar y probar la lógica que no toca el SDK; las pruebas contra el SDK real se hacen en la máquina Windows del usuario.
- Diseñar el código para que el acceso al SDK esté detrás de interfaces y así poder probar Application y la cola sin la DLL.
