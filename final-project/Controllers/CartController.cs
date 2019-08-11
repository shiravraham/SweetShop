using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using final_project.Data;
using final_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Accord.MachineLearning.Rules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace final_project.Controllers
{
    public class CartController : Controller
    {

        private readonly SweetShopContext _context;

        public CartController(SweetShopContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Cart()
        {

            var keys = HttpContext.Session.Keys.Where(key => key != "username" && key != "fullName");
            Dictionary<int, int> quantites = new Dictionary<int, int>();
            List<Product> productsInBag = _context.Products.Where(x => keys.Contains(x.ID.ToString())).Include(product=>product.Category).ToList();
            ViewBag.productsInBag = productsInBag;

            foreach (var key in keys)
            {
                quantites.Add(int.Parse(key), int.Parse(HttpContext.Session.GetString(key)));
            }

            ViewBag.Statistics = Statistics().ToList();
            ViewBag.quantites = quantites;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            if(HttpContext.Session.GetString(id.ToString()) ==null)
            {
                HttpContext.Session.SetString(id.ToString(), quantity.ToString());

            }
            else
            {
                var newQuantity = int.Parse(HttpContext.Session.GetString(id.ToString())) + quantity;
                HttpContext.Session.SetString(id.ToString(), newQuantity.ToString());

                
            }
            return RedirectToAction("Shop", "Home");
        }

        public async Task<IActionResult> DeleteProducet(int id)
        {
            HttpContext.Session.Remove(id.ToString());
            
            return RedirectToAction("Cart", "Home");
        }

        public IEnumerable<Product> GetCartFormSession()
        {
            var cartIDs = HttpContext.Session.Keys.Where(id => int.TryParse(id, out var num)).Select(int.Parse);
            return _context.Products.Include(prod => prod.Category).Where(product => cartIDs.Contains(product.ID));
        }

        public Product[] Statistics()
        {
            Product[] productToReturn = GetCartFormSession().ToArray();
            Apriori<Product> apriori = new Apriori<Product>(0, 0);

            // Get the Models
            List<Order> orders = _context.Orders.ToList();
            List<Product> products = _context.Products.Include(product => product.Category).ToList();
            List<OrderItem> orderItems = _context.OrderItems.ToList();

            // Group the oredered products by orders
            List<IGrouping<int, OrderItem>> groupItems = orderItems.GroupBy(o => o.Order.Id).ToList();

            // Define the sorted set for the learning algorithm
            SortedSet<Product>[] productSets = new SortedSet<Product>[groupItems.Count];

            int i = 0;

            // Initialize the sorted set for the learning algorithm
            foreach (IGrouping<int, OrderItem> group in groupItems)
            {
                productSets[i] = new SortedSet<Product>();

                foreach (OrderItem item in group)
                {
                    productSets[i].Add(item.Product);
                }

                i++;
            }

            // Execute the learning algorithm and get the rules' object to get the sugguestions from
            AssociationRuleMatcher<Product> productsRules = apriori.Learn(productSets);

            // Execute the suggustion function with the products chosen by the client => 'Decide'
            Product[][] decideProd = productsRules.Decide(productToReturn);

            if (decideProd.Length == 0)
            {
                return _context.Products.Include(product => product.Category).Take(3).ToArray();
            }
            // Return the first row of the suggestion - the most fit suggesion (can return the whole suggestions instead)
            return decideProd[0];
        }
    }
}
