using System;
using FluentValidation;

namespace Application.App
{
    public class RegularUserUpdateCommandValidator : AbstractValidator<RegularUserUpdateCommand>
    {
        public RegularUserUpdateCommandValidator()
        {
            RuleFor(x => x.Data.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email je obavezno polje")
                .EmailAddress().WithMessage("Email nije u korektnom formatu");

            RuleFor(x => x.Data.FirstName)
                .NotEmpty().WithMessage("Ime je obavezno polje");

            RuleFor(x => x.Data.LastName)
               .NotEmpty().WithMessage("Prezime je obavezno polje");

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id je obavezan");
        }
    }
}
