using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace final_project.Controllers
{
    public class CartController : Controller
    {
        // GET: /<controller>/
        public IActionResult Cart()
        {
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
    }
}
