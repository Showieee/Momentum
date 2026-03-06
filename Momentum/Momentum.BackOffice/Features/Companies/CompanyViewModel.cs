namespace Momentum.BackOffice.Features.Companies;

public class CompanyViewModel
{
    public virtual Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CUI { get; set; } = string.Empty;
    public AddressViewModel? Address { get; set; }
}

public class AddressViewModel
{
    public Guid Id { get; set; }
    public string StreetName { get; set; } = string.Empty;
    public string StreetNumber { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Email { get; set; } = string.Empty;
}

public sealed class CompanyEntryViewModel
{
    public string? Name { get; set; }
    public string? CUI { get; set; }
    public AddressEntryViewModel? Address { get; set; } = new();
}

public sealed class AddressEntryViewModel
{
    public string? StreetName { get; set; }
    public string? StreetNumber { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}
