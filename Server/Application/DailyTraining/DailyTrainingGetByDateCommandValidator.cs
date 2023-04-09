using Application.DailyTraining;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DailyTraining
{
    public class DailyTrainingGetByDateCommandValidator : AbstractValidator<DailyTrainingGetByDateCommand>
    {
        public DailyTrainingGetByDateCommandValidator()
        {
            RuleFor(x => x.DateTime)
                .NotEmpty().WithMessage("Date is required")
                .Must(BeAValidDate).WithMessage("Date is required");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
