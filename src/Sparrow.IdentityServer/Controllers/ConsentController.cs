using Microsoft.AspNetCore.Mvc;

namespace Sparrow.IdentityServer.Controllers
{
    public class ConsentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}