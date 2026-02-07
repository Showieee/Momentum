namespace Momentum.Services.Models;
public class PersonModel
{
	public Guid Id { get; set; }
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
    public AddressModel? Address { get; set; } = null;
	public required string CNP { get; set; }
}
