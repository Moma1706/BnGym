using System;
using System.Numerics;
using Application.GymUser;
using Application.GymWorker;
using FluentValidation;

namespace Application.GymWorker;

public class GymWorkerCreateCommandValidator : AbstractValidator<GymWorkerCreateCommand>
{
    public GymWorkerCreateCommandValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not in correct format");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required"); ;

        RuleFor(x => x.LastName)
           .NotEmpty().WithMessage("Last name is required");
    }
}