using Microsoft.AspNetCore.Mvc;

namespace Phone_mvc.Controllers
{
    public class AccessDeniedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
