using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Controllers.Mvc
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
