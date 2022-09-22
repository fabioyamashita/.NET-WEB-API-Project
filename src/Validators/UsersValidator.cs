using FluentValidation;
using Microsoft.AspNetCore.Rewrite;
using Newtonsoft.Json.Linq;
using SPX_WEBAPI.Domain.Dto;
using System;
using System.Text.RegularExpressions;

namespace SPX_WEBAPI.Validators
{
    public class UsersValidator : AbstractValidator<UsersDto>
    {
        public UsersValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Name must not be null or empty")
                .MaximumLength(50)
                    .WithMessage("Name length must be less or equal than 50")
                .MinimumLength(2)
                    .WithMessage("Name length must be greater or equal than 2");

            RuleFor(x => x.Username)
                .NotEmpty()
                    .WithMessage("Username must not be null or empty")
                .MaximumLength(15)
                    .WithMessage("Username length must be less or equal than 15")
                .MinimumLength(3)
                    .WithMessage("Username length must be greater or equal than 3");

            RuleFor(x => x.Password)
                .NotEmpty()
                    .WithMessage("Password must not be null or empty")
                .Matches(@"^(?=.*?[a-z])(?=.*?[0-9]).{8,}$")
                    .WithMessage("Password length must be at least 8 and must contain at least one letter and one number");

            RuleFor(x => x.Role)
                .NotEmpty()
                    .WithMessage("Role must not be null or empty");
                //.IsInEnum()
                //    .WithMessage("Role invalid");

        }
    }
}
