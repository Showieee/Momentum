namespace Momentum.Shared.Models.Responses;

public sealed record GetOptionsResponse
{
	public Guid Id { get; set; }
	public decimal PricePerHour { get; set; }
	public int MinimumMinutesToCharge { get; set; }
	public int BillingPeriodMinutes { get; set; }
	public int FreeMinutes { get; set; }
	public int MinutesToExit { get; set; }
	public int ParkingCapacity { get; set; }
	public int PriceRoundDecimals { get; set; }
	public int DateToleranceSeconds { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? UpdateDate { get; set; }
	public string? ExitCode { get; set; }
	public string? TestDevicesCode { get; set; }
}