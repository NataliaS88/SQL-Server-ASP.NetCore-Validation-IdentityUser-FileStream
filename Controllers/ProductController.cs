using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Shop.Repositories;
using Shop.ViewModels;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Shop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository repository;
        private readonly IWebHostEnvironment webHostEnvironment;
        private UserManager<User> _userManager;

        public ProductController(IProductRepository repository, IWebHostEnvironment hostEnvironment, UserManager<User> userManager)
        {
            this.repository = repository;
            this.webHostEnvironment = hostEnvironment;
            this._userManager = userManager;
        }

        public IActionResult GetAllProducts(string sort = null)
        {     
            List<Product> notSoldProd = NotSoldProd();         
            List<Product> prodInCart = ProductsIncart();
            var currentProd = notSoldProd.Except(prodInCart);
            List<Product> prodForSale = new List<Product>();
            if (sort == null)
            {
                currentProd.Reverse();
                foreach (var prod in currentProd)
                {
                    prodForSale.Add(prod);
                }
                return View(prodForSale);
            }
            else if (sort == "by title")
            {
                var sorted = currentProd.OrderByDescending(s => s.Title);
                foreach (var prod in sorted)
                {
                    prodForSale.Add(prod);
                }

                prodForSale.Reverse();
                return View(prodForSale);
            }
            else
            {
                var sorted = currentProd.OrderByDescending(s => s.Date);
                foreach (var prod in sorted)
                {
                    prodForSale.Add(prod);
                }

                prodForSale.Reverse();
                return View(prodForSale);
            }
        }

        private List<Product> ProductsIncart()
        {

            List<Product> prodInCart = new List<Product>();

            foreach (var key in HttpContext.Session.Keys)
            {
                int? value = HttpContext.Session.GetInt32(key);
                int v = (int)value;
                var prod = repository.GetProduct(v);
                prodInCart.Add(prod);
            }
            return prodInCart;
        }

        [Authorize]
        public IActionResult AddNewProduct()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNewProduct([Bind("Id,OwnerId,UserId,Title,ShortDescription,LongDescription,Date,Price,Img1,Img2,Img3, State")]ProductViewModel p)
        {
            if (ModelState.IsValid)
            {
                string photo1 = UploadedFile(p, 1);
                string photo2 = UploadedFile(p, 2);
                string photo3 = UploadedFile(p, 3);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Product newProd = new Product
                {
                    OwnerId = null,
                    UserId = userId,
                    Title = p.Title,
                    ShortDescription = p.ShortDescription,
                    LongDescription = p.LongDescription,
                    Date = p.Date,
                    Price = p.Price,
                    Image1 = photo1,
                    Image2 = photo2,
                    Image3 = photo3,
                    State = 0
                };
                repository.CreateProduct(newProd);
                return RedirectToAction("GetAllProducts", "Product");
            }
            return RedirectToAction("Privacy", "Home");
        }

        private string UploadedFile(ProductViewModel model, int photoNumber)
        {
            string uniqueFileName = "noImage.png";
            if (photoNumber == 1)
            {
                if (model.Img1 != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = model.Img1.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Img1.CopyTo(fileStream);
                    }
                }
            }
            if (photoNumber == 2)
            {
                if (model.Img2 != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = model.Img2.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Img2.CopyTo(fileStream);
                    }
                }
            }
            if (photoNumber == 3)
            {
                if (model.Img3 != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    uniqueFileName =  model.Img3.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Img3.CopyTo(fileStream);
                    }
                }
            }
            uniqueFileName = $"~/images/{uniqueFileName}";
            return uniqueFileName;
        }

        public async Task<IActionResult> ProductDetails(int id)
        {
            try
            {
                var prod = repository.GetProduct(id);
                User seller = await _userManager.FindByIdAsync(prod.UserId);

                ViewBag.sellerName = seller.FirstName;
                ViewBag.email = seller.Email;

                return View(prod);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorTitle = $"There is no product with Id={id}";
                ViewBag.ErrorMessage = "Please choose exist product from main page";
                return View("Error");
            }
        }
        public IActionResult EditProduct(int id)
        {
            var prod = repository.GetProduct(id);
            ViewBag.id = prod.Id;
            ViewBag.img1 = prod.Image1;
            ViewBag.img2 = prod.Image2;
            ViewBag.img3 = prod.Image3;
            ProductViewModel editViewModel = new ProductViewModel
            {
                Id = id,
                Title = prod.Title,
                ShortDescription = prod.ShortDescription,
                LongDescription = prod.LongDescription,
                Price = prod.Price
            };
            return View(editViewModel);
        }
        [HttpPost]
        public IActionResult EditProduct(ProductViewModel edited)
        {
            if (ModelState.IsValid)
            {
                var prod = repository.GetProduct(edited.Id);
                prod.Title = edited.Title;
                prod.ShortDescription = edited.ShortDescription;
                prod.LongDescription = edited.LongDescription;
                prod.Price = edited.Price;
                if (edited.Img1 != null)
                {
                    string photo1 = UploadedFile(edited, 1);
                    prod.Image1 = photo1;
                }
                if (edited.Img2 != null)
                {
                    string photo2 = UploadedFile(edited, 2);
                    prod.Image2 = photo2;
                }
                if (edited.Img3 != null)
                {
                    string photo3 = UploadedFile(edited, 3);
                    prod.Image3 = photo3;
                }
                repository.UpdateProduct(prod);

            }
            return RedirectToAction("GetAllProducts", "Product");
        }
        public IActionResult DeleteProduct(int id)
        {
            repository.DeleteProduct(id);
            return RedirectToAction("GetAllProducts", "Product");
        }
        public IActionResult AddToCart(int id)
        {
            string key = id.ToString();
            if (HttpContext.Session.Keys.Contains(key))
                return RedirectToAction("GetAllProducts", "Product");

            HttpContext.Session.SetInt32(key, id);
            return RedirectToAction("GetAllProducts", "Product");
        }

        public IActionResult ShopingCart()
        {
            double sum = 0;
            List<Product> prodInCart = new List<Product>();

            foreach (var key in HttpContext.Session.Keys)
            {
                int? value = HttpContext.Session.GetInt32(key);
                int v = (int)value;
                var prod = repository.GetProduct(v);
                prodInCart.Add(prod);
                sum += prod.Price;
            }

            ViewBag.fullAmount = 0 + sum;
            return View(prodInCart);
        }

        public IActionResult DeleteProductFromCart(int id)
        {
            try
            {
                string key = id.ToString();
                int? res = HttpContext.Session.GetInt32(key);
                int k = (int)res;
                if (k == id)
                {
                    HttpContext.Session.Remove(key);
                    if (HttpContext.Session.Keys.Count() == 0)
                     return RedirectToAction("GetAllProducts", "Product"); 
                    return RedirectToAction("ShopingCart", "Product");

                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorTitle = $"There is no product with Id={id}";
                ViewBag.ErrorMessage = "Please choose exist product from cart";
            }
            return View("Error");

        }

        [Authorize]
        public IActionResult BuyProducts()
        {
            int keys = HttpContext.Session.Keys.Count();
            if (keys == 0)
            {
                return RedirectToAction("GetAllProducts", "Product");
            }
            foreach (var key in HttpContext.Session.Keys)
            {
                int? value = HttpContext.Session.GetInt32(key);
                int v = (int)value;
                var prod = repository.GetProduct(v);
                prod.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                repository.UpdateProduct(prod);
                HttpContext.Session.Remove(key);
            }
            return RedirectToAction("ShopingCart", "Product");
        }

        private List<Product> NotSoldProd()
        {
            List<Product> allProducts = (List<Product>)repository.GetProducts();
            List<Product> notSoldProd = new List<Product>();
            foreach (var prod in allProducts)
            {
                if (prod.OwnerId == null)
                    notSoldProd.Add(prod);
            }
            return notSoldProd;
        }
        private List<Product> SoldProd()
        {
            List<Product> allProducts = (List<Product>)repository.GetProducts();
            List<Product> soldProd = new List<Product>();
            foreach (var prod in allProducts)
            {
                if (prod.OwnerId != null)
                    soldProd.Add(prod);
            }
            return soldProd;
        }
        [Authorize]
        public IActionResult Report()
        {
            try
            {
                if (this.User.Identity.Name == "admin")
                {

                    var countProduct = repository.GetCount();
                    var notSold = NotSoldProd();
                    var soldProd = SoldProd();
                    ViewBag.CountProducts = countProduct;
                    ViewBag.SoldProducts = soldProd;
                    ViewBag.NotSoldProducts = notSold;
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorTitle = $"No access";
                ViewBag.ErrorMessage = "Sorry you are not allowed to use this resources";
            }
            return View("Error");


        }
    }
}