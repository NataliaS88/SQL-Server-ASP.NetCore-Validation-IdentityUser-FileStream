using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewComponents.LoginViewComponent
{
    public class LoginViewComponent : ViewComponent
    {

        IHttpContextAccessor _httpContextAccessor;
        private SignInManager<User> _signInManager;

        public LoginViewComponent(IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string cookieUserName = "";
            string cookieUserPass = "";
            if (ModelState.IsValid && Request.HasFormContentType)
            {
               
                var result = await _signInManager.PasswordSignInAsync(Request.Form.First().Value,
                    Request.Form.Skip(1).First().Value, false, false);
                if (result.Succeeded)
                {
                    bool save = true;
                    string name = Request.Form.First().Value;
                    ViewBag.UserName = name;
                    CookieOptions opt = new CookieOptions();
                    opt.MaxAge = TimeSpan.FromMinutes(1);
                    opt.Path = cookieUserName;
                     cookieUserName = Request.Form.First().Value;
                     cookieUserPass = Request.Form.Skip(1).First().Value;
                    if (save)
                        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieUserName, cookieUserPass, opt);
                    else
                        _httpContextAccessor.HttpContext.Response.Cookies.Append(cookieUserName, cookieUserPass);
              
                    return await Task.FromResult<IViewComponentResult>(View("Logout", new LoginViewModel()));
                }
                else
                    ModelState.AddModelError("Password", "The user name or password provided is incorrect.");
            }

            if (this.User.Identity.IsAuthenticated)
            {
                return await Task.FromResult<IViewComponentResult>(View("Logout", new LoginViewModel()));
            }

            return await Task.FromResult<IViewComponentResult>(View("Login", new LoginViewModel()));
        }

    }
}

