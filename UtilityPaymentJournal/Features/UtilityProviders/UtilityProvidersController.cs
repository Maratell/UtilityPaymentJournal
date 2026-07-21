using Microsoft.AspNetCore.Mvc;


namespace UtilityPaymentJournal.Features.UtilityProviders
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
