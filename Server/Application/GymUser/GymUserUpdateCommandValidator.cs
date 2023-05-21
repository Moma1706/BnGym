using System;
using FluentValidation;

namespace Application.GymUser
{
	public class GymUserUpdateCommandValidator : AbstractValidator<GymUserUpdateCommand>
    {
        public GymUserUpdateCommandValidator()
        {
            RuleFor(x => x.Data.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email je obavezno polje")
                .EmailAddress().WithMessage("Email nije u korektnom formatu");

            RuleFor(x => x.Data.FirstName)
                .NotEmpty().WithMessage("Ime je obavezno polje");

            RuleFor(x => x.Data.LastName)
               .NotEmpty().WithMessage("Prezime je obavezno polje");

            RuleFor(x => x.Data.Type)
                .IsInEnum().WithMessage("Tip mora imati korektnu vrijednost");

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
