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
    public class StatisticsController : Controller
    {
        private readonly SweetShopContext _context;

        public StatisticsController(SweetShopContext context)
        {
            _context = context;
        }

        public ActionResult MostOrderedCake()
        {
            var amountOfProductsSold = _context.OrderItems
                .Include(order => order.Product)
                .GroupBy(Order => Order.Product)
                .Select(o => new GraphData
                {
                    Name = o.Key.Name,
                    Value = o.Sum(order => order.Quantity)
                })
                .OrderByDescending(o => o.Value)
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
                .Select(o => new GraphData
                {
                    Name = o.Key.Name,
                    Value = o.Sum(order => order.order.Quantity)
                })
                .ToList();

            return Json(sellesbyCategories);
        }

        public ActionResult SumOrdersPerMonthGraph()
        {
            var sellesbyCategories = _context.OrderItems
                .Join(_context.Products,
                order => order.Product.ID,
                product => product.ID,
                (orderItem, product) => new {
                    product = product,
                    orderItem = orderItem
                }).Join(_context.Orders,
                orderDetails => orderDetails.orderItem.Order.Id,
                order => order.Id,
                (orderDetails, order) => new {
                    orderItem = orderDetails.orderItem,
                    product = orderDetails.product,
                    order = order
                })
                .GroupBy(x => x.order.OrderDate.ToString("MM-yyyy"))
                .Select(o => new GraphData
                {
                    Name = o.Key.ToString(),
                    Value = o.Sum(order => order.orderItem.Quantity * order.product.Price)
                })
                .ToList();

            return Json(sellesbyCategories);
        }
    }
}
