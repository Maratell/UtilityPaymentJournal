using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Controllers.Mvc
{
    [Route("electricity-readings")]
    public class ElectricityReadingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
