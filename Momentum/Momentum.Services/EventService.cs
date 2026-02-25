using Microsoft.EntityFrameworkCore;
using Momentum.Domain.Entities;
using Momentum.Persistance;
using Momentum.Services.Extensions;
using Momentum.Services.Interfaces;
using Momentum.Services.Models;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Momentum.Services;

public class EventService : IEventService
{
    public MomentumDbContext DbContext { get; set; }

    public EventService(MomentumDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<List<EventModel>> Get()
    {
        var result = await DbContext.Set<EventEntity>().Include(c => c.Products).AsNoTracking().ToListAsync();
        return result.Select(a => a.ToModel()).ToList();
    }

    public async Task<EventModel?> GetById(Guid id)
    {
        var result = await DbContext.Set<EventEntity>().AsNoTracking().Include(c => c.Products).Where(x => x.Id == id).FirstOrDefaultAsync();
        return result?.ToModel();
    }

    public async Task Insert(EventModel eventModel)
    {
        var entity = eventModel.ToEntity();

        DbContext.Add(entity);

        await DbContext.SaveChangesAsync();
    }

    public async Task Update(Guid id, EventModel eventModel)
    {
        var entity = await DbContext.Set<EventEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
        if (entity == null)
        {
            return;
        }

        eventModel.MapToEntity(entity);

        await DbContext.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var entity = await DbContext.Set<EventEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
        if (entity != null)
        {
            DbContext.Remove(entity);
        }
        await DbContext.SaveChangesAsync();
    }
}
