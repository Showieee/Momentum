namespace Momentum.API.Models;

public class InsertPersonRequest
{
    public InsertAddressRequest? Address { get; set; } = null;
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string CNP { get; set; }
}
