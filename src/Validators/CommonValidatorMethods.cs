using FluentValidation;
using System;

namespace SPX_WEBAPI.Validators
{
    public static class CommonValidatorMethods
    {
        public static bool BeAValidDate(DateTime? date)
        {
            if (date?.Year < DateTime.UtcNow.Year - 100)
                return false;

            return true;
        }
    }
}
