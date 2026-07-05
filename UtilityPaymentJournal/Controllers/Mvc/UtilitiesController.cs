using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Controllers.Mvc
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
