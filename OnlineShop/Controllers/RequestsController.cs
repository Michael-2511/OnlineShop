using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RequestsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var isAdmin = User.IsInRole("Administrator");

            // If the user is an Administrator, fetch all requests; otherwise, fetch only the user's requests
            // db.Requests.Include("User") -> daca nu pun Include("User") si aici, adminul nu va putea vedea
            //                              numele utilizatorilor care au postat cererile si primesc eroare
            var requests = isAdmin ? db.Requests.Include("User") : db.Requests.Where(req => req.UserId == userId).Include("User");

            ViewBag.Requests = requests;
            // ViewBag.Requests = requests.ToList();  // Materialize the query to avoid issues with lazy loading

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.Alert = TempData["messageType"];
            }

            return View();
        }

        public IActionResult New()
        {
            var categories = from categ in db.Categories
                             select categ;

            ViewBag.Categories = categories;

            return View();
        }

        // Formular cerere adaugare produs
        [Authorize(Roles = "Colaborator")]
        [HttpPost]
        public IActionResult New(Request request)
        {
            request.UserId = _userManager.GetUserId(User);
            request.Status = "In asteptare";

            // if (ModelState.IsValid)
            try
            {
                db.Requests.Add(request);
                db.SaveChanges();
                TempData["message"] = "Cererea a fost trimisa";
                return RedirectToAction("Index");
            }
            //else
            catch (Exception)
            {
                return RedirectToAction("New");
            }
        }

        [Authorize(Roles = "Colaborator, Administrator")]
        public IActionResult Show(int id)
        {
            Request request = db.Requests.Include("Category")
                                        .Where(req => req.Id == id)
                                        .First();

            ViewBag.Request = request;
            ViewBag.Category = request.Category;


            //List<Product> products = db.Products.Where(prod => prod.RequestId == id)
            //                                    .ToList();
            //ViewBag.Products = products;

            SetAccessRights();

            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
                ViewBag.Alert = TempData["messageType"];
            }

            return View();
        }

        [Authorize(Roles = "Colaborator, Administrator")]
        public IActionResult Edit(int id)
        {
            Request request = db.Requests.Include("Category")
                            .Where(req => req.Id == id)
                            .First();

            ViewBag.Request = request;
            ViewBag.Category = request.Category;

            var categories = from categ in db.Categories
                             select categ;

            ViewBag.Categories = categories;

            if (request.UserId == _userManager.GetUserId(User) ||
                User.IsInRole("Administrator"))
            {
                TempData["message"] = "Produsul a fost editat";
                // return View();
                return View(request);
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
        public IActionResult Edit(int id, Request requestReq)
        {
            Request request = db.Requests.Find(id);

            if (request.UserId == _userManager.GetUserId(User) ||
                User.IsInRole("Administrator"))
            {
                try
                {
                    request.Description = requestReq.Description;
                    request.ProductName = requestReq.ProductName;
                    request.ProductDescription = requestReq.ProductDescription;
                    request.ProductPrice = requestReq.ProductPrice;
                    request.ProductPicture = requestReq.ProductPicture;
                    request.Category = requestReq.Category;
                    request.CategoryId = requestReq.CategoryId;

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
        [Authorize(Roles = "Administrator")]
        public IActionResult Accept(int id)
        {
            var request = db.Requests.Find(id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = "Acceptat";

            var product = new Product
            {
                Name = request.ProductName,
                Description = request.ProductDescription,
                Price = request.ProductPrice,
                Picture = "/images/"+request.ProductPicture,
                CategoryId = request.CategoryId,
                UserId = request.UserId
            };

            db.Products.Add(product);

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult Reject(int id)
        {
            var request = db.Requests.Find(id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = "Respins";
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Colaborator, Administrator")]
        public IActionResult Delete(int id)
        {
            Request request = db.Requests.Find(id);

            if (request.UserId == _userManager.GetUserId(User) ||
                User.IsInRole("Administrator"))
            {
                db.Requests.Remove(request);
                db.SaveChanges();
                TempData["message"] = "Cererea a fost stearsa";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti aceasta cerere.";
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