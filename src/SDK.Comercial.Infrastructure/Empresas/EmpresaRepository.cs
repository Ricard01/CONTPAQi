using System.Text;
using SDK.Comercial.Application.Empresas;
using SDK.Comercial.Domain.Empresas;
using SDK.Comercial.Infrastructure.Sdk.Cola;
using SDK.Comercial.Infrastructure.Sdk.Native;

namespace SDK.Comercial.Infrastructure.Empresas;

internal sealed class EmpresaRepository(SdkColaTrabajo cola) : IEmpresaRepository
{
    public Task<IReadOnlyList<Empresa>> ListarAsync(CancellationToken cancellationToken = default) =>
        cola.EncolarAsync<IReadOnlyList<Empresa>>(_ => Leer(), cancellationToken);

    private static List<Empresa> Leer()
    {
        var empresas = new List<Empresa>();
        var id = 0;
        var nombre = new StringBuilder(ComercialSdkNative.TamanoBuffer);
        var ruta = new StringBuilder(ComercialSdkNative.TamanoBuffer);

        var resultado = ComercialSdkNative.fPosPrimerEmpresa(ref id, nombre, ruta);
        while (resultado == 0)
        {
            empresas.Add(new Empresa(id, nombre.ToString().Trim(), ruta.ToString().Trim()));
            nombre.Clear();
            ruta.Clear();
            resultado = ComercialSdkNative.fPosSiguienteEmpresa(ref id, nombre, ruta);
        }

        return empresas;
    }
}
