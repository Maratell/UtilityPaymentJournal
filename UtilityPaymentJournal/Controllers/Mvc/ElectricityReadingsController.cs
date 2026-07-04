using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO.ElectricityReadings;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.ElectricityReadings;

namespace UtilityPaymentJournal.Controllers.Mvc
{
    [Route("electricity-readings")]
    public class ElectricityReadingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
