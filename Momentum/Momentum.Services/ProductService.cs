using Microsoft.EntityFrameworkCore;
using Momentum.Domain.Entities;
using Momentum.Persistance;
using Momentum.Services.Extensions;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;
using System.Net;

namespace Momentum.Services;

public class ProductService : IProductService
{
    public MomentumDbContext DbContext { get; set; }

    public ProductService(MomentumDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task Delete(Guid id)
    {
        var entity = await DbContext.Set<ProductEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
        if (entity != null)
        {
            DbContext.Remove(entity);
        }
        await DbContext.SaveChangesAsync();
    }

    public async Task<List<ProductModel>> Get()
    {
        var result = await DbContext.Set<ProductEntity>().Include(c => c.Company).AsNoTracking().ToListAsync();
        return result.Select(a => a.ToModel()).ToList();
    }

    public async Task<ProductModel?> GetById(Guid id)
    {
        var result = await DbContext.Set<ProductEntity>().AsNoTracking().Include(c => c.Company).Where(x => x.Id == id).FirstOrDefaultAsync();
        return result?.ToModel();
    }

    public async Task Insert(ProductModel product)
    {
        var companyEntity = await DbContext.Set<CompanyEntity>()
            .FirstOrDefaultAsync(x => x.Id == product.CompanyId);

        if (companyEntity == null)
        {
            throw new HttpRequestException($"Company with id {product.CompanyId} not found", null, HttpStatusCode.NotFound);
        }

        var entity = product.ToEntity(companyEntity);

        DbContext.Add(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task Update(Guid id, ProductModel product)
    {
        var entity = await DbContext.Set<ProductEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
        if (entity == null)
        {
            return;
        }

        product.MapToEntity(entity);

        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteById(Guid id)
    {
        var entity = await DbContext.Set<ProductEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
        if (entity != null)
        {
            DbContext.Remove(entity);
        }
        await DbContext.SaveChangesAsync();
    }
}
