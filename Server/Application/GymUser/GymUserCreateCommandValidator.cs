using System;
using System.Numerics;
using FluentValidation;

namespace Application.GymUser;

public class GymUserCreateCommandValidator : AbstractValidator<GymUserCreateCommand>
{
    public GymUserCreateCommandValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email je obavezno polje")
            .EmailAddress().WithMessage("Email nije u korektnom formatu");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ime je obavezno polje");

        RuleFor(x => x.LastName)
           .NotEmpty().WithMessage("Prezime je obavezno polje");

        RuleFor(x => x.Type)
            .NotNull().WithMessage("Tip je obavezno polje")
            .IsInEnum().WithMessage("Tip mora imati korektnu vrijednost");
    }
}