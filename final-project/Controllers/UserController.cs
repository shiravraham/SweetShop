using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using final_project.Data;

namespace final_project.Controllers
{
    public class UserController : Controller
    {
        private readonly SweetShopContext _context;

        public UserController(SweetShopContext context)
        {
            _context = context;
        }

        public IActionResult Get()
        {
            var a = _context.Users;
            return View();
        }

        public IActionResult Post()
        {
            return View();
        }

        public IActionResult Put()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
    }
}
