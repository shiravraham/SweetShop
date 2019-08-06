using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using final_project.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace final_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly SweetShopContext _context;

        public HomeController(SweetShopContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var a = _context.Users;
            return View();
        }
        public IActionResult Cart()
        {
            var keys = HttpContext.Session.Keys;
            Dictionary<int, int> quantites = new Dictionary<int, int>();
            List<Product> productsInBag = _context.Products.Where(x => keys.Contains(x.ID.ToString())).ToList();
            ViewBag.productsInBag = productsInBag;
            foreach(var key in keys)
            {
                quantites.Add(int.Parse(key), int.Parse(HttpContext.Session.GetString(key)));
            }
            ViewBag.quantites = quantites;
            return View();
        }

        [HttpGet]
        public IActionResult Shop()
        {
            List<Product> products = _context.Products.ToList();
            ViewBag.Products = products;

            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Shop(string searchText, int category, int minPrice, int maxPrice)
        {
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;

            List<Product> products;

            if (searchText != null)
            { 
                products = _context.Products.Where((p) => p.Name.Contains(searchText)).ToList();
            } else
            {
                products = _context.Products.ToList();
            }

            if (category != 0)
            {
                products = products.Where((p) => p.Category.ID == category).ToList();
            }

            if (maxPrice <= 0)
            {
                maxPrice = int.MaxValue;
            }

            products = products.Where((p) => p.Price >= minPrice && p.Price <= maxPrice).ToList();

            ViewBag.Products = products;
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
