using Shop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Validator
{
    public class UsernameValidationAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            LoginViewModel user = (LoginViewModel)validationContext.ObjectInstance;

            if (user.UserName==null)
            {
                return new ValidationResult("Sorry username is null");
            }
            if (user.UserName.Length <= 2)
            {
                return new ValidationResult("Sorry username needs to be longer");
            }
           
            if (user is RegistrationViewModel)
            {
                RegistrationViewModel registratedUser = (RegistrationViewModel)user;
                if (!registratedUser.Email.Contains("@"))
                {
                    return new ValidationResult("Email must contain @");
                }
            }
            return ValidationResult.Success;
        }
    }
}
