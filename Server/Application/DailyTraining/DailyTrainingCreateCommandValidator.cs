using Application.CheckIn.CheckIn;
using FluentValidation;

namespace Application.DailyTraining;

public class DailyTrainingCommandValidator : AbstractValidator<DailyTrainingCreateCommand>
{
    public DailyTrainingCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required"); ;

        RuleFor(x => x.LastName)
           .NotEmpty().WithMessage("Last name is required");

        RuleFor(x => x.DateOfBirth)
                    .NotEmpty().WithMessage("Date is required");
    }

    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }
}