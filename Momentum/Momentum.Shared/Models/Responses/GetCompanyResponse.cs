namespace Momentum.Shared.Models.Responses;

public class GetCompanyResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CUI { get; set; } = string.Empty;
    public AddressResponse? Address { get; set; }
}

public class AddressResponse
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
