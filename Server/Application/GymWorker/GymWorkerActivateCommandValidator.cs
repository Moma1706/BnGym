using System;
using FluentValidation;

namespace Application.GymWorker
{
    public class GymWorkerActivateCommandValidator : AbstractValidator<GymWorkerActivateCommand>
    {
        public GymWorkerActivateCommandValidator()
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

