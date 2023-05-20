using Application.CheckIn.CheckIn;
using FluentValidation;

namespace Application.CheckIn;

public class CheckInCommandValidator : AbstractValidator<CheckInCommand>
{
    public CheckInCommandValidator()
    {
        RuleFor(x => x.GymUserId)
           .NotEmpty().WithMessage("Id je obavezan")
           .Must(BeAValidGuid).WithMessage("Nevalidan UUID");
    }

    private bool BeAValidGuid(Guid guid)
    {
        return guid != Guid.Empty;
    }
}