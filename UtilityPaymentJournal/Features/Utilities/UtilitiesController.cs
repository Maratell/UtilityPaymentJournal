using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Features.Utilities
{
    [Route("utilities")]
    public class UtilitiesController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
