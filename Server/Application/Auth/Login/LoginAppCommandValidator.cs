using FluentValidation;

namespace Application.Auth.Login
{
    public class LoginAppCommandValidator : AbstractValidator<LoginAppCommand>
    {
        public LoginAppCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");
        }
    }
}
