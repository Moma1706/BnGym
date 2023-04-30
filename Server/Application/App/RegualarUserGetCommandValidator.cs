using System;
using FluentValidation;

namespace Application.User
{
    public class RegularUserGetCommandValidator : AbstractValidator<RegularUserGetCommand>
    {
        public RegularUserGetCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}

