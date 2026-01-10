using Microsoft.EntityFrameworkCore;
using Momentum.Domain.Entities;
using Momentum.Persistance;
using Momentum.Services.Extensions;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;
using System.Net;

namespace Momentum.Services;

public class CompanyService : ICompanyService
{
    public MomentumDbContext DbContext { get; set; }

    public CompanyService(MomentumDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task Delete(Guid id)
    {
        var entity = await DbContext.Set<CompanyEntity>().Include(c => c.Address).Where(x => x.Id == id).FirstOrDefaultAsync();
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

    public async Task<List<CompanyModel>> Get()
    {
        var result = await DbContext.Set<CompanyEntity>().Include(c => c.Address).AsNoTracking().ToListAsync();
        return result.Select(a => a.ToModel()).ToList();
    }

    public async Task<CompanyModel?> GetById(Guid id)
    {
        var result = await DbContext.Set<CompanyEntity>().AsNoTracking().Include(c => c.Address).Where(x => x.Id == id).FirstOrDefaultAsync();
        return result?.ToModel();
    }

    public async Task Insert(CompanyModel company)
    {
        var entity = company.ToEntity();

        DbContext.Add(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task Update(Guid id, CompanyModel company)
    {
        var entity = await DbContext.Set<CompanyEntity>().Include(c => c.Address).Where(x => x.Id == id).FirstOrDefaultAsync();
        if (entity == null)
        {
            return;
        }

        if (entity.Address == null)
        {
            var address = company.Address?.ToEntity();
            if (address != null)
            {
                DbContext.Add(address);
                entity.Address = address;
            }
        }
        else
        {
            company.Address?.MapToEntity(entity.Address);
        }
        entity.CUI = company.CUI;
        entity.Name = company.Name;

        await DbContext.SaveChangesAsync();
    }
}
