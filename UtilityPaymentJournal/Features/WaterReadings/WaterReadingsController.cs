using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Features.WaterReadings
{
    [Route("water-readings")]
    public class WaterReadingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
