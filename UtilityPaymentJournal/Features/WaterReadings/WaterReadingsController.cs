using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Features.WaterReadings
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("water-readings")]
    public class WaterReadingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
