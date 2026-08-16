using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Features.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
