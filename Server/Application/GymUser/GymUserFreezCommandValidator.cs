﻿using Application.GymUser;
using FluentValidation;

namespace Application.GymUser
{
    public class GymUserFreezCommandValidator : AbstractValidator<GymUserFreezCommand>
    {
        public GymUserFreezCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id je obavezan")
                .Must(BeAValidGuid).WithMessage("Nevalidan UUID");
        }

        private bool BeAValidGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }
    }
}
