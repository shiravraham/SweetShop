using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;

namespace final_project.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult Statistics()
        {
            return View();
        }

        public IActionResult EditProducts()
        {
            return View();
        }
    }
}
