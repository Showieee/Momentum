using Refit;

namespace Momentum.BackOffice.Services;

public interface IEventOrderService
{
    [Get(Momentum.Shared.Routes.EventOrders.Get)]
    Task<IApiResponse<List<EventOrderResponse>>> GetEventOrders(CancellationToken ct = default);

    [Get(Momentum.Shared.Routes.EventOrders.GetById)]
    Task<IApiResponse<EventOrderResponse>> GetEventOrderById(Guid id, CancellationToken ct = default);

    [Post(Momentum.Shared.Routes.EventOrders.Create)]
    Task<IApiResponse<EventOrderResponse>> CreateEventOrder([Body] CreateEventOrderRequest request, CancellationToken ct = default);

    [Put(Momentum.Shared.Routes.EventOrders.Update)]
    Task<IApiResponse> UpdateEventOrder(Guid id, [Body] CreateEventOrderRequest request, CancellationToken ct = default);

    [Post(Momentum.Shared.Routes.EventOrders.Pay)]
    Task<IApiResponse> PayEventOrder(Guid id, CancellationToken ct = default);

    [Delete(Momentum.Shared.Routes.EventOrders.Delete)]
    Task<IApiResponse> DeleteEventOrder(Guid id, CancellationToken ct = default);
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

public class EventOrderResponse
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public PersonDetail? Person { get; set; }
    
    public Guid EventId { get; set; }
    public EventDetail? Event { get; set; }
    
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
    public ProductDetail? Product { get; set; }
    
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int NumberOfPeople { get; set; } = 1;
    public int NumberOfHours { get; set; } = 1;
    public decimal TotalPrice { get; set; }
}

public class PersonDetail
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string CNP { get; set; } = string.Empty;
}

public class EventDetail
{
    public Guid Id { get; set; }
    public int Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}

public class ProductDetail
{
    public Guid Id { get; set; }
    public int Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsPerPerson { get; set; }
    public bool IsHourly { get; set; }
}
