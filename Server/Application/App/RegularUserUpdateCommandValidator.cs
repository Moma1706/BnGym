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
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not in correct format");

            RuleFor(x => x.Data.FirstName)
                .NotEmpty().WithMessage("First name is required");

            RuleFor(x => x.Data.LastName)
               .NotEmpty().WithMessage("Last name is required");

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
