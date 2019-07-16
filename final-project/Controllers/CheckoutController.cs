using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace final_project.Controllers
{
    public class CheckoutController : Controller
    {
        private Currency _currentCurrency = Currency.ILS;

        public Currency CurrentCurrency
        {
            get => _currentCurrency;
            set
            {
                _currentCurrency = value;
                //TODO: get current exchange rate from WebService
            }
        }

        public double CurrencyExchaneRate { get; set; } = 1;

        public List<Product> Cart { get; set; } = new List<Product>()
        {
            new Product()
                {ID = 1, Name = "chocolate cake", Price = 12, Category = new Category() {ID = 1, Name = "cakes"}},
            new Product() {ID = 1, Name = "cheese cake", Price = 12, Category = new Category() {ID = 1, Name = "cakes"}}
        }; //TODO: get cart from session

        // GET: /<controller>/{choosenCurrency}
        public IActionResult Checkout(Currency choosenCurrency)
        {
            CurrentCurrency = choosenCurrency;
            ViewBag.CurrentCurrency = CurrentCurrency;
            ViewBag.Cart = Cart;
            ViewBag.ConvertToCurrentCurrency = new Func<double,double> (ConvertToCurrentCurrency);
            ViewBag.CartSum = ConvertToCurrentCurrency(GetCartSum());
            ViewBag.CartSize = Cart.Count;
            return View();
        }

        public double GetCartSum()
        {
            return ConvertToCurrentCurrency(Cart.Sum(x => x.Price));
        }

        public double ConvertToCurrentCurrency(double value)
        {
            return value * CurrencyExchaneRate;
        }

    }
}