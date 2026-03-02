namespace Momentum.BackOffice.Services.Models;


public sealed record GetWhitelistResponse
{
	public Guid Id { get; set; }
	public string? PlateNumber { get; set; } = null!;
	public int ExitsLeft { get; set; }
}