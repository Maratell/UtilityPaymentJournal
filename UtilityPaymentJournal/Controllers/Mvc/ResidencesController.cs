using Microsoft.AspNetCore.Mvc;


namespace UtilityPaymentJournal.Controllers.Mvc
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
