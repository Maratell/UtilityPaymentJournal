using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTOs.Residences;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Services
{
    public partial class ResidenceService : IResidenceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResidenceMapper _residenceMapper;
        private readonly ILogger<ResidenceService> _logger;

        public ResidenceService(
            ApplicationDbContext context,
            IResidenceMapper residenceMapper,
            ILogger<ResidenceService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _residenceMapper = residenceMapper ?? throw new ArgumentNullException(nameof(residenceMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResidenceDto> CreateAsync(CreateResidenceDto createDto, CancellationToken cancellationToken = default)
        {
            LogResidenceCreationRequested(_logger, createDto);
            Residence entity = _residenceMapper.ToEntity(createDto);

            // используем синхронный Add, так как операция происходит в памяти
            _context.Residences.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogResidenceCreatedInDb(_logger, entity.Id);
            return _residenceMapper.ToDto(entity);
        }

        public async Task<ResidenceDto?> EditAsync(long id, EditResidenceDto editDto, CancellationToken cancellationToken = default)
        {
            LogResidenceUpdateRequested(_logger, id, editDto);

            Residence? entity = await FindEntityAsync(id, cancellationToken);
            if (entity == null)
            {
                LogResidenceNotFoundInDb(_logger, id);
                return null;
            }

            _residenceMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogResidenceUpdatedInDb(_logger, id);
            return _residenceMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogResidenceDeletionRequested(_logger, id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM Residences WHERE Id = @id
            // Метод возвращает количество удаленных строк (0 или 1)
            int deletedRowsCount = await _context.Residences
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0) 
                LogResidenceNotFoundInDb(_logger, id);
            else
                LogResidenceDeletedFromDb(_logger, id);

            return deletedRowsCount > 0;
        }



        public async Task<IReadOnlyCollection<ResidenceDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingAllResidencesFromDb(_logger);
            List<Residence> entities = await _context.Residences
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllResidencesFromDbCount(_logger, entities.Count);
            return entities
                .Select(e => _residenceMapper.ToDto(e))
                .ToList();
        }

        public async Task<ResidenceDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingResidenceByIdFromDb(_logger, id);

            // загружаем entity со всеми деталями для передачи клиенту в UI
            Residence? entity = await FindEntityAsync(id, cancellationToken);
            if (entity is null)
            {
                LogResidenceNotFoundInDb(_logger, id);
                return null;
            }

            return _residenceMapper.ToDto(entity);
        }

        private async Task<Residence?> FindEntityAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.Residences
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        #region Шаблоны логов

        #region Начало выполнения операций (Уровень Debug/Information) ---

        [LoggerMessage(EventId = 2001, Level = LogLevel.Information, Message = "Запрос на создание жилого объекта в БД: {@CreateDto}")]
        private static partial void LogResidenceCreationRequested(ILogger logger, CreateResidenceDto createDto);

        [LoggerMessage(EventId = 2002, Level = LogLevel.Information, Message = "Запрос на обновление жилого объекта в БД {ResidenceId}: {@EditDto}")]
        private static partial void LogResidenceUpdateRequested(ILogger logger, long residenceId, EditResidenceDto editDto);

        [LoggerMessage(EventId = 2003, Level = LogLevel.Information, Message = "Запрос на удаление жилого объекта из БД {ResidenceId}")]
        private static partial void LogResidenceDeletionRequested(ILogger logger, long residenceId);

        #endregion

        #region Успешный финал операций записи (Уровень Information) 

        [LoggerMessage(EventId = 2004, Level = LogLevel.Information, Message = "Жилой объект {ResidenceId} успешно сохранен в БД")]
        private static partial void LogResidenceCreatedInDb(ILogger logger, long residenceId);

        [LoggerMessage(EventId = 2005, Level = LogLevel.Information, Message = "Жилой объект {ResidenceId} успешно изменен в БД")]
        private static partial void LogResidenceUpdatedInDb(ILogger logger, long residenceId);

        [LoggerMessage(EventId = 2006, Level = LogLevel.Information, Message = "Жилой объект {ResidenceId} успешно удален из БД")]
        private static partial void LogResidenceDeletedFromDb(ILogger logger, long residenceId);

        #endregion

        #region Чтение данных (Уровень Debug для снижения шума в Prod)

        [LoggerMessage(EventId = 2007, Level = LogLevel.Debug, Message = "Запрос к БД на получение списка всех жилых объектов")]
        private static partial void LogFetchingAllResidencesFromDb(ILogger logger);

        [LoggerMessage(EventId = 2008, Level = LogLevel.Debug, Message = "Из БД успешно извлечено {Count} записей жилых объектов")]
        private static partial void LogFetchedAllResidencesFromDbCount(ILogger logger, int count);

        [LoggerMessage(EventId = 2009, Level = LogLevel.Debug, Message = "Запрос к БД на получение жилого объекта по {ResidenceId}")]
        private static partial void LogFetchingResidenceByIdFromDb(ILogger logger, long residenceId);

        #endregion

        #region Ошибки бизнес-логики (Уровень Warning)

        [LoggerMessage(EventId = 2010, Level = LogLevel.Warning, Message = "Операция отменена: жилой объект {ResidenceId} отсутствует в БД")]
        private static partial void LogResidenceNotFoundInDb(ILogger logger, long residenceId);

        #endregion

        #endregion
    }
}
