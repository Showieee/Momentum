using Microsoft.EntityFrameworkCore;
using Momentum.Domain.Entities;
using Momentum.Persistance;
using Momentum.Services.Extensions;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;
using System.Net;

namespace Momentum.Services;

public class AddressService : IAddressService
{
    public MomentumDbContext DbContext { get; set; }

    public AddressService(MomentumDbContext dbContext)
    {
        DbContext = dbContext;
    }
    public async Task<AddressModel?> GetById(Guid id)
    {
        var result = await DbContext.Set<AddressEntity>().AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();
        return result?.ToModel();
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

    public async Task Update(Guid id, AddressModel address)
    {
        var entity = await DbContext.Set<AddressEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
        if (entity == null)
        {
            return;
        }

        entity.State = address.State;
        entity.StreetNumber = address.StreetNumber;
        entity.StreetName = address.StreetName;
        entity.PhoneNumber = address.PhoneNumber;
        entity.City = address.City;
        entity.Country = address.Country;
        entity.Email = address.Email;

        await DbContext.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var entity = await DbContext.Set<AddressEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
        if (entity != null) 
        { 
            DbContext.Remove(entity);
        }
        await DbContext.SaveChangesAsync();
    }
}
