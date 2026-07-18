using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.Complaints.Commands;
using UtilityPaymentJournal.Features.Complaints.Models;
using UtilityPaymentJournal.Features.Complaints.Queries;
using UtilityPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Controllers.Api
{
    /// <summary>
    /// АПИ-контроллер для управления жалобами.
    /// </summary>
    [ApiController]
    [Route("api/complaints")]
    public class ComplaintsApiController : ControllerBase
    {
        private readonly IComplaintQueryService _queryService;
        private readonly IComplaintCommandService _commandService;
        private readonly IComplaintMapper _complaintMapper;
        public ComplaintsApiController(
            IComplaintQueryService queryService,
            IComplaintCommandService commandService,
            IComplaintMapper complaintMapper)
        {
            _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
            _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            _complaintMapper = complaintMapper ?? throw new ArgumentNullException(nameof(complaintMapper));
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ComplaintDetailsViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            IReadOnlyCollection<ComplaintQueryResultDto> dtos = await _queryService.GetAllAsync(cancellationToken);
            ComplaintDetailsViewModel[] viewModels = dtos
                .Select(_complaintMapper.ToDetailsViewModel)
                .ToArray();

            return Ok(viewModels);
        }
        /// <summary>
        /// Получить развернутые детали жалобы по её уникальному идентификатору.
        /// </summary>
        [HttpGet("{id:long}")]
        public async Task<ActionResult<ComplaintDetailsViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ComplaintQueryResultDto dto = await _queryService.GetByIdAsync(id, cancellationToken);
            ComplaintDetailsViewModel viewModel = _complaintMapper.ToDetailsViewModel(dto);

            return Ok(viewModel);
        }
        /// <summary>
        /// Создать новую запись жалобы.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ComplaintCreatedViewModel>> Create([FromBody] CreateComplaintViewModel createViewModel, CancellationToken cancellationToken)
        {
            CreateComplaintDto createDto = _complaintMapper.ToDto(createViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ComplaintCommandResultDto createdDto = await _commandService.CreateAsync(createDto, cancellationToken);
            ComplaintCreatedViewModel resultViewModel = _complaintMapper.ToCreatedViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = resultViewModel.Id }, resultViewModel);
        }
        /// <summary>
        /// Отредактировать существующие данные жалобы.
        /// </summary>
        [HttpPut("{id:long}")]
        public async Task<ActionResult<ComplaintUpdatedViewModel>> Edit([FromRoute] long id, [FromBody] EditComplaintViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditComplaintDto editDto = _complaintMapper.ToDto(editViewModel);
            // При отсутствии объекта сервис выбросит KeyNotFoundException (обработается в NotFoundExceptionHandler)
            ComplaintCommandResultDto updatedDto = await _commandService.EditAsync(id, editDto, cancellationToken);
            ComplaintUpdatedViewModel resultViewModel = _complaintMapper.ToUpdatedViewModel(updatedDto);

            return Ok(resultViewModel);
        }
        /// <summary>
        /// Точечно изменить статус существующей жалобы.
        /// </summary>
        [HttpPatch("change-status")]
        public async Task<ActionResult<ComplaintUpdatedViewModel>> ChangeStatus([FromBody] ChangeComplaintStatusViewModel changeStatusViewModel, CancellationToken cancellationToken)
        {
            ChangeComplaintStatusDto changeStatusDto = _complaintMapper.ToDto(changeStatusViewModel);
            ComplaintCommandResultDto updatedDto = await _commandService.ChangeStatusAsync(changeStatusDto, cancellationToken);
            ComplaintUpdatedViewModel resultViewModel = _complaintMapper.ToUpdatedViewModel(updatedDto);

            return Ok(resultViewModel);
        }
        /// <summary>
        /// Удалить запись жалобы из системы.
        /// </summary>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            await _commandService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
