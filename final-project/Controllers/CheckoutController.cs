using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using final_project.Data;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Remotion.Linq.Clauses;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace final_project.Controllers
{
    public class CheckoutController : Controller
    {
        private const string BaseCurrencyApiURL = "https://api.exchangeratesapi.io/latest?base=ILS&symbols=";
        private readonly SweetShopContext _context;

        public Currency CurrentCurrency { get; set; }

        public double CurrencyExchangeRate { get; set; }

        public List<OrderItem> Cart { get; set; }

        public CheckoutController(SweetShopContext context)
        {
            _context = context;
        }

        // GET: /<controller>/{choosenCurrency}
        public async Task<IActionResult> Checkout(List<string> invalidFieldsList, Currency choosenCurrency = Currency.ILS)
        {
            Cart = GetCartFormSession().ToList();
            await UpdateCurrency(choosenCurrency);
            ViewBag.CurrentCurrency = CurrentCurrency;
            ViewBag.Cart = Cart;
            ViewBag.ConvertToCurrentCurrency = new Func<double, double>(ConvertToCurrentCurrency);
            ViewBag.CartSum = ConvertToCurrentCurrency(GetCartSum());
            ViewBag.CartSize = Cart.Count;
            ViewBag.invalidFieldsList = invalidFieldsList;
            ViewBag.GetInputClass = new Func<string, string>(GetInputClass);
            return View();
        }

        [HttpPost]
        public IActionResult AddOrder(Order order)
        {
            Cart = GetCartFormSession().ToList();

            var invalidFields = new List<string>();

            if (order.Costumer.FirstName == null) invalidFields.Add("FirstName");

            if (order.Costumer.LastName == null) invalidFields.Add("LastName");

            if (order.Costumer.Email == null || !(new EmailAddressAttribute().IsValid(order.Costumer.Email))) invalidFields.Add("Email");

            if (order.Address == null) invalidFields.Add("Address");

            if (order.Zip == null || order.Zip?.Length != 7) invalidFields.Add("Zip");

            if (order.CCName == null) invalidFields.Add("CCName");

            if (order.CCNumber == null || order.CCNumber?.Length != 16) invalidFields.Add("CCNumber");

            if (order.CCExpiration == null || !Regex.IsMatch(order.CCExpiration, @"([0][1-9]|[1][0-2])/\d{2}")) invalidFields.Add("CCExpiration");

            if (order.CCCvv == null || order.CCCvv?.Length != 3) invalidFields.Add("CCCvv");

            if (invalidFields.Count != 0) return RedirectToAction("Checkout", new { invalidFieldsList = invalidFields, choosenCurrency = CurrentCurrency});

            order.OrderDate = DateTime.Today;
            order.Status = _context.OrderStatuses.Single(x => x.Name == "New");
            order.OrderItems = Cart;
            order.Costumer.Email = order.Costumer.Email.ToLower();

            var existingCostumer = _context.Costumers.SingleOrDefault(x => x.Email == order.Costumer.Email);

            if (existingCostumer != null)
            {
                order.Costumer = existingCostumer;
            }

            try
            {
                _context.Add(order);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View("OrderComplete");
        }

        public double GetCartSum()
        {
            return Cart.Sum(x => x.Product.Price * x.Quantity);
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

            return (double) returnVal["rates"][wantedCurrency.ToString()];
        }

        public async Task UpdateCurrency(Currency newCurrency)
        {
            CurrentCurrency = newCurrency;
            CurrencyExchangeRate = await GetCurrencyExchangeRate(newCurrency);
        }

        public string GetInputClass(string inputName)
        {
            var invalidFieldsList = (List<string>)ViewBag.invalidFieldsList;

            if (invalidFieldsList.Count == 0) return "form-control";

            return invalidFieldsList.Contains(inputName) ? "form-control is-invalid" : "form-control is-valid";
        }

        public IEnumerable<OrderItem> GetCartFormSession()
        {
            var cartIDs = HttpContext.Session.Keys.Where(id => int.TryParse(id, out var num)).Select(int.Parse);
            return _context.Products.Where(product => cartIDs.Contains(product.ID)).Include("Category")
                .Select(product => new OrderItem() {Product = product, Quantity = int.Parse(HttpContext.Session.GetString(product.ID.ToString()))});
        }
    }
}