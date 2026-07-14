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
            LogResidenceCreationRequested(_logger, createResidence);

            CreateResidenceDto createDto = _residenceMapper.ToDto(createResidence);
            ResidenceDto createdDto = await _residenceService.CreateAsync(createDto, cancellationToken);
            ResidenceViewModel createdViewModel = _residenceMapper.ToViewModel(createdDto);

            LogResidenceCreated(_logger, createdViewModel.Id);
            return CreatedAtAction(nameof(GetById), new { id = createdViewModel.Id }, createdViewModel);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<ResidenceViewModel>> Edit([FromRoute] long id, [FromBody] EditResidenceViewModel editViewModel, CancellationToken cancellationToken)
        {
            LogResidenceUpdateRequested(_logger, id, editViewModel);

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
            LogResidenceDeletionRequested(_logger, id);

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

        #region Начало выполнения операций (Уровень Information)

        [LoggerMessage(EventId = 1008, Level = LogLevel.Information, Message = "Запрос на создание жилого объекта: {@ResidenceData}")]
        private static partial void LogResidenceCreationRequested(ILogger logger, CreateResidenceViewModel residenceData);

        [LoggerMessage(EventId = 1009, Level = LogLevel.Information, Message = "Запрос на обновление жилого объекта {ResidenceId}: {@ResidenceData}")]
        private static partial void LogResidenceUpdateRequested(ILogger logger, long residenceId, EditResidenceViewModel residenceData);

        [LoggerMessage(EventId = 1010, Level = LogLevel.Information, Message = "Запрос на удаление жилого объекта {ResidenceId}")]
        private static partial void LogResidenceDeletionRequested(ILogger logger, long residenceId);

        #endregion

        #region Успешный финал операций (Уровень Information)

        [LoggerMessage(EventId = 1001, Level = LogLevel.Information, Message = "Жилой объект {ResidenceId} удален")]
        private static partial void LogResidenceDeleted(ILogger logger, long residenceId);

        [LoggerMessage(EventId = 1002, Level = LogLevel.Information, Message = "Создан жилой объект {ResidenceId}")]
        private static partial void LogResidenceCreated(ILogger logger, long residenceId);

        [LoggerMessage(EventId = 1003, Level = LogLevel.Information, Message = "Обновлен жилой объект {ResidenceId}")]
        private static partial void LogResidenceUpdated(ILogger logger, long residenceId);

        #endregion

        #region Чтение данных (Уровень Debug)

        [LoggerMessage(EventId = 1004, Level = LogLevel.Debug, Message = "Запрос на получение всех жилых объектов")]
        private static partial void LogFetchingAllResidences(ILogger logger);

        [LoggerMessage(EventId = 1005, Level = LogLevel.Debug, Message = "Получено {Count} жилых объектов")]
        private static partial void LogFetchedAllResidencesCount(ILogger logger, int count);

        #endregion

        #region Ошибки и проверки (Уровень Warning)

        [LoggerMessage(EventId = 1006, Level = LogLevel.Warning, Message = "Жилой объект {ResidenceId} не найден")]
        private static partial void LogResidenceNotFound(ILogger logger, long residenceId);

        [LoggerMessage(EventId = 1007, Level = LogLevel.Warning, Message = "Не удалось удалить жилой объект {ResidenceId}: объект не найден")]
        private static partial void LogResidenceDeleteFailedNotFound(ILogger logger, long residenceId);

        #endregion

        #endregion
    }
}
