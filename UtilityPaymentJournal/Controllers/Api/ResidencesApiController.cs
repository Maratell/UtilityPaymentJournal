using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTOs.Residences;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Models.Residences;


namespace UtilityPaymentJournal.Controllers.Api
{
    [ApiController]
    [Route("api/residences")]
    public partial class ResidencesApiController : ControllerBase
    {
        private readonly IResidenceService _residenceService;
        private readonly IResidenceMapper _residenceMapper;
        private readonly ILogger<ResidencesApiController> _logger;

        public ResidencesApiController(
            IResidenceService residenceService,
            IResidenceMapper residenceMapper,
            ILogger<ResidencesApiController> logger)
        {
            _residenceService = residenceService ?? throw new ArgumentNullException(nameof(residenceService));
            _residenceMapper = residenceMapper ?? throw new ArgumentNullException(nameof(residenceMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ResidenceViewModel>>> GetAll(CancellationToken cancellationToken)
        {
            LogFetchingAllResidences(_logger);

            IEnumerable<ResidenceDto> dtos = await _residenceService.GetAllAsync(cancellationToken);
            ResidenceViewModel[] viewModels = dtos
                .Select(_residenceMapper.ToViewModel)
                .ToArray();

            LogFetchedAllResidencesCount(_logger, viewModels.Length);
            return Ok(viewModels);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<ResidenceViewModel>> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            ResidenceDto? dto = await _residenceService.GetByIdAsync(id, cancellationToken);
            if (dto is null)
            {
                LogResidenceNotFound(_logger, id);
                return NotFound($"Жилой объект с ID {id} не найден.");
            }

            ResidenceViewModel viewModel = _residenceMapper.ToViewModel(dto);
            return Ok(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult<ResidenceViewModel>> Create([FromBody] CreateResidenceViewModel createResidence, CancellationToken cancellationToken)
        {
            CreateResidenceDto createDto = _residenceMapper.ToDto(createResidence);
            ResidenceDto createdDto = await _residenceService.CreateAsync(createDto, cancellationToken);
            ResidenceViewModel createdViewModel = _residenceMapper.ToViewModel(createdDto);

            LogResidenceCreated(_logger, createdViewModel.Id);
            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ResidenceViewModel>> Edit([FromRoute] long id, [FromBody] EditResidenceViewModel editViewModel, CancellationToken cancellationToken)
        {
            EditResidenceDto editDto = _residenceMapper.ToDto(editViewModel);
            ResidenceDto? updatedDto = await _residenceService.EditAsync(id, editDto, cancellationToken);
            if (updatedDto is null)
            {
                LogResidenceNotFound(_logger, id);
                return NotFound($"Жилой объект с ID {id} не найден.");
            }

            ResidenceViewModel updatedViewModel = _residenceMapper.ToViewModel(updatedDto);
            LogResidenceUpdated(_logger, id);
            return Ok(updatedViewModel);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
        {
            bool isDeleted = await _residenceService.DeleteAsync(id, cancellationToken);
            if (!isDeleted)
            {
                LogResidenceDeleteFailedNotFound(_logger, id);
                return NotFound($"Не удалось удалить. Жилой объект с ID {id} не найден.");
            }

            LogResidenceDeleted(_logger, id);
            return NoContent();
        }

        #region Шаблоны логов

        [LoggerMessage(EventId = 1001, Level = LogLevel.Information, Message = "Жилой объект {ResidenceId} успешно удален из системы")]
        private static partial void LogResidenceDeleted(ILogger logger, long residenceId);

        [LoggerMessage(EventId = 1002, Level = LogLevel.Information, Message = "Создан новый жилой объект с ID {ResidenceId}")]
        private static partial void LogResidenceCreated(ILogger logger, long residenceId);

        [LoggerMessage(EventId = 1003, Level = LogLevel.Information, Message = "Жилой объект {ResidenceId} успешно обновлен")]
        private static partial void LogResidenceUpdated(ILogger logger, long residenceId);

        [LoggerMessage(EventId = 1004, Level = LogLevel.Debug, Message = "Запрос на получение всех жилых объектов")]
        private static partial void LogFetchingAllResidences(ILogger logger);

        [LoggerMessage(EventId = 1005, Level = LogLevel.Debug, Message = "Успешно получено {Count} жилых объектов")]
        private static partial void LogFetchedAllResidencesCount(ILogger logger, int count);

        [LoggerMessage(EventId = 1006, Level = LogLevel.Warning, Message = "Жилой объект с ID {ResidenceId} не найден в системе")]
        private static partial void LogResidenceNotFound(ILogger logger, long residenceId);

        [LoggerMessage(EventId = 1007, Level = LogLevel.Warning, Message = "Не удалось удалить жилой объект {ResidenceId}: объект не найден")]
        private static partial void LogResidenceDeleteFailedNotFound(ILogger logger, long residenceId);

        #endregion
    }
}
