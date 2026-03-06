using FluentValidation;
using Microsoft.IdentityModel.Tokens;
using Momentum.BackOffice.Shared.Models;

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

		RuleFor(x => x.CNP)
			.NotEmpty().WithMessage("CNP is required.");

		RuleFor(x => x.Address!.StreetName)
			.NotEmpty().WithMessage("Street name is required.")
			.When(x => x.Address != null && !IsEmptyAddress(x.Address));

		RuleFor(x => x.Address!.StreetNumber)
			.NotEmpty().WithMessage("Street number is required.")
			.When(x => x.Address != null && !IsEmptyAddress(x.Address));

		RuleFor(x => x.Address!.City)
			.NotEmpty().WithMessage("City is required.")
			.When(x => x.Address != null && !IsEmptyAddress(x.Address));

		RuleFor(x => x.Address!.State)
			.NotEmpty().WithMessage("State is required.")
			.When(x => x.Address != null && !IsEmptyAddress(x.Address));

		RuleFor(x => x.Address!.Country)
			.NotEmpty().WithMessage("Country is required.")
			.When(x => x.Address != null && !IsEmptyAddress(x.Address));

		RuleFor(x => x.Address!.Email)
			.NotEmpty().WithMessage("Email is required.")
			.EmailAddress().WithMessage("Invalid email address.")
			.When(x => x.Address != null && !IsEmptyAddress(x.Address));

	}

	private static bool IsEmptyAddress(AddressEntryViewModel address)
	{
		return address.StreetName.IsNullOrEmpty() &&
			   address.StreetNumber.IsNullOrEmpty() &&
			   address.City.IsNullOrEmpty() &&
			   address.State.IsNullOrEmpty() &&
			   address.Country.IsNullOrEmpty() &&
			   address.Email.IsNullOrEmpty();
	}
}

