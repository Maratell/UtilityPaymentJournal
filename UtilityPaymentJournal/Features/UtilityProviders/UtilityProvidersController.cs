using Microsoft.AspNetCore.Mvc;


namespace UtilityPaymentJournal.Features.UtilityProviders
{
    [ApiExplorerSettings(IgnoreApi = true)]
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
