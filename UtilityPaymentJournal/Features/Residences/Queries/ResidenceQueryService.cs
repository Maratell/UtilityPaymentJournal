using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.Queries
{
    /// <summary>
    /// Сервис запросов (чтения) показаний счетчиков электроэнергии.
    /// Реализует логику эффективного извлечения данных из БД без изменения состояния системы.
    /// </summary>
    public partial class ResidenceQueryService : IResidenceQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResidenceMapper _residenceMapper;
        private readonly ILogger<ResidenceQueryService> _logger;

        public ResidenceQueryService(
            ApplicationDbContext context,
            IResidenceMapper residenceMapper,
            ILogger<ResidenceQueryService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _residenceMapper = residenceMapper ?? throw new ArgumentNullException(nameof(residenceMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResidenceQueryResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingResidenceByIdFromDb(_logger, id);

            Residence? entity = await _context.Residences
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (entity is null)
            {
                LogResidenceNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Показание счетчика электроэнергии с ID {id} не найдено.");
            }

            return _residenceMapper.ToQueryResultDto(entity);
        }

        public async Task<IReadOnlyCollection<ResidenceQueryResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingAllResidencesFromDb(_logger);

            List<Residence> entities = await _context.Residences
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllResidencesFromDbCount(_logger, entities.Count);

            return entities
                .Select(w => _residenceMapper.ToQueryResultDto(w))
                .ToArray();
        }
    }
}
