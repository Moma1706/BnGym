using Application.GymUser;
using FluentValidation;

namespace Application.GymUser
{
    public class GymUserGetOneCommandValidator : AbstractValidator<GymUserGetOneCommand>
    {
        public GymUserGetOneCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
