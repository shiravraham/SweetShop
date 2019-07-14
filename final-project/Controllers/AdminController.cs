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
    public class AdminController : Controller
    {
        private readonly SweetShopContext _context;

        public AdminController(SweetShopContext context)
        {
            _context = context;
        }

        public IActionResult Orders()
        {
            List<Order> orders = _context.Orders.ToList();
            List<User> users = _context.Users.ToList();
            List<OrderStatus> statuses = _context.OrderStatuses.ToList();
            ViewBag.Orders = orders;

            return View();
        }
        public IActionResult Statistics()
        {
            return View();
        }

        public IActionResult EditProducts()
        {
            List<Product> products = _context.Products.ToList();
            ViewBag.Products = products;
            return View();
        }
    }
}
