using System;
using FluentValidation;

namespace Application.GymUser
{
	public class GymUserDeleteCommandValidator : AbstractValidator<GymUserDeleteCommand>
    {
        public GymUserDeleteCommandValidator()
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

