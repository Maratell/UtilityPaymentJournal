using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Features.Utilities
{
    [ApiExplorerSettings(IgnoreApi = true)]
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
