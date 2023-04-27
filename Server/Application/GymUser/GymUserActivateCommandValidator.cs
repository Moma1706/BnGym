using Application.GymUser;
using FluentValidation;

namespace Application.GymUser
{
    public class GymUserActivateCommandValidator : AbstractValidator<GymUserActivateCommand>
    {
        public GymUserActivateCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .Must(BeAValidGuid).WithMessage("Invalid UUID");
        }

        private bool BeAValidGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }
    }
}
