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
    public class ComplaintBoardController : Controller
    {
        private readonly IComplaintBoardService _complaintBoardService;
        private readonly IUtilityService _utilityService;
        private readonly IComplaintMapper _complaintMapper;
        private readonly IUtilityMapper _utilityMapper;

        public ComplaintBoardController(
            IComplaintBoardService complaintBoardService,
            IUtilityService utilityService,
            IComplaintMapper complaintMapper,
            IUtilityMapper utilityMapper)
        {
            _complaintBoardService = complaintBoardService;
            _utilityService = utilityService;

            _complaintMapper = complaintMapper;
            _utilityMapper = utilityMapper;
        }

        public async Task<IActionResult> GetView()
        {
            IEnumerable<ComplaintDTO> complaintDtos = await _complaintBoardService.GetAllAsync();
            IEnumerable<ComplaintViewModel> complaints = complaintDtos.Select(e => _complaintMapper.ToViewModel(e));

            IEnumerable<UtilityDTO> utilityDtos = await _utilityService.GetAllAsync();
            IEnumerable<UtilityViewModel> utilities = utilityDtos.Select(u => _utilityMapper.ToViewModel(u));

            var boardViewModel = new ComplaintBoardViewModel
            {
                NewComplaints = complaints.Where(c => c.Status == ComplaintStatus.New).ToList(),
                InProgressComplaints = complaints.Where(c => c.Status == ComplaintStatus.InProgress).ToList(),
                ResolvedComplaints = complaints.Where(c => c.Status == ComplaintStatus.Resolved).ToList(),

                AvailableUtilities = utilities.ToList()
            };

            return View("~/Views/ComplaintBoard/ComplaintBoard.cshtml", boardViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateComplaintViewModel vm)
        {
            CreateComplaintDTO dto = _complaintMapper.ToDto(vm);

            ComplaintDTO resultDto = await _complaintBoardService.CreateAsync(dto);

            ComplaintViewModel resultVm = _complaintMapper.ToViewModel(resultDto);

            return Json(new { success = true, data = resultVm });
        }

        [HttpGet("ComplaintBoard/Get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            IEnumerable<ComplaintDTO> entities = await _complaintBoardService.GetAllAsync();

            ComplaintDTO? entity = entities.FirstOrDefault(e => e.Id == id);
            if (entity == null)
            {
                return NotFound("Жалоба с таким ID не найдена.");
            }

            ComplaintViewModel resultVm = _complaintMapper.ToViewModel(entity);

            return Json(resultVm);
        }

        [HttpPost("ComplaintBoard/Edit/{id}")]
        public async Task<IActionResult> Edit(long id, [FromBody] EditComplaintViewModel vm)
        {
            EditComplaintDTO dto = _complaintMapper.ToDto(vm);

            ComplaintDTO resultDto = await _complaintBoardService.EditAsync(id, dto);

            ComplaintViewModel resultVm = _complaintMapper.ToViewModel(resultDto);

            return Json(new { success = true, data = resultVm });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(long id, int status)
        {
            var currentComplaints = await _complaintBoardService.GetAllAsync();
            var current = currentComplaints.FirstOrDefault(c => c.Id == id);
            if (current == null) 
                return NotFound();

            var editDto = new EditComplaintDTO
            {
                Title = current.Title,
                Description = current.Description,
                UtilityId = current.UtilityId,
                Status = (ComplaintStatus)status,

                CreatedAt = current.CreatedAt,
                SubmissionDate = current.SubmissionDate,
                IssueResolutionDate = current.IssueResolutionDate
            };

            await _complaintBoardService.EditAsync(id, editDto);
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<ComplaintDTO> entities = await _complaintBoardService.GetAllAsync();

            IEnumerable<ComplaintViewModel> result = entities.Select(e => _complaintMapper.ToViewModel(e));

            return Json(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(long id)
        {
            await _complaintBoardService.DeleteAsync(id);

            return Ok(new { success = true, message = "Запись успешно удалена" });
        }
    }
}
