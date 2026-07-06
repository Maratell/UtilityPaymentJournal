using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.DTO.ComplaintBoard;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.ComplaintBoard;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/complaints")]
    public class ComplaintsApiController : ControllerBase
    {
        private readonly IComplaintService _complaintService;
        private readonly IComplaintMapper _complaintMapper;

        public ComplaintsApiController(
            IComplaintService complaintService,
            IComplaintMapper complaintMapper)
        {
            _complaintService = complaintService;
            _complaintMapper = complaintMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ComplaintViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<ComplaintDTO> dtos = await _complaintService.GetAllAsync(cancellationToken);

            IEnumerable<ComplaintViewModel> viewModels = dtos
                .Select(e => _complaintMapper.ToViewModel(e))
                .ToList();

            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ComplaintViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            ComplaintDTO? dto = await _complaintService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
            {
                return NotFound($"Жалоба с ID {id} не найдена.");
            }

            ComplaintViewModel viewModel = _complaintMapper.ToViewModel(dto);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult<ComplaintViewModel>> Create([FromBody] CreateComplaintViewModel createViewModel, CancellationToken cancellationToken)
        {
            CreateComplaintDTO createDto = _complaintMapper.ToDto(createViewModel);

            ComplaintDTO createdDto = await _complaintService.CreateAsync(createDto, cancellationToken);

            ComplaintViewModel createdViewModel = _complaintMapper.ToViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] EditComplaintViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditComplaintDTO editDto = _complaintMapper.ToDto(editViewModel);

            ComplaintDTO? updatedDto = await _complaintService.EditAsync(id, editDto, cancellationToken);
            if (updatedDto is null)
            {
                return NotFound($"Жалоба с ID {id} не найдена.");
            }

            ComplaintViewModel updatedViewModel = _complaintMapper.ToViewModel(updatedDto);

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
            ComplaintDTO? dto = await _complaintService.GetByIdAsync(id, cancellationToken);
            if (dto == null)
                return NotFound($"Жалоба с ID {id} не найдена.");

            EditComplaintDTO editDto = _complaintMapper.ToDto(dto, (ComplaintStatus)status);
            ComplaintDTO? updatedDto = await _complaintService.EditAsync(id, editDto, cancellationToken);
            if (updatedDto is null)
            {
                return NotFound($"Не удалось обновить статус. Жалоба с ID {id} не найдена.");
            }

            // Маппим обновленный DTO в ViewModel и возвращаем 200 OK
            ComplaintViewModel updatedViewModel = _complaintMapper.ToViewModel(updatedDto);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            bool isDeleted = await _complaintService.DeleteAsync(id, cancellationToken);
            if (!isDeleted)
            {
                return NotFound($"Не удалось удалить. Жалоба с ID {id} не найдена.");
            }

            return NoContent();
        }
    }
}
