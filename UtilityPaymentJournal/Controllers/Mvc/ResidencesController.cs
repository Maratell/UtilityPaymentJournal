using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO.Residences;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Residences;


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
