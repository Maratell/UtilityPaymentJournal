using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.Common.Specifications;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.Queries
{
    /// <summary>
    /// Сервис запросов (чтения) коммунальных услуг.
    /// Реализует логику эффективного извлечения данных из БД без изменения состояния системы.
    /// </summary>
    public partial class UtilityQueryService : IUtilityQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityMapper _utilityMapper;
        private readonly ILogger<UtilityQueryService> _logger;

        public UtilityQueryService(
            ApplicationDbContext context,
            IUtilityMapper utilityMapper,
            ILogger<UtilityQueryService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _utilityMapper = utilityMapper ?? throw new ArgumentNullException(nameof(utilityMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UtilityQueryResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingUtilityByIdFromDb(_logger, id);

            Utility? entity = await _context.Utilities
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (entity is null)
            {
                LogUtilityNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Коммунальной услуги с ID {id} не найдено.");
            }

            return _utilityMapper.ToQueryResultDto(entity);
        }

        public async Task<IReadOnlyCollection<UtilityQueryResultDto>> GetAllAsync(ICriteriaSpecification<Utility> criteria, CancellationToken cancellationToken = default)
        {
            LogFetchingAllUtilitiesFromDb(_logger);

            // Декларативно строим запрос к таблице, накладывая спецификацию критериев отбора
            List<Utility> entities = await _context.Utilities
                .AsNoTracking()
                .FilterWith(criteria)
                .ToListAsync(cancellationToken);

            LogFetchedAllUtilitiesFromDbCount(_logger, entities.Count);

            return entities
                .Select(_utilityMapper.ToQueryResultDto)
                .ToArray();
        }
    }
}
