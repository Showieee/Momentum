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
    public bool LocationIsPerPerson { get; set; }
    public bool LocationIsHourly { get; set; }
    public int LocationPeople { get; set; } = 1;
    public int LocationHours { get; set; } = 1;
    public List<EventProductDetailViewModel> SelectedProducts { get; set; } = [];
    public decimal TotalPrice { get; set; }
    public Guid? EventOrderId { get; set; }
    public bool IsPaid { get; set; }

    public decimal LocationLineTotal => LocationPrice
        * (LocationIsPerPerson ? Math.Max(1, LocationPeople) : 1)
        * (LocationIsHourly ? Math.Max(1, LocationHours) : 1);
}

public class EventProductDetailViewModel
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsPerPerson { get; set; }
    public bool IsHourly { get; set; }
    public int NumberOfPeople { get; set; } = 1;
    public int NumberOfHours { get; set; } = 1;

    public decimal LineTotal => Price
        * (IsPerPerson ? Math.Max(1, NumberOfPeople) : 1)
        * (IsHourly ? Math.Max(1, NumberOfHours) : 1);
}

public sealed class EventEntryViewModel
{
    public int Type { get; set; }
    public string? Name { get; set; }
    public string? Date { get; set; }
    public Guid? SelectedLocationId { get; set; }
    public List<Guid> SelectedProductIds { get; set; } = [];
}

public sealed class CheckoutViewModel
{
    public string? CardholderName { get; set; }
    public string? CardNumber { get; set; }
    public string? Expiry { get; set; }
    public string? Cvc { get; set; }
}

public class LocationOptionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsPerPerson { get; set; }
    public bool IsHourly { get; set; }
}

public class ProductOptionViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Type { get; set; }
    public bool IsPerPerson { get; set; }
    public bool IsHourly { get; set; }
    public int NumberOfPeople { get; set; } = 1;
    public int NumberOfHours { get; set; } = 1;
}
