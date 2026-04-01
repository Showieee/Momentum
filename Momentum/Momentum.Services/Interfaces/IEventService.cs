using Momentum.Services.Models;

namespace Momentum.Services.Interfaces;

public interface IEventService
{
    Task<List<EventModel>> Get();

    Task<EventModel?> GetById(Guid id);

    Task<Guid> Insert(EventModel eventModel);

    Task Update(Guid id, EventModel eventModel);
    
    Task Delete(Guid id);
}
