using Microsoft.AspNetCore.Mvc;

namespace Phone_mvc.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
