using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using final_project.Data;
using Microsoft.EntityFrameworkCore;

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
            List<Category> categories = _context.Categories.ToList();

            ViewBag.Products = products;
            ViewBag.Categories = categories;
            return View();
        }

        public IActionResult RemoveProduct(int id)
        {
            _context.Remove(_context.Products.Single(p => p.ID == id));
            _context.SaveChanges();
            return Redirect("/Admin/EditProducts");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(string name, int category, int price)
        {
            Product newProduct = new Product()
            {
                Name = name,
                Category = _context.Categories.Single(c => c.ID == category),
                Price = price,
                
            };

            _context.Add(newProduct);
            _context.SaveChanges();
            return Redirect("/Admin/EditProducts");
        }
    }
}
