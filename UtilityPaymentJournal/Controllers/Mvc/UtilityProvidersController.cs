using Microsoft.AspNetCore.Mvc;


namespace UtilityPaymentJournal.Controllers
{
    [Route("utility-providers")]
    public class UtilityProvidersController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
