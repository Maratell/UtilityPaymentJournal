using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Features.Users
{
    [AllowAnonymous] // Разрешает доступ неавторизованным гостям
    [Route("admin")]
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
