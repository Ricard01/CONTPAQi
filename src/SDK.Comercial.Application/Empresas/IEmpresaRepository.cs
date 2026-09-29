using SDK.Comercial.Domain.Empresas;

namespace SDK.Comercial.Application.Empresas;

public interface IEmpresaRepository
{
    Task<IReadOnlyList<Empresa>> ListarAsync(CancellationToken cancellationToken = default);
}
