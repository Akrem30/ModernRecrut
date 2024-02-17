using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ModernRecrut.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AutorisationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
