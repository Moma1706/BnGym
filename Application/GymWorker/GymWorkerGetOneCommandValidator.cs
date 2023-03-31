﻿using Application.GymWorker;
using FluentValidation;

namespace Application.GymWorker
{
    public class GymWorkerGetOneCommandValidator : AbstractValidator<GymWorkerGetOneCommand>
    {
        public GymWorkerGetOneCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
