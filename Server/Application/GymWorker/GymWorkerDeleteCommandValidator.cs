﻿using System;
using FluentValidation;

namespace Application.GymWorker
{
    public class GymWorkerDeleteCommandValidator : AbstractValidator<GymWorkerDeleteCommand>
    {
        public GymWorkerDeleteCommandValidator()
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