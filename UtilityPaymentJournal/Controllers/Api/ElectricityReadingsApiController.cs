using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTOs.ElectricityReadings;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.ElectricityReadings;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/electricity-readings")]
    public class ElectricityReadingsApiController : ControllerBase
    {
        private readonly IElectricityReadingService _electricityReadingService;
        private readonly IElectricityReadingMapper _electricityReadingMapper;

        public ElectricityReadingsApiController(
            IElectricityReadingService electricityReadingService,
            IElectricityReadingMapper electricityReadingMapper)
        {
            _electricityReadingService = electricityReadingService;
            _electricityReadingMapper = electricityReadingMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ElectricityReadingViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<ElectricityReadingDTO> dtos = await _electricityReadingService.GetAllAsync(cancellationToken);

            List<ElectricityReadingViewModel> viewModels = dtos
                .Select(r => _electricityReadingMapper.ToViewModel(r))
                .ToList();

            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ElectricityReadingViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            ElectricityReadingDTO? dto = await _electricityReadingService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
            {
                return NotFound($"Показание счетчика с ID {id} не найдено.");
            }

            ElectricityReadingViewModel viewModel = _electricityReadingMapper.ToViewModel(dto);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult<ElectricityReadingViewModel>> Create([FromBody] CreateElectricityReadingViewModel createViewModel, CancellationToken cancellationToken)
        {
            CreateElectricityReadingDTO createDto = _electricityReadingMapper.ToDto(createViewModel);

            ElectricityReadingDTO createdDto = await _electricityReadingService.CreateAsync(createDto, cancellationToken);

            ElectricityReadingViewModel createdViewModel = _electricityReadingMapper.ToViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ElectricityReadingViewModel>> Edit([FromRoute] long id, [FromBody] EditElectricityReadingViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditElectricityReadingDTO editDto = _electricityReadingMapper.ToDto(editViewModel);

            ElectricityReadingDTO? updatedDto = await _electricityReadingService.EditAsync(id, editDto, cancellationToken);
            if (updatedDto is null)
            {
                return NotFound($"Показание счетчика с ID {id} не найдено.");
            }

            ElectricityReadingViewModel updatedViewModel = _electricityReadingMapper.ToViewModel(updatedDto);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            bool isDeleted = await _electricityReadingService.DeleteAsync(id, cancellationToken);
            if (!isDeleted)
            {
                return NotFound($"Не удалось удалить. Показание счетчика с ID {id} не найдено.");
            }

            return NoContent();
        }
    }
}
