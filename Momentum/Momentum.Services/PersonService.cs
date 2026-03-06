using Microsoft.EntityFrameworkCore;
using Momentum.Domain.Entities;
using Momentum.Persistance;
using Momentum.Services.Extensions;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;

namespace Momentum.Services;

public class PersonService : IPersonService
{
    public MomentumDbContext DbContext { get; set; }

    public PersonService(MomentumDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<List<PersonModel>> Get()
    {
        var result = await DbContext.Set<PersonEntity>().AsNoTracking().Include(p=>p.Address).ToListAsync();
        return result.Select(a => a.ToModel()).ToList();
    }

    public async Task<PersonModel?> GetById(Guid id)
    {
        var result = await DbContext.Set<PersonEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return result?.ToModel();
    }

    public async Task Insert(PersonModel person)
    {
        var entity = person.ToEntity();
        DbContext.Persons.Add(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task Update(Guid id, PersonModel person)
    {
        var entity = await DbContext.Set<PersonEntity>().Include(c => c.Address).Where(x => x.Id == id).FirstOrDefaultAsync();
        if (entity == null)
        {
            return;
        }

        if (entity.Address == null)
        {
            var address = person.Address?.ToEntity();
            if (address != null)
            {
                DbContext.Add(address);
                entity.Address = address;
            }
        }
        else
        {
            person.Address?.MapToEntity(entity.Address);
        }
        entity.FirstName = person.FirstName;
        entity.LastName = person.LastName;
        entity.CNP = person.CNP;

        await DbContext.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var entity = await DbContext.Set<PersonEntity>().Include(c => c.Address).Where(x => x.Id == id).FirstOrDefaultAsync();
        if (entity != null)
        {
            DbContext.Remove(entity);
            if (entity.Address != null)
            {
                DbContext.Remove(entity.Address);
            }
        }
        await DbContext.SaveChangesAsync();
    }
}
