using System;
using Application.DailyUser;
using FluentValidation;

namespace Application.DailyUser
{
    public class DailyUserUpdateCommandValidator : AbstractValidator<DailyUserUpdateCommand>
    {
        public DailyUserUpdateCommandValidator()
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
                .Must(BeAValidDate).WithMessage("Invalid date format");
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
