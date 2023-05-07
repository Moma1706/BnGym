using Application.DailyUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DailyUser
{
    public class DailyUserGetByDateCommandValidator : AbstractValidator<DailyUserGetByDateCommand>
    {
        public DailyUserGetByDateCommandValidator()
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
