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
        public class CostumerViewModel
        {
            public int ID { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public int OrdersNumber { get; set; }
        }

        private readonly SweetShopContext _context;
        private readonly IFileProvider fileProvider;
        private readonly IHostingEnvironment hostingEnvironment;

        public AdminController(SweetShopContext context, IFileProvider fileprovider, IHostingEnvironment env)
        {
            _context = context;
            fileProvider = fileprovider;
            hostingEnvironment = env;
        }

        #region Orders

        public IActionResult Orders()
        {
            if(HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }
            List<Order> orders = _context.Orders.ToList();
            List<User> users = _context.Users.ToList();
            List<OrderStatus> statuses = _context.OrderStatuses.ToList();
            ViewBag.Orders = orders;
            ViewBag.statuses = statuses;

            return View();
        }

        [HttpPost]
        public IActionResult Orders(int orderId, int orderStatus, DateTime? orderDate)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }
            List<Order> orders;

            if (orderId != 0)
            {
                orders = _context.Orders.Where((order) => order.Id == orderId).ToList();
            }
            else
            {
                orders = _context.Orders.Where((order) => (orderStatus != 0 && orderDate != null && orderStatus == order.Status.ID) ||
                        (orderStatus != 0 && orderStatus == order.Status.ID) ||
                        (orderDate != null && orderDate.Equals(order.OrderDate))).ToList();
            }

            List<OrderStatus> statuses = _context.OrderStatuses.ToList();
            List<User> users = _context.Users.ToList();

            ViewBag.Orders = orders;
            ViewBag.statuses = statuses;

            return View();
        }

        #endregion

        #region Categories

        public IActionResult Categories()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }
            List<Category> categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }

        public IActionResult RemoveCategory(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }
            _context.Remove(_context.Categories.Single(c => c.ID == id));

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                TempData["CategoryRemovalFailed"] = true;
            }

            return Redirect("/Admin/Categories");
        }

        public IActionResult AddCategory(string name)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }
            Category newCategory = new Category() { Name = name };

            _context.Add(newCategory);
            _context.SaveChanges();
            return Redirect("/Admin/Categories");
        }

        public IActionResult EditCategory(int id, string name)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }
            Category categoryToEdit = _context.Categories.Single(c => c.ID == id);
            categoryToEdit.Name = name;

            _context.Update(categoryToEdit);
            _context.SaveChanges();
            return Redirect("/Admin/Categories");
        }
        #endregion

        public IActionResult Welcome()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }
            ViewBag.Name = HttpContext.Session.GetString("fullName");
            return View();
        }


        #region Products

        public IActionResult EditProducts()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }

            List<Product> products = _context.Products.ToList();
            List<Category> categories = _context.Categories.ToList();

            ViewBag.Products = products;
            ViewBag.Categories = categories;
            return View();
        }

        public IActionResult RemoveProduct(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }

            _context.Remove(_context.Products.Single(p => p.ID == id));

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                TempData["ProductRemovalFailed"] = true;
            }

            return Redirect("/Admin/EditProducts");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(string name, int category, int price, IFormFile img)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }

            string pathToSave = await SaveImageFile(img, name);

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
        public async Task<IActionResult> EditProduct(int id,
                                                     string name,
                                                     int category,
                                                     int price,
                                                     IFormFile img)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }

            Product productToEdit = _context.Products.Single(p => p.ID == id);
            productToEdit.Name = name;
            productToEdit.Category = _context.Categories.Single(c => c.ID == category);
            productToEdit.Price = price;

            if (img != null)
            {
                productToEdit.ImgPath = await SaveImageFile(img, name);
            }

            _context.Update(productToEdit);
            await _context.SaveChangesAsync();
            return Redirect("/Admin/EditProducts");
        }

        #endregion

        #region Branches

        public IActionResult Branches()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }
            List<Branch> branches = _context.Branches.ToList();
            ViewBag.Branches = branches;
            return View();
        }

        public IActionResult RemoveBranch(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }
            _context.Remove(_context.Branches.Single(b => b.ID == id));
            _context.SaveChanges();
            return Redirect("/Admin/Branches");
        }

        public IActionResult EditBranch(int id, string name, string address, float x, float y)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }
            Branch branchToEdit = _context.Branches.Single(b => b.ID == id);

            branchToEdit.branchName = name;
            branchToEdit.addressInfo = address;
            branchToEdit.locationX = x;
            branchToEdit.locationY = y;

            _context.Update(branchToEdit);
            _context.SaveChanges();
            return Redirect("/Admin/Branches");
        }

        public IActionResult AddBranch(int id, string name, string address, float x, float y)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }
            Branch branchToEdit = new Branch()
            {
                branchName = name,
                addressInfo = address,
                locationX = x,
                locationY = y
            };

            _context.Add(branchToEdit);
            _context.SaveChanges();
            return Redirect("/Admin/Branches");
        }

        #endregion

        public IActionResult Costumers()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }

            List<User> costumers = _context.Users.ToList();
            List<Order> orders = _context.Orders.ToList();
            List<CostumerViewModel> costumerView = orders.GroupBy(g => g.User.Id)
                .Select(g =>
                {
                    User costumer = costumers.Single(c => c.Id == g.Key);
                    return new CostumerViewModel
                    {
                        ID = g.Key,
                        FullName = costumer.FullName,
                        Email = costumer.Email,
                        OrdersNumber = g.Count()
                    };
                }).ToList();

            ViewBag.CostumersView = costumerView;
            return View();
        }

        public IActionResult Statistics()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return View("Views/Users/NotFound.cshtml");
            }

            return View();
        }

        private async Task<string> SaveImageFile(IFormFile img, string name)
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

            return pathToSave;
        }
    }
}
