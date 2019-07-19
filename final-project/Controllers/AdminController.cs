using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using final_project.Models;
using final_project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;

namespace final_project.Controllers
{
    public class AdminController : Controller
    {
        private readonly SweetShopContext _context;
        private readonly IFileProvider fileProvider;
        private readonly IHostingEnvironment hostingEnvironment;

        public AdminController(SweetShopContext context, IFileProvider fileprovider, IHostingEnvironment env)
        {
            _context = context;
            fileProvider = fileprovider;
            hostingEnvironment = env;
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
        public async Task<IActionResult> AddProduct(string name, int category, int price, IFormFile img)
        {
            // Create a File Info 
            FileInfo fi = new FileInfo(img.FileName);

            // This code creates a unique file name to prevent duplications 
            // stored at the file location
            var newFilename = name + "_" + String.Format("{0:d}",
                              (DateTime.Now.Ticks / 10) % 100000000) + fi.Extension;
            var webPath = hostingEnvironment.WebRootPath;
            var path = Path.Combine("", webPath + @"\ImageFiles\" + newFilename);

            // IMPORTANT: The pathToSave variable will be save on the column in the database
            var pathToSave = @"/ImageFiles/" + newFilename;

            // This stream the physical file to the allocate wwwroot/ImageFiles folder
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await img.CopyToAsync(stream);
            }

            Product newProduct = new Product()
            {
                Name = name,
                Category = _context.Categories.Single(c => c.ID == category),
                Price = price,
                ImgPath = pathToSave
            };

            _context.Add(newProduct);
            await _context.SaveChangesAsync();
            return Redirect("/Admin/EditProducts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProduct(int id, string name, int category, int price)
        {
            Product newProduct = new Product()
            {
                ID = id,
                Name = name,
                Category = _context.Categories.Single(c => c.ID == category),
                Price = price,

            };

            _context.Update(newProduct);
            _context.SaveChanges();
            return Redirect("/Admin/EditProducts");
        }
    }
}
