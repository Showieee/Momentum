using Momentum.Shared.Models.Requests;
using Momentum.Shared.Models.Responses;
using Refit;

namespace Momentum.BackOffice.Services;

public interface IPersonService
{
    [Get(Momentum.Shared.Routes.Persons.Get)]
    Task<IApiResponse<List<GetPersonsResponse>>> GetPersons(CancellationToken ct = default);

    [Post(Momentum.Shared.Routes.Persons.Insert)]
    Task<IApiResponse> InsertPerson([Body] InsertPersonRequest request, CancellationToken ct = default);

    [Put(Momentum.Shared.Routes.Persons.Update)]
    Task<IApiResponse> UpdatePerson(Guid id, [Body] UpdatePersonRequest request, CancellationToken ct = default);

    [Delete(Momentum.Shared.Routes.Persons.Delete)]
    Task<IApiResponse> DeletePerson(Guid id, CancellationToken ct = default);
}