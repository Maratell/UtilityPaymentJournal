using Microsoft.AspNetCore.Mvc;


namespace UtilityPaymentJournal.Features.Residences
{
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
