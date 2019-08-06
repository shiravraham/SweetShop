using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using final_project.Data;
using Microsoft.Extensions.FileProviders;
using Accord.MachineLearning.Rules;

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

        public IActionResult Statistics(Product[] productToReturn)
        {
            Apriori<Product> apriori = new Apriori<Product>(0, 0);

            List<Order> orders = _context.Orders.ToList();
            List<Product> products = _context.Products.ToList();
            List<OrderItem> orderItems = _context.OrderItems.ToList();
            List<IGrouping<int, OrderItem>> groupItems = orderItems.GroupBy(o => o.Order.Id).ToList();
            SortedSet<Product>[] productSets = new SortedSet<Product>[groupItems.Count];

            int i = 0;

            foreach (IGrouping<int, OrderItem> group in groupItems)
            {
                productSets[i] = new SortedSet<Product>();

                foreach (OrderItem item in group)
                {
                    productSets[i].Add(item.Product);
                }

                i++;
            }

            AssociationRuleMatcher<Product> productsRules = apriori.Learn(productSets);

            //Product[] productToReturn = new Product[] { products[0] };

            Product[][] decideProd = productsRules.Decide(productToReturn);

            return Json(decideProd[0]);
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
