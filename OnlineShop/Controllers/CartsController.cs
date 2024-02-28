using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public CartsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            db = context;

            _userManager = userManager;

            _roleManager = roleManager;
        }
        [Authorize(Roles ="Inregistrat")]
        public IActionResult Index()
        {

            if (User.IsInRole("Inregistrat"))
            {
                var carts = db.Carts
                                  .Include("CartProducts.Product.Category")
                                  .Include("CartProducts.Product.User")
                                  .Include("User")
                                  .Where(c => c.UserId == _userManager.GetUserId(User))
                                  .FirstOrDefault();
                ViewBag.CartProducts =carts.CartProducts;
                if (carts == null)
                {
                    TempData["message"] = "Nu aveti produse in cos";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Products");
                }

                return View(carts);
            }
            else
            {
                TempData["message"] = "Nu aveti drepturi";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Products");
            }
        }

        public IActionResult Show()
        {
            var cart = db.Carts
                         .Include("User")
                         .Where(c => c.UserId == _userManager.GetUserId(User))
                         .First();
            db.Carts.Remove(cart);

            db.SaveChanges();

            return RedirectToAction("Index", "Products");
        }
        [Authorize(Roles = "Inregistrat")]
        public IActionResult Edit(int id)
        {
            CartProduct cart = db.CartProducts.Include("Cart")
                                              .Include("Product")
                                              .Where(c => c.Cart.UserId == _userManager.GetUserId(User))
                                              .Where(c => c.ProductId ==id)
                                              .First();

            if (cart.Cart.UserId == _userManager.GetUserId(User))
            {
                return View(cart);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa editati produsul din cos";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Carts");
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, CartProduct requestproduct)
        {
            CartProduct cart = db.CartProducts.Include("Cart")
                                              .Include("Product")
                                              .Include("Cart.User")
                                              .Where(cp => cp.Cart.UserId == _userManager.GetUserId(User))
                                              .Where(c => c.ProductId == id)
                                              .First();

            if (ModelState.IsValid)
            {
                cart.Quantity = requestproduct.Quantity;

                db.SaveChanges();

                return Redirect("/Carts/Index");
            }
            else
            {
                return View(requestproduct);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Inregistrat")]
        public IActionResult Delete(int id)
        {
            CartProduct cart = db.CartProducts.Include("Cart")
                                               .Include("Product")
                                               .Where(c => c.Cart.UserId == _userManager.GetUserId(User))
                                               .Where(c => c.ProductId == id)
                                               .First();
            if (cart.Cart.UserId == _userManager.GetUserId(User))
            {
                db.CartProducts.Remove(cart);
                db.SaveChanges();
                return Redirect("/Carts/Index");
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti produsul din cos";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Carts");
            }
        }
    }
        

}
