namespace Momentum.Domain.Entities;

public class PersonEntity
{
	public Guid Id { get; set; }
    public Guid? AddressId { get; set; }
    public AddressEntity? Address { get; set; } = null;
	public required string FirstName { get; set; }
	public required string LastName { get; set; }
	public required string CNP { get; set; }
}
