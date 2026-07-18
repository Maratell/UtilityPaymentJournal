using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Features.ElectricityReadings
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
