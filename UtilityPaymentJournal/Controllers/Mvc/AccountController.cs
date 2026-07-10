using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Controllers.Mvc
{
    [AllowAnonymous] // Разрешает доступ неавторизованным гостям
    [Route("account")]
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}
