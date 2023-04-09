﻿using Application.GymUser;
using FluentValidation;

namespace Application.Auth.Login
{
    public class GymUserFreezCommandValidator : AbstractValidator<GymUserFreezCommand>
    {
        public GymUserFreezCommandValidator()
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