using Microsoft.AspNetCore.Mvc;

namespace Andoromeda.Kyubey.Incubator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
