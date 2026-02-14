using Momentum.Domain.Enums;

namespace Momentum.Domain.Entities;

public class EventEntity
{
    public Guid Id { get; set; }
    public EventType Type { get; set; }
    public required string Name { get; set; }
    public required string Date { get; set; }
    public virtual List<ProductEntity>? Products { get; set; } = [];
}
