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
    public class BranchController : Controller
    {
        private readonly SweetShopContext _context;

        public BranchController(SweetShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult getBranches()
        {
            List<Branch> branches = _context.Branches.ToList();
            ViewBag.Branches = branches;

            return Json(branches);
        }
    }
}
