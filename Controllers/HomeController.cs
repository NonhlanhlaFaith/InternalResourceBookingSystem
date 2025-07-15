using Microsoft.AspNetCore.Mvc;

namespace InternalResourceBookingSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
