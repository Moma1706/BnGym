using System;
using FluentValidation;

namespace Application.DailyUser
{
	public class DailyUserAddArrivalCommandValidator : AbstractValidator<DailyUserAddArrivalCommand>
    {
        public DailyUserAddArrivalCommandValidator()
        {
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

