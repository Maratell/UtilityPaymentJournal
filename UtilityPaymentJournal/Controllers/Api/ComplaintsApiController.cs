using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.DTOs.ComplaintBoard;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.ComplaintBoard;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/complaints")]
    public partial class ComplaintsApiController : ControllerBase
    {
        private readonly IComplaintService _complaintService;
        private readonly IComplaintMapper _complaintMapper;
        private readonly ILogger<ComplaintsApiController> _logger;

        public ComplaintsApiController(
            IComplaintService complaintService,
            IComplaintMapper complaintMapper,
            ILogger<ComplaintsApiController> logger)
        {
            _complaintService = complaintService ?? throw new ArgumentNullException(nameof(complaintService));
            _complaintMapper = complaintMapper ?? throw new ArgumentNullException(nameof(complaintMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ComplaintViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            LogFetchingAllComplaints(_logger);

            IEnumerable<ComplaintDto> dtos = await _complaintService.GetAllAsync(cancellationToken);
            ComplaintViewModel[] viewModels = dtos
                .Select(e => _complaintMapper.ToViewModel(e))
                .ToArray();

            LogFetchedAllComplaintsCount(_logger, viewModels.Length);
            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ComplaintViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ComplaintDto dto = await _complaintService.GetByIdAsync(id, cancellationToken);
            ComplaintViewModel viewModel = _complaintMapper.ToViewModel(dto);

            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult<ComplaintViewModel>> Create([FromBody] CreateComplaintViewModel createViewModel, CancellationToken cancellationToken)
        {
            LogComplaintCreationRequested(_logger, createViewModel.UtilityId, createViewModel.Title);

            CreateComplaintDto createDto = _complaintMapper.ToDto(createViewModel);
            ComplaintDto createdDto = await _complaintService.CreateAsync(createDto, cancellationToken);
            ComplaintViewModel createdViewModel = _complaintMapper.ToViewModel(createdDto);

            LogComplaintCreated(_logger, createdViewModel.Id);
            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ComplaintViewModel>> Edit([FromRoute] long id, [FromBody] EditComplaintViewModel editViewModel, CancellationToken cancellationToken)
        {
            LogComplaintUpdateRequested(_logger, id, editViewModel.UtilityId, editViewModel.Title);

            EditComplaintDto editDto = _complaintMapper.ToDto(editViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ComplaintDto updatedDto = await _complaintService.EditAsync(id, editDto, cancellationToken);
            ComplaintViewModel updatedViewModel = _complaintMapper.ToViewModel(updatedDto);

            LogComplaintUpdated(_logger, id);
            return Ok(updatedViewModel);
        }

        /// <summary>
        /// Возвращаю ComplaintViewModel при обновлении статуса с кодом 200ок для удобства тестирвоания
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch("{id:long}/status/{status:int}")]
        public async Task<ActionResult<ComplaintViewModel>> UpdateStatus([FromRoute] long id, [FromRoute] int status, CancellationToken cancellationToken)
        {
            LogComplaintStatusUpdateRequested(_logger, id, status);

            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ComplaintDto currentDto = await _complaintService.GetByIdAsync(id, cancellationToken);
            EditComplaintDto editDto = _complaintMapper.ToDto(currentDto, (ComplaintStatus)status);

            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ComplaintDto updatedDto = await _complaintService.EditAsync(id, editDto, cancellationToken);
            ComplaintViewModel updatedViewModel = _complaintMapper.ToViewModel(updatedDto);

            LogComplaintStatusUpdated(_logger, id, status);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            LogComplaintDeletionRequested(_logger, id);

            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            await _complaintService.DeleteAsync(id, cancellationToken);

            LogComplaintDeleted(_logger, id);
            return NoContent();
        }
    }
}
