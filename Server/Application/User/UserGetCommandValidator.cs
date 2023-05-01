using System;
using Application.GymWorker;
using FluentValidation;

namespace Application.User
{
    public class UserGetCommandValidator : AbstractValidator<UserGetCommand>
    {
        public UserGetCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
