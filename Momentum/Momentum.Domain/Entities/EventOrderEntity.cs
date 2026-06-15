namespace Momentum.Domain.Entities;

public class EventOrderEntity
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public virtual PersonEntity? Person { get; set; }

    public Guid EventId { get; set; }
    public virtual EventEntity? Event { get; set; }

    public virtual List<EventOrderProductEntity> Products { get; set; } = [];

    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public class EventOrderProductEntity
{
    public Guid Id { get; set; }
    public Guid EventOrderId { get; set; }
    public virtual EventOrderEntity? EventOrder { get; set; }

    public Guid ProductId { get; set; }
    public virtual ProductEntity? Product { get; set; }

    public decimal Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }

    public int NumberOfPeople { get; set; } = 1;
    public int NumberOfHours { get; set; } = 1;
}
