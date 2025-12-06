using Microsoft.EntityFrameworkCore;
using Momentum.Domain.Entities;
using Momentum.Persistance;
using Momentum.Services.Extensions;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;

namespace Momentum.Services;

public class AddressService : IAddressService
{
	public MomentumDbContext DbContext { get; set; }

	public AddressService(MomentumDbContext dbContext)
	{
		DbContext = dbContext;
	}
	public async Task<AddressModel> GetById(Guid id)
	{
		throw new NotImplementedException();
	}

	public async Task<List<AddressModel>> Get()
	{
		var result = await DbContext.Set<AddressEntity>().AsNoTracking().ToListAsync();
		return result.Select(a => a.ToModel()).ToList();
	}

	public async Task Insert(AddressModel address)
	{
		var entity = address.ToEntity();

		DbContext.Add(entity);
		await DbContext.SaveChangesAsync();
	}

	public Task Update(Guid id, AddressModel address)
	{
		throw new NotImplementedException();
	}

	public Task Delete(Guid id)
	{
		throw new NotImplementedException();
	}
}
