using FluentValidation;
using SPX_WEBAPI.Domain.Dto;

namespace SPX_WEBAPI.Validators
{
    public class SpxValidator : AbstractValidator<SpxDto>
    {
        public SpxValidator()
        {
            RuleFor(spx => spx.Date)
                .NotEmpty()
                    .WithMessage("Date must not be null or empty")
                .Must(CommonValidatorMethods.BeAValidDate)
                    .WithMessage("Date must be valid");

            RuleFor(spx => spx.Close)
                .NotEmpty()
                    .WithMessage("Close must not be null or empty")
                .GreaterThan(0)
                    .WithMessage("Close must be a positive number");

            RuleFor(spx => spx.Open)
                .NotEmpty()
                    .WithMessage("Open must not be null or empty")
                .GreaterThan(0)
                    .WithMessage("Open must be a positive number");

            RuleFor(spx => spx.High)
                .NotEmpty()
                    .WithMessage("High must not be null or empty")
                .GreaterThan(0)
                    .WithMessage("High must be a positive number");

            RuleFor(spx => spx.Low)
                .NotEmpty()
                    .WithMessage("Low must not be null or empty")
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Low must be a positive number")
                .LessThanOrEqualTo(spx => spx.High)
                    .WithMessage("Low should not be grater than High");

        }
    }
}
