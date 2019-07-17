using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using Microsoft.CodeAnalysis.CSharp;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace final_project.Controllers
{
    public class CheckoutController : Controller
    {
        private const string BaseCurrencyApiURL = "https://api.exchangeratesapi.io/latest?base=ILS&symbols=";

        public Currency CurrentCurrency { get; set; }

        public double CurrencyExchangeRate { get; set; }

        public List<Product> Cart { get; set; } = new List<Product>()
        {
            new Product()
                {ID = 1, Name = "chocolate cake", Price = 12, Category = new Category() {ID = 1, Name = "cakes"}},
            new Product() {ID = 1, Name = "cheese cake", Price = 12, Category = new Category() {ID = 1, Name = "cakes"}}
        }; //TODO: get cart from session

        // GET: /<controller>/{choosenCurrency}
        public async Task<IActionResult> Checkout(Currency choosenCurrency = Currency.ILS)
        {
            await UpdateCurrency(choosenCurrency);
            ViewBag.CurrentCurrency = CurrentCurrency;
            ViewBag.Cart = Cart;
            ViewBag.ConvertToCurrentCurrency = new Func<double,double> (ConvertToCurrentCurrency);
            ViewBag.CartSum = ConvertToCurrentCurrency(GetCartSum());
            ViewBag.CartSize = Cart.Count;
            return View();
        }

        public double GetCartSum()
        {
            return Cart.Sum(x => x.Price);
        }

        public double ConvertToCurrentCurrency(double value)
        {
            return value * CurrencyExchangeRate;
        }

        public async Task<double> GetCurrencyExchangeRate(Currency wantedCurrency)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"{BaseCurrencyApiURL}{CurrentCurrency}");
            response.EnsureSuccessStatusCode();

            var returnVal = JObject.Parse(await response.Content.ReadAsStringAsync());

            return (double)returnVal["rates"][wantedCurrency.ToString()];
        }

        public async Task UpdateCurrency(Currency newCurrency)
        {
            CurrentCurrency = newCurrency;
            CurrencyExchangeRate = await GetCurrencyExchangeRate(newCurrency);
        }
    }
}