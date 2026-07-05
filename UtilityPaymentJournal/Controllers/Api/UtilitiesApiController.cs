using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UtilityPaymentJournal.DTO.Residences;
using UtilityPaymentJournal.DTO.Utilities;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Mapping;
using UtilityPaymentJournal.Models.Residences;
using UtilityPaymentJournal.Models.Utilities;
using UtilityPaymentJournal.Services;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/utilities")]
    public class UtilitiesApiController : ControllerBase
    {
        private readonly IUtilityService _utilityService;
        private readonly IUtilityMapper _utilityMapper;

        public UtilitiesApiController(
            IUtilityService utilityService,
            IUtilityMapper utilityMapper)
        {
            _utilityService = utilityService;
            _utilityMapper = utilityMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<UtilityViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<UtilityDTO> dtos = await _utilityService.GetAllAsync(cancellationToken);

            IEnumerable<UtilityViewModel> viewModels = dtos
                .Select(dto => _utilityMapper.ToViewModel(dto))
                .ToList();

            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<UtilityViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            UtilityDTO? dto = await _utilityService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
            {
                return NotFound($"Услуга с ID {id} не найдена.");
            }

            UtilityViewModel viewModel = _utilityMapper.ToViewModel(dto);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUtilityViewModel createViewModel, CancellationToken cancellationToken)
        {
            CreateUtilityDTO createDto = _utilityMapper.ToDto(createViewModel);

            UtilityDTO createdDto = await _utilityService.CreateAsync(createDto, cancellationToken);

            UtilityViewModel createdViewModel = _utilityMapper.ToViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }



        [HttpPut("{id:long}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] EditUtilityViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditUtilityDTO editDto = _utilityMapper.ToDto(editViewModel);

            UtilityDTO? updatedDto = await _utilityService.EditAsync(id, editDto, cancellationToken);
            if (updatedDto is null)
            {
                return NotFound($"Услуга с ID {id} не найдена.");
            }

            UtilityViewModel updatedViewModel = _utilityMapper.ToViewModel(updatedDto);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            bool isDeleted = await _utilityService.DeleteAsync(id, cancellationToken);
            if (!isDeleted)
            {
                return NotFound($"Не удалось удалить. Услуга с ID {id} не найдена.");
            }

            return NoContent();
        }
    }
}
