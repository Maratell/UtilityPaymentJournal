using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Controllers.Mvc
{
    [Route("admin")]
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
