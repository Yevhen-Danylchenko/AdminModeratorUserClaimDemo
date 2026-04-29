using Microsoft.AspNetCore.Mvc;

namespace AdminModeratorUserClaimDemo.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
