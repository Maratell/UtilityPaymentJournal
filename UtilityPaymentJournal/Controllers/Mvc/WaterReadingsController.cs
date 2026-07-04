using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Controllers.Mvc
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
