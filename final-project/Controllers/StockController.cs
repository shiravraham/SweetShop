using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;

namespace final_project.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            throw new NotImplementedException();
        }
        public IActionResult Update()
        {
            throw new NotImplementedException();
        }
        public IActionResult Delete()
        {
            throw new NotImplementedException();
        }
    }
}
