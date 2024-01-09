using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Controllers
{
    public class CartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
