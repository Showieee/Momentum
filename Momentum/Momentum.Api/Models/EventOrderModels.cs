namespace Momentum.API.Models;

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

public class EventOrderResponse
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public PersonResponse? Person { get; set; }
    
    public Guid EventId { get; set; }
    public EventResponse? Event { get; set; }
    
    public List<EventOrderProductResponse> Products { get; set; } = [];
    
    public bool IsPaid { get; set; }
    public DateTime? PaidAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public decimal TotalPrice { get; set; }
}

public class EventOrderProductResponse
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public ProductResponse? Product { get; set; }
    
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int NumberOfPeople { get; set; } = 1;
    public int NumberOfHours { get; set; } = 1;
    public decimal TotalPrice { get; set; }
}

public class PersonResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string CNP { get; set; } = string.Empty;
}

public class EventResponse
{
    public Guid Id { get; set; }
    public int Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}

public class ProductResponse
{
    public Guid Id { get; set; }
    public int Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsPerPerson { get; set; }
    public bool IsHourly { get; set; }
}
