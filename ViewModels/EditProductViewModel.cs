using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewModels
{
    public class EditProductViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter product title.")]
        [Display(Name = "Title:")]
        [StringLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please write short product info.")]
        [Display(Name = "Short description:")]
        [StringLength(500)]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Please write full product info.")]
        [Display(Name = "Full description:")]
        [StringLength(4000)]
        public string LongDescription { get; set; }

        [Required(ErrorMessage = "Please enter the price.")]
        [Display(Name = "Price:")]
        public double Price { get; set; }

        [Display(Name = "Image1:")]
        public IFormFile Image1 { get; set; }

        [Display(Name = "Image2:")]
        public IFormFile Image2 { get; set; }

        [Display(Name = "Image3:")]
        public IFormFile Image3 { get; set; }
    }
}
