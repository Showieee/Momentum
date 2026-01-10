using Momentum.Services.Models;

namespace Momentum.Services.Interfaces;

public interface ICompanyService
{
    Task<List<CompanyModel>> Get();

    Task<CompanyModel?> GetById(Guid id);

    Task Insert(CompanyModel company);

    Task Update(Guid id, CompanyModel company);

    Task Delete(Guid id);
}
