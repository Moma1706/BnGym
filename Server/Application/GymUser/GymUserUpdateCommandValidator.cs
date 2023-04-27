using System;
using FluentValidation;

namespace Application.GymUser
{
	public class GymUserUpdateCommandValidator : AbstractValidator<GymUserUpdateCommand>
    {
        public GymUserUpdateCommandValidator()
        {
            RuleFor(x => x.Data.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not in correct format");

            RuleFor(x => x.Data.FirstName)
                .NotEmpty().WithMessage("First name is required");

            RuleFor(x => x.Data.LastName)
               .NotEmpty().WithMessage("Last name is required");

            RuleFor(x => x.Data.Type)
                .NotNull().WithMessage("User type is required")
                .IsInEnum().WithMessage("User type must have correct value");

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .Must(BeAValidGuid).WithMessage("Invalid UUID");
        }

        private bool BeAValidGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }
    }
}
