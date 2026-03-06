using Momentum.BackOffice.Shared.Models;

namespace Momentum.BackOffice.Features.Persons;

public sealed class PersonEntryViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public AddressEntryViewModel? Address { get; set; } = new();
    public string? CNP { get; set; }
}

