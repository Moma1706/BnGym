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
                .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required");

            RuleFor(x => x.NewPassword)
               .NotEmpty().WithMessage("New password is required")
               .MinimumLength(8).WithMessage("Password must have at least 8 characters")
               .Matches("[0-9]").WithMessage("Password must have at least one number");

            RuleFor(x => x.ConfirmNewPassword)
               .NotEmpty().WithMessage("You must confirm password")
               .Equal(x => x.NewPassword).WithMessage("Passwords do not match");
        }
    }
}

