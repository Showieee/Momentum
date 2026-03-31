using FluentValidation;

namespace Momentum.BackOffice.Features.Products;

public class ProductEntryViewModelValidator : AbstractValidator<ProductEntryViewModel>
{
    public ProductEntryViewModelValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Product name is required")
            .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters");

        RuleFor(p => p.Type)
            .NotEmpty().WithMessage("Product type is required")
            .InclusiveBetween(1, int.MaxValue).WithMessage("Product type must be valid");

        RuleFor(p => p.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0");

        RuleFor(p => p.CompanyId)
            .NotEmpty().WithMessage("Company is required");
    }
}
