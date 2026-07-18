using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.WaterReadings;

namespace UtilityPaymentJournal.Features.WaterReadings.Queries
{
    /// <summary>
    /// Сервис запросов (чтения) показаний счетчиков воды.
    /// Реализует логику эффективного извлечения данных из БД без изменения состояния системы.
    /// </summary>
    public partial class WaterReadingQueryService : IWaterReadingQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWaterReadingMapper _waterReadingMapper;
        private readonly ILogger<WaterReadingQueryService> _logger;

        public WaterReadingQueryService(
            ApplicationDbContext context,
            IWaterReadingMapper waterReadingMapper,
            ILogger<WaterReadingQueryService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _waterReadingMapper = waterReadingMapper ?? throw new ArgumentNullException(nameof(waterReadingMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<WaterReadingQueryResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingWaterReadingByIdFromDb(_logger, id);

            // Загружаем entity со всеми деталями (Eager Loading) для передачи клиенту в UI
            WaterReading? entity = await FindEntityAsync(id, cancellationToken: cancellationToken);
            if (entity is null)
            {
                LogWaterReadingNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Показание счетчика воды с ID {id} не найдено.");
            }

            return _waterReadingMapper.ToQueryResultDto(entity);
        }

        public async Task<IReadOnlyCollection<WaterReadingQueryResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingAllWaterReadingsFromDb(_logger);

            // Извлекаем данные из БД с жадной загрузкой (Eager Loading) связанных объектов
            List<WaterReading> entities = await _context.WaterReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllWaterReadingsFromDbCount(_logger, entities.Count);

            return entities
                .Select(w => _waterReadingMapper.ToQueryResultDto(w))
                .ToArray();
        }

        /// <summary>
        /// Вспомогательный приватный метод для жадной загрузки показания счетчика воды по его идентификатору.
        /// </summary>
        private async Task<WaterReading?> FindEntityAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.WaterReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
