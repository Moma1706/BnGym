using Application.GymUser;
using FluentValidation;

namespace Application.Auth.Login
{
    public class GymUserGetOneCommandValidator : AbstractValidator<GymUserGetOneCommand>
    {
        public GymUserGetOneCommandValidator()
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
