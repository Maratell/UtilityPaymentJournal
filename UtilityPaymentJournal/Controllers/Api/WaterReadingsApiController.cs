using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO.WaterReadings;
using UtilityPaymentJournal.Models.WaterReadings;
using WaterReadingPaymentJournal.Interface.Mapping;
using WaterReadingPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/water-readings")]
    public class WaterReadingsApiController : ControllerBase
    {
        private readonly IWaterReadingService _waterReadingService;
        private readonly IWaterReadingMapper _waterReadingMapper;

        public WaterReadingsApiController(
            IWaterReadingService waterReadingService,
            IWaterReadingMapper waterReadingMapper)
        {
            _waterReadingService = waterReadingService;
            _waterReadingMapper = waterReadingMapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<WaterReadingViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            IEnumerable<WaterReadingDTO> dtos = await _waterReadingService.GetAllAsync(cancellationToken);

            List<WaterReadingViewModel> viewModels = dtos
                .Select(dto => _waterReadingMapper.ToViewModel(dto))
                .ToList();

            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<WaterReadingViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            WaterReadingDTO? dto = await _waterReadingService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
            {
                return NotFound($"Показание счетчика с ID {id} не найдено.");
            }

            WaterReadingViewModel viewModel = _waterReadingMapper.ToViewModel(dto);
            return Ok(viewModel);
        }


        [HttpPost]
        public async Task<ActionResult<WaterReadingViewModel>> Create([FromBody] CreateWaterReadingViewModel createViewModel, CancellationToken cancellationToken)
        {
            CreateWaterReadingDTO createDto = _waterReadingMapper.ToDto(createViewModel);

            WaterReadingDTO createdDto = await _waterReadingService.CreateAsync(createDto, cancellationToken);

            WaterReadingViewModel createdViewModel = _waterReadingMapper.ToViewModel(createdDto);

            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<WaterReadingViewModel>> Edit([FromRoute] long id, [FromBody] EditWaterReadingViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditWaterReadingDTO editDto = _waterReadingMapper.ToDto(editViewModel);

            WaterReadingDTO? updatedDto = await _waterReadingService.EditAsync(id, editDto, cancellationToken);
            if (updatedDto is null)
            {
                return NotFound($"Показание счетчика с ID {id} не найдено.");
            }

            WaterReadingViewModel updatedViewModel = _waterReadingMapper.ToViewModel(updatedDto);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            bool isDeleted = await _waterReadingService.DeleteAsync(id, cancellationToken);
            if (!isDeleted)
            {
                return NotFound($"Не удалось удалить. Показание счетчика с ID {id} не найдено.");
            }

            return NoContent();
        }
    }
}
