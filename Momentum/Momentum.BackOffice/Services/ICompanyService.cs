using Momentum.Shared.Models.Requests;
using Momentum.Shared.Models.Responses;
using Refit;

namespace Momentum.BackOffice.Services;

public interface ICompanyService
{
    [Get(Momentum.Shared.Routes.Companies.Get)]
    Task<IApiResponse<List<GetCompanyResponse>>> GetCompanies(CancellationToken ct = default);

    [Post(Momentum.Shared.Routes.Companies.Insert)]
    Task<IApiResponse> InsertCompany([Body] InsertCompanyRequest request, CancellationToken ct = default);

    [Put(Momentum.Shared.Routes.Companies.Update)]
    Task<IApiResponse> UpdateCompany(Guid id, [Body] UpdateCompanyRequest request, CancellationToken ct = default);

    [Delete(Momentum.Shared.Routes.Companies.Delete)]
    Task<IApiResponse> DeleteCompany(Guid id, CancellationToken ct = default);
}
