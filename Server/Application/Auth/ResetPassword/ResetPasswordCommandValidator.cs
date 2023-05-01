using FluentValidation;

namespace Application.Auth.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Password)
           .NotEmpty().WithMessage("Password is required")
           .MinimumLength(8).WithMessage("Password must have at least 8 characters")
           .Matches("[0-9]").WithMessage("Password must have at least one number");

        RuleFor(x => x.ConfirmPassword)
           .NotEmpty().WithMessage("You must confirm password")
           .Equal(x => x.Password).WithMessage("Passwords do not match");
    }
}