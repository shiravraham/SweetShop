using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;

namespace final_project.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Get()
        {
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
