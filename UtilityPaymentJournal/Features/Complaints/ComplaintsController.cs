using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Features.Complaints.Models;
using UtilityPaymentJournal.Features.Complaints.Queries;

namespace UtilityPaymentJournal.Features.Complaints
{
    [Route("complaints")]
    public class ComplaintsController : Controller
    {
        private readonly IComplaintQueryService _complaintQueryService;

        public ComplaintsController(IComplaintQueryService complaintQueryService)
        {
            _complaintQueryService = complaintQueryService ?? throw new ArgumentNullException(nameof(complaintQueryService));
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            // 1. Извлекаем из сервиса чтения жалобы, сгруппированные по статусам внутри словаря
            Dictionary<ComplaintStatus, List<ComplaintViewModel>> complaintsByStatus 
                = await _complaintQueryService.GetComplaintsGroupedByStatusAsync(cancellationToken);

            // 2. Формируем модель представления доски
            ComplaintBoardViewModel boardViewModel = new ComplaintBoardViewModel
            {
                ComplaintsByStatus = complaintsByStatus,
                EmptyForm = new CreateComplaintViewModel() // Инициализируем форму для создания карточки
            };

            return View(boardViewModel);
        }
    }
}
