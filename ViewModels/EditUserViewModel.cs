using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace Shop.ViewModels
{
    public class EditUserViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your first name.")]
        [StringLength(50)]
        [Display(Name = "First Name:")]

        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name.")]
        [StringLength(50)]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your birthdate.")]
        [Display(Name = "Birthdate:")]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Required(ErrorMessage = "Please enter your email.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email is not valid")]
        [StringLength(50)]
        [Display(Name = "Email Address:")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
