using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewModels
{
    public class ProductViewModel
    {
        [Key]
        public int Id { get; set; }

        //id user that bought this product
        [Display(Name = "Owner Id:")]
        public string OwnerId { get; set; }

        [Display(Name = "User Id:")]
        public string UserId { get; set; }

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

        [Required(ErrorMessage = "Please enter the date.")]
        [Display(Name = "Date of receiving:")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please enter the price.")]
        [Display(Name = "Price:")]
        public double Price { get; set; }

        //[Required(ErrorMessage = "Please choose profile image")]
        [Display(Name = "Image1:")]
        public IFormFile Img1 { get; set; }

      
        [Display(Name = "Image2:")]
        public IFormFile Img2 { get; set; }

        [Display(Name = "Image3:")]
        public IFormFile Img3 { get; set; }

        //[Required(ErrorMessage = "Please enter the state.")]
        [Display(Name = "State:")]

        //states: 0-for sale, 1- in shopping cart 2-sold
        public int State { get; set; }
    }
}
