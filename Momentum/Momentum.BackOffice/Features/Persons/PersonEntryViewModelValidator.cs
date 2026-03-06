using FluentValidation;

namespace Momentum.BackOffice.Features.Persons;

public class PersonEntryViewModelValidatorValidator : AbstractValidator<PersonEntryViewModel>
{
	public PersonEntryViewModelValidatorValidator()
	{
		RuleFor(x => x.FirstName)
			.NotEmpty().WithMessage("First name is required.")
			.Matches("^[a-zA-Z]*$").WithMessage("Only letter characters are allowed.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .Matches("^[a-zA-Z]*$").WithMessage("Only letter characters are allowed.");
    }
}
