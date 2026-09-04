using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Features.ElectricityReadings
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("electricity-readings")]
    public class ElectricityReadingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
