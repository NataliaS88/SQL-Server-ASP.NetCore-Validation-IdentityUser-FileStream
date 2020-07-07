using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop.Models;
using Shop.Repositories;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _repository;

        public HomeController(IProductRepository repository, ILogger<HomeController> logger)
        {
            this._logger = logger;
           this._repository = repository;
        }
        
        public IActionResult Index()
        {
            var list = _repository.GetProducts();
          
           var lastPosts= list.Skip(Math.Max(0, list.Count() - 3));
            if (lastPosts.First()!=null)
           ViewBag.p1= lastPosts.First();
            if (lastPosts.First() != lastPosts.Last())
            {
                ViewBag.p2 = lastPosts.ElementAt(1);
                if (lastPosts.ElementAt(1) != lastPosts.Last())
                    ViewBag.p3 = lastPosts.Last();
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
