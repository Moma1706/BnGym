using System;
using Application.Auth.Login;
using FluentValidation;

namespace Application.Auth
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id je obavezan");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Trenutna lozinka je obavezno polje");

            RuleFor(x => x.NewPassword)
               .NotEmpty().WithMessage("Nova lozinka je obavezno polje")
               .MinimumLength(8).WithMessage("Lozinka mora sadržati najmanje 8 karaktera")
               .Matches("[0-9]").WithMessage("Lozinka mora sadržati najmanje 1 broj")
               .Matches("[A-Z]").WithMessage("Lozinka mora sadržati najmanje jedno veliko slovo");

            RuleFor(x => x.ConfirmNewPassword)
               .NotEmpty().WithMessage("Morate potvrditi novu lozinku")
               .Equal(x => x.NewPassword).WithMessage("Nove lozinke se ne podudaraju");
        }
    }
}

