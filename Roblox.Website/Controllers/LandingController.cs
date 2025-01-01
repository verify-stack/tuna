using Microsoft.AspNetCore.Mvc;

namespace Roblox.Website.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Animated");
        }

        public IActionResult Animated()
        {
            return View();
        }
    }
}
