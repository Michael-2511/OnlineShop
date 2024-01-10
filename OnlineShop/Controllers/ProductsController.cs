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
                                         .Include("User")
                                         .Include("Reviews")
                                         .Include("Reviews.User")
                                         .Where(prod => prod.Id == id)
                                         .First();
            float sum = 0, number_of_reviews = 0;
            foreach(var star in product.Reviews)
            {
                if(star.Stars!=null && star.Stars!=0)
                {
                    number_of_reviews++;
                    sum += star.Stars;
                }
            }
            if(number_of_reviews>0)
            {
                product.Rating = (float)Math.Round((sum / number_of_reviews), 2);
            }

            ViewBag.Product = product;
            ViewBag.Category = product.Category;
            SetAccessRights();
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.Alert = TempData["messageType"];
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Inregistrat,Colaborator,Administrator")]
        public IActionResult Show([FromForm] Review rev)
        {
            rev.Date = DateTime.Now;

            // preluam id-ul utilizatorului care posteaza comentariul
            rev.UserId = _userManager.GetUserId(User);

            if (ModelState.IsValid)
            {
                db.Reviews.Add(rev);
                db.SaveChanges();
                return Redirect("/Products/Show/" + rev.ProductId);
            }

            else
            {
                Product product = db.Products.Include("Category")
                                         .Include("User")
                                         .Include("Reviews")
                                         .Include("Reviews.User")
                                         .Where(art => art.Id == rev.ProductId)
                                         .First();


                SetAccessRights();

                return View(product);
            }
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

            //try
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost adaugat";
                return RedirectToAction("Index");
            }
            //catch(Exception)
            else
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
                {
                    product.Name = requestProduct.Name;
                    product.Price = requestProduct.Price;
                    product.Description = requestProduct.Description;
                    product.Category = requestProduct.Category;
                    product.CategoryId = requestProduct.CategoryId;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }

                catch (Exception)
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
        [Authorize(Roles = "Colaborator,Administrator")]
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Include("Reviews")
                                         .Where(art => art.Id == id)
                                         .First();

            if (product.UserId == _userManager.GetUserId(User) || User.IsInRole("Administrator"))
            {
                db.Products.Remove(product);
                db.SaveChanges();
                TempData["message"] = "Produsul a fost sters";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti un produs care nu va apartine";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
        }
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
    }
}