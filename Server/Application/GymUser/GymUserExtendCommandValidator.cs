﻿using Application.GymUser;
using FluentValidation;

namespace Application.GymUser
{
    public class GymUserExtendCommandValidator : AbstractValidator<GymUserExtendCommand>
    {
        public GymUserExtendCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id je obavezan")
                .Must(BeAValidGuid).WithMessage("Nevalidan UUID");
            RuleFor(x => x.Data.Type)
                .NotEmpty().WithMessage("Tip is obavezno polje")
                .NotNull().WithMessage("Tip is obavezno polje")
                .IsInEnum().WithMessage("Tip mora imati korektnu vrijednost");
        }

        private bool BeAValidGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }
    }
}
