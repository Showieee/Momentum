namespace Momentum.BackOffice.Features.Options;

public class OptionsViewModel
{
	public virtual Guid Id { get; set; }
	public virtual decimal? PricePerHour { get; set; }
	public virtual int? MinimumMinutesToCharge { get; set; }
	public virtual int? BillingPeriodMinutes { get; set; }
	public virtual int? FreeMinutes { get; set; }
	public virtual int? MinutesToExit { get; set; }
	public virtual int? ParkingCapacity { get; set; }
	public virtual int? PriceRoundDecimals { get; set; }
	public virtual int? DateToleranceSeconds { get; set; }
	public virtual DateTime CreatedDate { get; set; }
	public virtual DateTime UpdateDate { get; set; }
	public virtual string? ExitCode { get; set; }
	public virtual string? TestDevicesCode { get; set; }
}