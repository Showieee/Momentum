using Momentum.Services.Models;

namespace Momentum.Services.Interfaces;

public interface IPersonService
{
    Task<List<PersonModel>> Get();

    Task<PersonModel?> GetById(Guid id);

    Task Insert(PersonModel person);

    Task Update(Guid id, PersonModel person);

    Task Delete(Guid id);
}
