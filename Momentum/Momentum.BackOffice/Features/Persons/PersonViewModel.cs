namespace Momentum.BackOffice.Features.Persons;

public class PersonViewModel
{
    public virtual Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public AddressViewModel? Address { get; set; } = null;
    public string CNP { get; set; } = string.Empty;
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