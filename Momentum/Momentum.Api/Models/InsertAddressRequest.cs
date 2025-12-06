namespace Momentum.API.Models;

public class InsertAddressRequest
{
	public required string StreetName { get; set; }
	public required string StreetNumber { get; set; }
	public required string City { get; set; }
	public required string State { get; set; }
	public required string Country { get; set; }
	public string? PhoneNumber { get; set; }
	public required string Email { get; set; }
}
