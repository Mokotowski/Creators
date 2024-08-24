using Microsoft.AspNetCore.Mvc;

namespace Creators.Creators.Controllers
{
    public class CreatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
