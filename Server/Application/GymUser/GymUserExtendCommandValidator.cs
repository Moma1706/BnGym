using Application.GymUser;
using FluentValidation;

namespace Application.Auth.Login
{
    public class GymUserExtendCommandValidator : AbstractValidator<GymUserExtendCommand>
    {
        public GymUserExtendCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .Must(BeAValidGuid).WithMessage("Invalid UUID");
            RuleFor(x => x.Data)
                .NotEmpty().WithMessage("Type is required")
                .NotNull();
        }

        private bool BeAValidGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }
    }
}
