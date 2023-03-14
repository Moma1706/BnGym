using System;
using System.Numerics;
using FluentValidation;

namespace Application.GymUser;

public class GymUserCreateCommandValidator : AbstractValidator<GymUserCreateCommand>
{
    public GymUserCreateCommandValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not in correct format");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required"); ;

        RuleFor(x => x.LastName)
           .NotEmpty().WithMessage("Last name is required");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("User type is required")
            .IsInEnum().WithMessage("User type must have correct value");
    }
}