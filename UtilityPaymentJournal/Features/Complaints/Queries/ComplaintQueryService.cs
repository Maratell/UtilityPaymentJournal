using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Features.Complaints.Models;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;

namespace UtilityPaymentJournal.Features.Complaints.Queries
{
    /// <summary>
    /// Сервис запросов (чтения) жалоб.
    /// Реализует логику эффективного извлечения данных из БД без изменения состояния системы.
    /// </summary>
    public partial class ComplaintQueryService : IComplaintQueryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IComplaintMapper _complaintMapper;
        private readonly ILogger<ComplaintQueryService> _logger;

        public ComplaintQueryService(
            ApplicationDbContext context,
            IComplaintMapper complaintMapper,
            ILogger<ComplaintQueryService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _complaintMapper = complaintMapper ?? throw new ArgumentNullException(nameof(complaintMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ComplaintQueryResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingComplaintByIdFromDb(_logger, id);

            Complaint? entity = await FindEntityAsync(id, cancellationToken);
            if (entity is null)
            {
                LogComplaintNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Жалоба с ID {id} не найдена.");
            }

            return _complaintMapper.ToQueryResultDto(entity);
        }

        public async Task<IReadOnlyCollection<ComplaintQueryResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingAllComplaintsFromDb(_logger);

            List<Complaint> entities = await _context.Complaints
                .Include(w => w.Utility)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllComplaintsFromDbCount(_logger, entities.Count);

            return entities
                .Select(w => _complaintMapper.ToQueryResultDto(w))
                .ToArray();
        }

        public async Task<Dictionary<ComplaintStatus, List<ComplaintViewModel>>> GetComplaintsGroupedByStatusAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingComplaintBoardFromDb(_logger);

            List<Complaint> entities = await _context.Complaints
                .Include(w => w.Utility)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            // Инициализируем словарь со всеми возможными статусами из Enum, чтобы избежать NullReference в HTML при пустых колонках
            var dictionary = Enum.GetValues<ComplaintStatus>()
                .ToDictionary(status => status, status => new List<ComplaintViewModel>());

            // Мапим доменные сущности в QueryResultDto, преобразуем во ViewModel и распределяем по статусам за один проход
            foreach (var entity in entities)
            {
                var queryResultDto = _complaintMapper.ToQueryResultDto(entity);
                var viewModel = _complaintMapper.ToViewModel(queryResultDto);
                dictionary[entity.Status].Add(viewModel);
            }

            LogFetchedComplaintBoardFromDb(_logger, entities.Count);
            return dictionary;
        }

        private async Task<Complaint?> FindEntityAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.Complaints
                .Include(w => w.Utility)
                .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
