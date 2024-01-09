using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Data;

namespace OnlineShop.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ReviewsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager
        )
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        //[Authorize(Roles = "Inregistrat,Colaborator,Administrator")]
        //[HttpPost]
        //public IActionResult New(Review rev)
        //{
        //    rev.Date = DateTime.Now;
        //    rev.UserId =_userManager.GetUserId(User);

        //    if (ModelState.IsValid)
        //    {
        //        db.Reviews.Add(rev);
        //        db.SaveChanges();
        //        return Redirect("/Products/Show/" + rev.ProductId);
        //    }

        //    else
        //    {
        //        return Redirect("/Products/Show/" + rev.ProductId);
        //    }

        //}

        // Stergerea unui comentariu asociat unui articol din baza de date
        [HttpPost]
        [Authorize(Roles = "Inregistrat,Colaborator,Administrator")]
        public IActionResult Delete(int id)
        {
            Review rev = db.Reviews.Find(id);
            if (rev.UserId == _userManager.GetUserId(User) || User.IsInRole("Administrotor"))
            {
                db.Reviews.Remove(rev);
                db.SaveChanges();
                return Redirect("/Products/Show/" + rev.ProductId);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti comentariul";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Products");
            }
        }

        // In acest moment vom implementa editarea intr-o pagina View separata
        // Se editeaza un comentariu existent
        [Authorize(Roles = "Inregistrat,Colaborator,Administrator")]
        public IActionResult Edit(int id)
        {
            Review rev = db.Reviews.Find(id);
            if (rev.UserId == _userManager.GetUserId(User) || User.IsInRole("Administrator"))
            {
                return View(rev);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa editati comentariul";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Products");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Inregistrat,Colaborator,Administrator")]
        public IActionResult Edit(int id, Review requestReview)
        {
            Review rev = db.Reviews.Find(id);
            if (rev.UserId == _userManager.GetUserId(User) || User.IsInRole("Administrator"))
            { 
                if (ModelState.IsValid)
                {

                    rev.Content = requestReview.Content;

                    db.SaveChanges();

                    return Redirect("/Products/Show/" + rev.ProductId);
                }
                else
                {
                    return Redirect("/Products/Show/" + rev.ProductId);
                }
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Products");
            }
        }
    }
}
