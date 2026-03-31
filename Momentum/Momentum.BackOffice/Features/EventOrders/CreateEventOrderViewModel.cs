namespace Momentum.BackOffice.Features.EventOrders;

public class CreateEventOrderViewModel
{
    // Step 1: Person Selection
    public Guid SelectedPersonId { get; set; }
    public List<PersonOptionViewModel> AvailablePersons { get; set; } = [];

    // Step 2: Event Data
    public int EventType { get; set; }
    public string? EventName { get; set; }
    public string? EventDate { get; set; }

    // Step 3: Product Selection
    public List<ProductSelectionViewModel> AvailableProducts { get; set; } = [];
    public List<SelectedProductViewModel> SelectedProducts { get; set; } = [];

    // Summary
    public decimal TotalPrice => SelectedProducts.Sum(p => p.TotalPrice);
}

public class PersonOptionViewModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string CNP { get; set; } = string.Empty;
}

public class ProductSelectionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Type { get; set; }
    public string TypeName { get; set; } = string.Empty;
}

public class SelectedProductViewModel
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public decimal Quantity { get; set; } = 1;
    public decimal TotalPrice => UnitPrice * Quantity;
}
