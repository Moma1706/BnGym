using Application.GymUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CheckIn
{
    public class CheckInGetByDateCommandValidator : AbstractValidator<CheckInGetByDateCommand>
    {
        public CheckInGetByDateCommandValidator()
        {
            RuleFor(x => x.DateTime)
                .NotEmpty().WithMessage("Date is required");
        }
    }
}
