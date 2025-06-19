using Microsoft.AspNetCore.Mvc;

namespace Phone_mvc.Controllers
{
    public class BrandController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
