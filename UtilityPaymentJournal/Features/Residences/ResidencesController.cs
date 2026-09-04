using Microsoft.AspNetCore.Mvc;


namespace UtilityPaymentJournal.Features.Residences
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("residences")]
    public class ResidencesController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
