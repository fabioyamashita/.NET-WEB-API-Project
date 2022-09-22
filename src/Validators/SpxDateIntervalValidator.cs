using FluentValidation;
using SPX_WEBAPI.Domain.Dto;
using System.Runtime.CompilerServices;

namespace SPX_WEBAPI.Validators
{
    public class SpxDateIntervalValidator : AbstractValidator<SpxDateInterval>
    {
        public SpxDateIntervalValidator()
        {
            RuleFor(d => d.StartDate)
                .NotEmpty()
                    .WithMessage("StartDate is required")
                .Must(CommonValidatorMethods.BeAValidDate)
                    .WithMessage("Date must be valid");

            RuleFor(d => d.EndDate)
                .NotEmpty()
                    .WithMessage("EndDate is required")
                .Must(CommonValidatorMethods.BeAValidDate)
                    .WithMessage("Date must be valid")
                .GreaterThanOrEqualTo(d => d.StartDate)
                    .WithMessage("End date must be greater or equal than Start date");
        }
    }
}
