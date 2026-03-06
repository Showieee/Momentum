using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Momentum.BackOffice.Features.Companies;

public class CompanyEntryViewModelValidator : AbstractValidator<CompanyEntryViewModel>
{
	public CompanyEntryViewModelValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Company name is required.");

		RuleFor(x => x.CUI)
			.NotEmpty().WithMessage("CUI is required.");

		RuleFor(x => x.Address)
			.Custom(ValidateAddress).When(x => x.Address != null);
	}

	/// <summary>
	/// Checks if ALL address fields are empty (null or whitespace).
	/// Returns true only if every field is empty, meaning no validation is needed.
	/// Returns false if at least one field has a value, triggering full validation.
	/// </summary>
	private void ValidateAddress(AddressEntryViewModel? address, ValidationContext<CompanyEntryViewModel> context)
	{
		if (address == null || IsEmptyAddress(address))
			return;

		if (address.StreetName.IsNullOrEmpty())
			context.AddFailure(nameof(address.StreetName), "Street name is required.");

		if (address.StreetNumber.IsNullOrEmpty())
			context.AddFailure(nameof(address.StreetNumber), "Street number is required.");

		if (address.City.IsNullOrEmpty())
			context.AddFailure(nameof(address.City), "City is required.");

		if (address.State.IsNullOrEmpty())
			context.AddFailure(nameof(address.State), "State is required.");

		if (address.Country.IsNullOrEmpty())
			context.AddFailure(nameof(address.Country), "Country is required.");

		if (address.Email.IsNullOrEmpty())
			context.AddFailure(nameof(address.Email), "Email is required.");
		else if (!IsValidEmail(address.Email))
			context.AddFailure(nameof(address.Email), "Invalid email address.");
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

	private static bool IsValidEmail(string email)
	{
		try
		{
			var addr = new System.Net.Mail.MailAddress(email);
			return addr.Address == email;
		}
		catch
		{
			return false;
		}
	}
}
