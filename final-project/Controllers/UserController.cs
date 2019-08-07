using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using final_project.Data;
using Microsoft.AspNetCore.Http;

namespace final_project.Controllers
{
    public class UserController : Controller
    {
        private readonly SweetShopContext _context;

        public UserController(SweetShopContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (email == null || password == null)
            {
                return View();
            }

            var user = _context.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }

            //HttpContext.Session.SetString("isAdmin",  "true");
            HttpContext.Session.SetString("username", user.Username);
            HttpContext.Session.SetString("fullName", user.FullName);

            if (user != null)
            {
                return RedirectToAction("Welcome", "Admin", null);
            }
            return RedirectToAction("Index", "Home", null);
        }
    }
}
