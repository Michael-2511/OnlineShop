using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{

    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ProductsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // [Authorize(Roles = "Inregistrat,Colaborator,Administrator")]
        public IActionResult Index()
        {
            var products = db.Products.Include("Category").Include("User");

            //var products = from product in db.Products
            //               orderby product.Name
            //               select product;

            ViewBag.Products = products;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.Alert = TempData["messageType"];
            }

            return View();
        }
        public IActionResult Show(int id)
        {
            Product product = db.Products.Include("Category")
                                         .Include("Reviews")
                                         .Where(prod => prod.Id == id)
                                         .First();

            ViewBag.Product = product;
            ViewBag.Category = product.Category;

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.Alert = TempData["messageType"];
            }

            return View();
        }

        // Conditiile de afisare a butoanelor de editare si stergere
        private void SetAccessRights()
        {
            ViewBag.AfisareButoane = false;

            if (User.IsInRole("Colaborator"))
            {
                ViewBag.AfisareButoane = true;
            }

            ViewBag.EsteAdmin = User.IsInRole("Administrator");

            ViewBag.UserCurent = _userManager.GetUserId(User);
        }


        // Se afiseaza formularul in care se vor completa datele unui produs
        // impreuna cu selectarea categoriei din care face parte
        // HttpGet implicit
        public IActionResult New()
        {
            var categories = from categ in db.Categories
                             select categ;

            ViewBag.Categories = categories;

            return View();
        }
        // Se adauga articolul in baza de date
        [Authorize(Roles = "Colaborator, Administrator")]
        [HttpPost]
        public IActionResult New(Product product)
        {
            product.UserId = _userManager.GetUserId(User);

            try
            //if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost adaugat";
                return RedirectToAction("Index");
            }
            catch(Exception)
            //else
            {
                return RedirectToAction("New");
            }
        }

        [Authorize(Roles = "Colaborator, Administrator")]
        public IActionResult Edit(int id)
        {
            Product products = db.Products.Include("Category")
                                            .Where(prd => prd.Id == id)
                                            .First();
            ViewBag.Product = products;
            ViewBag.Category = products.Category;

            var categories = from categ in db.Categories
                             select categ;

            ViewBag.Categories = categories;

            if (products.UserId == _userManager.GetUserId(User) ||
                User.IsInRole("Administrator"))
            {
                TempData["message"] = "Produsul a fost editat";

                return View(products);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa editati comentariul";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Products");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Colaborator, Administrator")]
        public IActionResult Edit(int id, Product requestProduct)
        {
            Product product = db.Products.Find(id);

            if (product.UserId == _userManager.GetUserId(User) ||
                User.IsInRole("Administrator"))
            {
                try
                //if (ModelState.IsValid)
                {
                    product.Name = requestProduct.Name;
                    product.Description = requestProduct.Description;
                    product.Price = requestProduct.Price;
                    product.Picture = requestProduct.Picture;
                    product.Category = requestProduct.Category;
                    product.CategoryId = requestProduct.CategoryId;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                catch (Exception)
                // else 
                {
                    return RedirectToAction("Edit", id);
                }
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Products");
            }


        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);

            if (product.UserId == _userManager.GetUserId(User) ||
                User.IsInRole("Administrator"))
            {
                db.Products.Remove(product);
                db.SaveChanges();

                return RedirectToAction("Index");

            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti acest produs";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Products");

            }
        }
    }
}