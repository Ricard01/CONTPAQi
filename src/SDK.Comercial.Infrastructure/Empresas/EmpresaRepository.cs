using System.Text;
using SDK.Comercial.Application.Empresas;
using SDK.Comercial.Domain.Empresas;
using SDK.Comercial.Infrastructure.Sdk.Cola;
using SDK.Comercial.Infrastructure.Sdk.Native;

namespace SDK.Comercial.Infrastructure.Empresas;

/// <summary>
/// Consulta el catálogo de empresas publicado por MGWServicios.dll. La enumeración se envía
/// a la cola para que todas las funciones nativas se ejecuten en el mismo hilo que la sesión.
/// </summary>
internal sealed class EmpresaRepository(SdkColaTrabajo cola) : IEmpresaRepository
{
    public Task<IReadOnlyList<Empresa>> ListarAsync(CancellationToken cancellationToken = default) =>
        cola.EncolarAsync<IReadOnlyList<Empresa>>(_ => Leer(), cancellationToken);

    private static List<Empresa> Leer()
    {
        // Las funciones posicionales escriben nombre y directorio en buffers ANSI proporcionados
        // por el llamador. El tamaño compartido pertenece al contrato nativo, no a una sesión.
        var empresas = new List<Empresa>();
        var id = 0;
        var nombre = new StringBuilder(MgwServicios.TamanoBuffer);
        var ruta = new StringBuilder(MgwServicios.TamanoBuffer);

        // Un resultado igual a cero confirma que el SDK se posicionó en el primer registro y
        // escribió en los parámetros el identificador, el nombre y el directorio de la empresa.
        var resultado = MgwServicios.fPosPrimerEmpresa(ref id, nombre, ruta);
        while (resultado == 0)
        {
            empresas.Add(new Empresa(id, nombre.ToString().Trim(), ruta.ToString().Trim()));
            nombre.Clear();
            ruta.Clear();

            // El contrato usa un valor distinto de cero cuando no existe otro registro. Por eso
            // la navegación lo trata como fin de la secuencia. La firma nativa no permite distinguir
            // aquí ese final de otros códigos de error, así que el resultado no se envía a
            // SdkResultado.Verificar.
            resultado = MgwServicios.fPosSiguienteEmpresa(ref id, nombre, ruta);
        }

        return empresas;
    }
}
