# CLAUDE.md — tests

- `SDK.Comercial.Infrastructure.Tests`: pruebas unitarias rápidas en AnyCPU usando `Fakes/` (`FakeDocumentosSdk`, `FakeSesionComercialSdk`). Deben correr en cualquier entorno sin Comercial instalado.
- `SDK.Comercial.Infrastructure.IntegrationTests`: pruebas `x86` contra `MGWServicios.dll` real usando `[FactIntegracion]`.
  - Se omiten automáticamente salvo que `COMERCIAL_PRUEBAS_INTEGRACION=1` y existan en variables de entorno: `ComercialSdk__RutaEmpresa`, `Integracion__Concepto`, `Integracion__Cliente`, `Integracion__Producto`, `Integracion__Almacen` (y opcionalmente `ComercialSdk__Usuario`, `ComercialSdk__Contrasena`, `ComercialSdk__RutaInstalacion`).
  - Deben apuntar a una empresa de pruebas y limpiar los documentos creados al finalizar.
