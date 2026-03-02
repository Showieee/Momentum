using FluentValidation;
using Momentum.BackOffice.Properties;

namespace Momentum.BackOffice.Features.Options;

public class OptionsViewModelValidator : AbstractValidator<OptionsViewModel>
{
	public OptionsViewModelValidator()
	{
		RuleFor(m => m.PricePerHour)
			.NotNull()
			.GreaterThan(0)
			.WithName(Resources.pr_OptionsViewModel_PricePerHour);

		RuleFor(m => m.MinimumMinutesToCharge)
			.NotNull()
			.GreaterThanOrEqualTo(0)
			.LessThanOrEqualTo(120)
			.WithName(Resources.pr_OptionsViewModel_MinimumMinutesToCharge);

		RuleFor(m => m.BillingPeriodMinutes)
			.NotNull()
			.GreaterThanOrEqualTo(0)
			.LessThanOrEqualTo(120)
			.WithName(Resources.pr_OptionsViewModel_BillingPeriodMinutes);

		RuleFor(m => m.FreeMinutes)
			.NotNull()
			.GreaterThanOrEqualTo(0)
			.LessThanOrEqualTo(120)
			.WithName(Resources.pr_OptionsViewModel_FreeMinutes);

		RuleFor(m => m.MinutesToExit)
			.NotNull()
			.GreaterThan(0)
			.LessThanOrEqualTo(120)
			.WithName(Resources.pr_OptionsViewModel_MinutesToExit);

		RuleFor(m => m.ParkingCapacity)
			.NotNull()
			.GreaterThanOrEqualTo(0)
			.WithName(Resources.pr_OptionsViewModel_ParkingCapacity);

		RuleFor(m => m.PriceRoundDecimals)
			.NotNull()
			.GreaterThanOrEqualTo(0)
			.LessThanOrEqualTo(4)
			.WithName(Resources.pr_OptionsViewModel_PriceRoundDecimals);

		RuleFor(m => m.DateToleranceSeconds)
			.NotNull()
			.GreaterThanOrEqualTo(0)
			.LessThanOrEqualTo(60)
			.WithName(Resources.pr_OptionsViewModel_DateToleranceSeconds);
		
		RuleFor(m => m.TestDevicesCode)
			.MaximumLength(9)
			.WithName(Resources.pr_OptionsViewModel_TestDevicesCode);
	}
}