using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using final_project.Data;
using Accord.MachineLearning.Rules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace final_project.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly SweetShopContext _context;

        public StatisticsController(SweetShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Statistics(Product[] productToReturn)
        {
            Apriori<Product> apriori = new Apriori<Product>(0, 0);

            // Get the Models
            List<Order> orders = _context.Orders.ToList();
            List<Product> products = _context.Products.ToList();
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

            // Return the first row of the suggestion - the most fit suggesion (can return the whole suggestions instead)
            return Json(decideProd[0]);
        }
        [AllowAnonymous]
        public ActionResult MostOrderedCake()
        {
            var amountOfProductsSold = _context.OrderItems
                .Include(order => order.Product)
                .GroupBy(Order => Order.Product)
                .Select(o => new SoldDesert
                {
                    product = o.Key.Name,
                    count = o.Sum(order => order.Quantity)
                })
                .OrderByDescending(o => o.count)
                .ToList();

            return Json(amountOfProductsSold);
        }

        public ActionResult CategoriesGraph()
        {

            var sellesbyCategories = _context.OrderItems
                .Join(_context.Products,
                order => order.Product.ID,
                product => product.ID, 
                (order, product) => new {
                    product = product,
                    order = order
                }).Join(_context.Categories,
                order => order.product.Category.ID,
                category=>category.ID,
                (order, category) => new {
                    order = order.order,
                    product = order.product,
                    category = category
                })
                .GroupBy(x => x.category)
                .Select(o => new SoldDesert
                {
                    product = o.Key.Name,
                    count = o.Sum(order => order.order.Quantity)
                })
                .ToList();

            return Json(sellesbyCategories);
        }
    }
}
