using Application.CheckIn.CheckIn;
using FluentValidation;

namespace Application.DailyUser;

public class DailyUserCommandValidator : AbstractValidator<DailyUserCreateCommand>
{
    public DailyUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required"); ;

        RuleFor(x => x.LastName)
           .NotEmpty().WithMessage("Last name is required");

        RuleFor(x => x.DateOfBirth)
           .NotEmpty().WithMessage("Date is required")
           .Must(BeAValidDate).WithMessage("Invalid date format");
    }

    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }
}