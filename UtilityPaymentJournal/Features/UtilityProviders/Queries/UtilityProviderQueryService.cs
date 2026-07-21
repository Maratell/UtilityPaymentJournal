using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.UtilityProviders.Queries
{
    /// <summary>
    /// Сервис запросов (чтения) поставщиков коммунальных услуг.
    /// Реализует логику эффективного извлечения данных из БД без изменения состояния системы.
    /// </summary>
    public partial class UtilityProviderQueryService : IUtilityProviderQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityProviderMapper _utilityProviderMapper;
        private readonly ILogger<UtilityProviderQueryService> _logger;

        public UtilityProviderQueryService(
            ApplicationDbContext context,
            IUtilityProviderMapper utilityProviderMapper,
            ILogger<UtilityProviderQueryService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _utilityProviderMapper = utilityProviderMapper ?? throw new ArgumentNullException(nameof(utilityProviderMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UtilityProviderQueryResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingUtilityProviderByIdFromDb(_logger, id);

            UtilityProvider? entity = await _context.UtilityProviders
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (entity is null)
            {
                LogUtilityProviderNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Поставщик коммунальных услуг с ID {id} не найден.");
            }

            return _utilityProviderMapper.ToQueryResultDto(entity);
        }

        public async Task<IReadOnlyCollection<UtilityProviderQueryResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingAllUtilityProvidersFromDb(_logger);

            List<UtilityProvider> entities = await _context.UtilityProviders
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllUtilityProvidersFromDbCount(_logger, entities.Count);

            return entities
                .Select(p => _utilityProviderMapper.ToQueryResultDto(p))
                .ToArray();
        }
    }
}
