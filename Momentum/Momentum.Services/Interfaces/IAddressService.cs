using Momentum.Services.Models;

namespace Momentum.Services.Interfaces;

public interface IAddressService
{
	Task<List<AddressModel>> Get();

	Task<AddressModel?> GetById(Guid id);

	Task Insert(AddressModel address);

	Task Update(Guid id, AddressModel address);

	Task Delete(Guid id);
}
