using System;
using Application.DailyUser;
using FluentValidation;

namespace Application.DailyUser
{
    public class DailyUserUpdateCommandValidator : AbstractValidator<DailyUserUpdateCommand>
    {
        public DailyUserUpdateCommandValidator()
        {
            RuleFor(x => x.Data.FirstName)
                .NotEmpty().WithMessage("Ime je obavezno polje");

            RuleFor(x => x.Data.LastName)
               .NotEmpty().WithMessage("Prezime je obavezno polje");

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id je obavezan")
                .Must(BeAValidGuid).WithMessage("Nevallidan UUID");

            RuleFor(x => x.Data.DateOfBirth)
                .NotEmpty().WithMessage("Datum rodjenja je obavezno polje")
                .Must(BeAValidDate).WithMessage("Nevalidan format datumat");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }

        private bool BeAValidGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }
    }
}
