namespace Momentum.BackOffice.Features.Events;

public class EventViewModel
{
    public Guid Id { get; set; }
    public int Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public Guid? SelectedLocationId { get; set; }
    public string? LocationName { get; set; }
    public decimal LocationPrice { get; set; }
    public List<EventProductDetailViewModel> SelectedProducts { get; set; } = [];
    public decimal TotalPrice { get; set; }
}

public class EventProductDetailViewModel
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public sealed class EventEntryViewModel
{
    public int Type { get; set; }
    public string? Name { get; set; }
    public string? Date { get; set; }
    public Guid? SelectedLocationId { get; set; }
    public List<Guid> SelectedProductIds { get; set; } = [];
}

public class LocationOptionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}

public class ProductOptionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Type { get; set; }
}
