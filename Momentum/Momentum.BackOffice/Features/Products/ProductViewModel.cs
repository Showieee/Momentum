namespace Momentum.BackOffice.Features.Products;

public class ProductViewModel
{
    public Guid Id { get; set; }
    public int Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsPerPerson { get; set; }
    public bool IsHourly { get; set; }
    public Guid CompanyId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
}

public sealed class ProductEntryViewModel
{
    public int Type { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsPerPerson { get; set; }
    public bool IsHourly { get; set; }
    public Guid CompanyId { get; set; }
}
