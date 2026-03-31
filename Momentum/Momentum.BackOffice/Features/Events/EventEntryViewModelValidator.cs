using FluentValidation;

namespace Momentum.BackOffice.Features.Events;

public class EventEntryViewModelValidator : AbstractValidator<EventEntryViewModel>
{
    public EventEntryViewModelValidator()
    {
        RuleFor(e => e.Name)
            .NotEmpty().WithMessage("Event name is required")
            .MaximumLength(100).WithMessage("Event name cannot exceed 100 characters");

        RuleFor(e => e.Type)
            .NotEmpty().WithMessage("Event type is required")
            .InclusiveBetween(1, int.MaxValue).WithMessage("Event type must be valid");

        RuleFor(e => e.Date)
            .NotEmpty().WithMessage("Event date is required")
            .Matches(@"^\d{4}-\d{2}-\d{2}$").WithMessage("Date must be in YYYY-MM-DD format");
    }
}
