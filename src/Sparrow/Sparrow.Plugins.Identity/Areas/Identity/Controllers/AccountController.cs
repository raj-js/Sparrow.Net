using Microsoft.AspNetCore.Mvc;

namespace Sparrow.Plugins.Identity.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}