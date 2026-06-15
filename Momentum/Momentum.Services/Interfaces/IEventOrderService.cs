using Momentum.Services.Models;

namespace Momentum.Services.Interfaces;

public interface IEventOrderService
{
    Task<List<EventOrderModel>> Get();
    Task<EventOrderModel?> GetById(Guid id);
    Task<EventOrderModel> Create(CreateEventOrderRequest request);
    Task Update(Guid id, CreateEventOrderRequest request);
    Task MarkAsPaid(Guid id);
    Task Delete(Guid id);
}
