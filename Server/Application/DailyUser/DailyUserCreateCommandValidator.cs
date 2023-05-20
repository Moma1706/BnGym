using Application.CheckIn.CheckIn;
using FluentValidation;

namespace Application.DailyUser;

public class DailyUserCommandValidator : AbstractValidator<DailyUserCreateCommand>
{
    public DailyUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ime je obavezno polje");

        RuleFor(x => x.LastName)
           .NotEmpty().WithMessage("Prezime je obavezno polje");

        RuleFor(x => x.DateOfBirth)
           .NotEmpty().WithMessage("Datum rodjenja je obavezno polje")
           .Must(BeAValidDate).WithMessage("Nevalidan format datuma");
    }

    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }
}