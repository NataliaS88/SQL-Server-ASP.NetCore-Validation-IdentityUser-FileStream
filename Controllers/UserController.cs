using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Exchange.WebServices.Data;
using Shop.Data;
using Shop.Models;
using Shop.Repositories;
using Shop.ViewModels;

namespace Shop.Controllers
{
    public class UserController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private readonly AppDbContext db;

        public UserController(SignInManager <User> signInManager, UserManager<User> userManager, AppDbContext db )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            this.db = db;       
        }

        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User
                {
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    UserName = registerModel.UserName,
                    Birthdate = registerModel.Birthdate,
                    Email = registerModel.Email,
                   

                };

                var result = await _userManager.CreateAsync(newUser, registerModel.Password);
                if (result.Succeeded)
                {
                   await _signInManager.SignInAsync(newUser, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            int keys = HttpContext.Session.Keys.Count();
            if (keys > 0)
            {
                foreach (var key in HttpContext.Session.Keys)
                {
                    HttpContext.Session.Remove(key);
                }
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit()
        {
         var   userId = _userManager.GetUserId(HttpContext.User);

               var user = await _userManager.FindByIdAsync(userId);
            EditUserViewModel editViewModel = new EditUserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Birthdate = user.Birthdate
            };
            return View(editViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel editModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(userId);
                user.FirstName = editModel.FirstName;
                user.LastName = editModel.LastName;
                user.Email = editModel.Email;
                user.Birthdate = editModel.Birthdate;
                var result = await _userManager.UpdateAsync(user);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Report()
        {
            try
            {
                if (this.User.Identity.Name == "admin")
                {
                    var countUsers = db.AppUsers.Count();
                    ViewBag.CountUsers = countUsers;
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