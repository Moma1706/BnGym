using System;
using FluentValidation;

namespace Application.DailyUser
{
	public class DailyUserGetOneCommandValidator : AbstractValidator<DailyUserGetOneCommand>
    {
        public DailyUserGetOneCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id je obavezan")
                .Must(BeAValidGuid).WithMessage("Nevalidan UUID");
        }

        private bool BeAValidGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }
    }
}

