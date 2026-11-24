# CLAUDE.md — SDK.Comercial.Infrastructure

## Reglas del SDK nativo (`MGWServicios.dll`)
- Todo P/Invoke vive en `Sdk/Native/MgwServicios.*.cs` dentro de `internal static partial class MgwServicios` (organizado por área). Ningún otro proyecto referencia código nativo.
- Estructuras nativas en `Sdk/Native/DatosAbstractos/` usan `[StructLayout(LayoutKind.Sequential, Pack = 4, CharSet = CharSet.Ansi)]`.
- Las cadenas ANSI usan buffers fijos; reutilizar constantes de `Sdk/Native/Constantes/MgwConstantes.cs` y `MgwServicios.TamanoBuffer`.
- `Directory.SetCurrentDirectory` se ajusta a la ruta de instalación antes de inicializar el SDK (`ComercialSdk:RutaInstalacion` o Registro de Windows).

## Cola, sesión y repositorios
- Toda operación entra por `SdkColaTrabajo.EncolarAsync(contexto => ...)` y se registra como `Singleton` en `DependencyInjection.cs`.
- En operaciones que **escriben documentos** (ej. `FacturaRepository`): llamar `contexto.UsarEmpresa()` al iniciar y `contexto.CerrarEmpresa()` en `finally` para evitar bloqueos ("en uso por otro usuario") en Comercial; compensar con `BorrarDocumento` si falla a mitad.
- Traducir códigos nativos dentro del hilo consumidor con `SdkResultado.Verificar(...)` (`fError` → `ComercialSdkException`).
- Envolver nuevas operaciones nativas detrás de interfaces internas (como `IDocumentosSdk`) para probar repositorios sin la DLL real.
- `SDK.Comercial.Infrastructure.csproj` expone `internals` mediante `InternalsVisibleTo` a ambos proyectos de pruebas.
