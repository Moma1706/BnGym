using System;
using FluentValidation;

namespace Application.DailyTraining
{
	public class DailyTrainingAddArrivalCommandValidator : AbstractValidator<DailyTrainingAddArrivalCommand>
    {
        public DailyTrainingAddArrivalCommandValidator()
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

