namespace Momentum.Shared.Providers;

public class DateTimeProvider : IDateTimeProvider
{
	public DateTime Now => DateTime.Now;
	public DateTime UtcNow => DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
}