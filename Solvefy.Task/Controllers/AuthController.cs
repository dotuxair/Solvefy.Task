using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Solvefy.Task.Data;
using Solvefy.Task.Models.Dtos;
using System;
using System.Linq;
using Solvefy.Task.Models.Entities;

namespace Solvefy.Task.Controllers
{
    public class AuthController : Controller
    {
        private readonly InventoryDbContext _dbContext;
        public AuthController(InventoryDbContext dbContext) => _dbContext = dbContext;

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginDto userData)
        {
            if (ModelState.IsValid)
            {
                var user = _dbContext.Users.SingleOrDefault(u => u.Email == userData.Email && u.Password == userData.Password);
                if (user != null)
                {
                    if(user.Role == 1)
                    {
                        SetCookie("role", "Admin", 10);
                    }
                    SetCookie("role", "User", 10);
                    return RedirectToAction("Index", "Product");
                }
                ViewBag.Error = "Wrong Email or Password.";
            }
            return View(userData);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("role");
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserSignupDto userData)
        {
            if (ModelState.IsValid)
            {
                var dbuser = _dbContext.Users.SingleOrDefault(u => u.Email == userData.Email);
                if (dbuser != null) {
                    ViewBag.Error = "Email already exist";
                    return View();
                }
                var user = new User
                {
                    Name = userData.Name,
                    Email = userData.Email,
                    Password = userData.Password,
                };
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return RedirectToAction("Login");
            }
            return View(userData);
        }

        private void SetCookie(string key, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddMinutes(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);

            Response.Cookies.Append(key, value, option);
        }
    }
}
