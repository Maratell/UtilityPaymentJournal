using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO.Utilities;
using UtilityPaymentJournal.DTO.UtilityProviders;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Utilities;
using UtilityPaymentJournal.Models.UtilityProviders;
using UtilityProviderPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Controllers
{
    [Route("utility-provider")]
    public class UtilityProviderController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
