namespace Momentum.Services.Models;

public class EventOrderModel
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public PersonOrderModel? Person { get; set; }

    public Guid EventId { get; set; }
    public EventModel? Event { get; set; }

    public List<EventOrderProductModel> Products { get; set; } = [];

    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public decimal TotalPrice => Products.Sum(p => p.TotalPrice);
}

public class EventOrderProductModel
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public ProductModel? Product { get; set; }

    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public int NumberOfPeople { get; set; } = 1;
    public int NumberOfHours { get; set; } = 1;

    public decimal TotalPrice => UnitPrice * Quantity * NumberOfPeople * NumberOfHours;
}

public class CreateEventOrderRequest
{
    public Guid PersonId { get; set; }
    public Guid EventId { get; set; }
    public List<EventOrderProductRequest> Products { get; set; } = [];
}

public class EventOrderProductRequest
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; } = 1;
    public int NumberOfPeople { get; set; } = 1;
    public int NumberOfHours { get; set; } = 1;
}

public class PersonOrderModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string CNP { get; set; } = string.Empty;
}
