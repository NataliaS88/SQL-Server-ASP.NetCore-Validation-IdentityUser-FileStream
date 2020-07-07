using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewComponents.ProductViewComponent
{
    public class ProductViewComponent : ViewComponent
    {
        public  Task<IViewComponentResult> InvokeAsync(Shop.Models.Product prod)
        {
            return  Task.FromResult<IViewComponentResult>(View("Product", prod));
        }
    }
}