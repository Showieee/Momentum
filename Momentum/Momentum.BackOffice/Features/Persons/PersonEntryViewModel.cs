namespace Momentum.BackOffice.Features.Persons;

public sealed class PersonEntryViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public AddressEntryViewModel? Address { get; set; } = new();
    public string? CNP { get; set; }
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
