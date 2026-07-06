using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Controllers.Mvc
{
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
