using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.ElectricityReadings;

namespace UtilityPaymentJournal.Features.ElectricityReadings.Queries
{
    /// <summary>
    /// Сервис запросов (чтения) показаний счетчиков электроэнергии.
    /// Реализует логику эффективного извлечения данных из БД без изменения состояния системы.
    /// </summary>
    public partial class ElectricityReadingQueryService : IElectricityReadingQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IElectricityReadingMapper _electricityReadingMapper;
        private readonly ILogger<ElectricityReadingQueryService> _logger;

        public ElectricityReadingQueryService(
            ApplicationDbContext context,
            IElectricityReadingMapper electricityReadingMapper,
            ILogger<ElectricityReadingQueryService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _electricityReadingMapper = electricityReadingMapper ?? throw new ArgumentNullException(nameof(electricityReadingMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ElectricityReadingQueryResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingElectricityReadingByIdFromDb(_logger, id);

            // Загружаем entity со всеми деталями (Eager Loading)
            ElectricityReading? entity = await _context.ElectricityReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (entity is null)
            {
                LogElectricityReadingNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Показание счетчика электроэнергии с ID {id} не найдено.");
            }

            return _electricityReadingMapper.ToQueryResultDto(entity);
        }

        public async Task<IReadOnlyCollection<ElectricityReadingQueryResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingAllElectricityReadingsFromDb(_logger);

            // Извлекаем данные из БД с жадной загрузкой (Eager Loading) связанных объектов
            List<ElectricityReading> entities = await _context.ElectricityReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllElectricityReadingsFromDbCount(_logger, entities.Count);

            return entities
                .Select(w => _electricityReadingMapper.ToQueryResultDto(w))
                .ToArray();
        }
    }
}
