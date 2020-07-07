using Microsoft.AspNetCore.Identity;
using Shop.Validator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class LoginViewModel 
    {
        

        [StringLength(50)]
        [Display(Name = "Username:")]
        [RegularExpression("[a-zA-Z0-9_\\.-]{3,10}$", ErrorMessage = "Username length from 3 to 10")]
        [UsernameValidationAttribute]
        [Required(ErrorMessage = "Please enter your username.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [RegularExpression("(?=.*)(?=.*[a-z])(?=.*[A-Z]).{6,}$", ErrorMessage = "Password must contain at least 6 characters, including UPPER/lowercase and numbers")]
        [StringLength(50)]
        [Display(Name = "Password:")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
