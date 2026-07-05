using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UtilityPaymentJournal.DTO.ComplaintBoard;
using UtilityPaymentJournal.DTO.Utilities;
using UtilityPaymentJournal.Enumerations;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.ComplaintBoard;
using UtilityPaymentJournal.Models.Utilities;
using UtilityPaymentJournal.Services;


namespace UtilityPaymentJournal.Controllers
{
    [Route("complaints")]
    public class ComplaintsController : Controller
    {
        private readonly IComplaintService _complaintService; 
        private readonly IUtilityService _utilityService;
        private readonly IComplaintMapper _complaintMapper;
        private readonly IUtilityMapper _utilityMapper;

        public ComplaintsController(
            IComplaintService complaintService, 
            IUtilityService utilityService,
            IComplaintMapper complaintMapper,
            IUtilityMapper utilityMapper)
        {
            _complaintService = complaintService;
            _utilityService = utilityService;
            _complaintMapper = complaintMapper;
            _utilityMapper = utilityMapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            // Загружаем данные из бд
            IReadOnlyCollection<ComplaintDTO> complaintDtos = await _complaintService.GetAllAsync(cancellationToken);
            IReadOnlyCollection<UtilityDTO> utilityDtos = await _utilityService.GetAllAsync(cancellationToken);

            // Маппим результаты
            IEnumerable<ComplaintViewModel> complaints = complaintDtos.Select(_complaintMapper.ToViewModel);
            List<UtilityViewModel> utilities = utilityDtos.Select(_utilityMapper.ToViewModel).ToList();

            ComplaintBoardViewModel boardViewModel = new ComplaintBoardViewModel { AvailableUtilities = utilities };

            // Разделяем жалобы по статусам за один проход
            foreach (ComplaintViewModel complaint in complaints)
            {
                switch (complaint.Status)
                {
                    case ComplaintStatus.New:
                        boardViewModel.NewComplaints.Add(complaint);
                        break;
                    case ComplaintStatus.InProgress:
                        boardViewModel.InProgressComplaints.Add(complaint);
                        break;
                    case ComplaintStatus.Resolved:
                        boardViewModel.ResolvedComplaints.Add(complaint);
                        break;
                }
            }

            return View(boardViewModel);
        }
    }
}
