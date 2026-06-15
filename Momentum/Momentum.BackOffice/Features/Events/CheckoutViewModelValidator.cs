using FluentValidation;

namespace Momentum.BackOffice.Features.Events;

public class CheckoutViewModelValidator : AbstractValidator<CheckoutViewModel>
{
    public CheckoutViewModelValidator()
    {
        RuleFor(c => c.CardholderName)
            .NotEmpty().WithMessage("Cardholder name is required")
            .MaximumLength(100).WithMessage("Cardholder name cannot exceed 100 characters");

        RuleFor(c => c.CardNumber)
            .NotEmpty().WithMessage("Card number is required")
            .Must(BeAValidCardNumber).WithMessage("Card number must be 13 to 19 digits");

        RuleFor(c => c.Expiry)
            .NotEmpty().WithMessage("Expiry date is required")
            .Matches(@"^(0[1-9]|1[0-2])\/\d{2}$").WithMessage("Expiry must be in MM/YY format");

        RuleFor(c => c.Cvc)
            .NotEmpty().WithMessage("CVC is required")
            .Matches(@"^\d{3,4}$").WithMessage("CVC must be 3 or 4 digits");
    }

    private static bool BeAValidCardNumber(string? cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
            return false;

        var digits = cardNumber.Replace(" ", string.Empty);
        return digits.Length is >= 13 and <= 19 && digits.All(char.IsDigit);
    }
}
