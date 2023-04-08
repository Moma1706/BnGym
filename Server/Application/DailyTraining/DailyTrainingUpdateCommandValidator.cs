using System;
using FluentValidation;

namespace Application.DailyTraining
{
    public class DailyTrainingUpdateCommandValidator : AbstractValidator<DailyTrainingUpdateCommand>
    {
        public DailyTrainingUpdateCommandValidator()
        {
            RuleFor(x => x.Data.FirstName)
                .NotEmpty().WithMessage("First name is required");

            RuleFor(x => x.Data.LastName)
               .NotEmpty().WithMessage("Last name is required");

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .Must(BeAValidGuid).WithMessage("Invalid UUID");

            RuleFor(x => x.Data.DateOfBirth)
                .NotEmpty().WithMessage("Date is required")
                .Must(BeAValidDate).WithMessage("Date is required");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }

        private bool BeAValidGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }
    }
}
