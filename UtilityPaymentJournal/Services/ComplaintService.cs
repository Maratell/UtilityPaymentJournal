using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.DTOs.ComplaintBoard;
using UtilityPaymentJournal.EF.Entity.ComplaintBoard;
using Microsoft.EntityFrameworkCore;

namespace UtilityPaymentJournal.Services
{
    public partial class ComplaintService : IComplaintService
    {
        private readonly ApplicationDbContext _context;
        private readonly IComplaintMapper _complaintMapper;
        private readonly ILogger<ComplaintService> _logger;

        public ComplaintService(
            ApplicationDbContext context,
            IComplaintMapper complaintMapper,
            ILogger<ComplaintService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _complaintMapper = complaintMapper ?? throw new ArgumentNullException(nameof(complaintMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ComplaintDto> CreateAsync(CreateComplaintDto createDto, CancellationToken cancellationToken = default)
        {
            LogComplaintCreationRequested(_logger, createDto.UtilityId, createDto.Title);
            Complaint entity = _complaintMapper.ToEntity(createDto);

            // Используем синхронный Add, так как операция происходит в памяти
            _context.Complaints.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            // Подгружаем связи к entity
            await LoadDetailsAsync(entity, cancellationToken);

            LogComplaintCreatedInDb(_logger, entity.Id);
            return _complaintMapper.ToDto(entity);
        }

        public async Task<ComplaintDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingComplaintByIdFromDb(_logger, id);

            // Загружаем entity со всеми деталями для передачи клиенту
            Complaint? entity = await FindEntityAsync(id, includeDetails: true, cancellationToken);
            if (entity is null)
            {
                LogComplaintNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Жалоба с ID {id} не найдена.");
            }

            return _complaintMapper.ToDto(entity);
        }

        public async Task<ComplaintDto> EditAsync(long id, EditComplaintDto editDto, CancellationToken cancellationToken = default)
        {
            LogComplaintUpdateRequested(_logger, id, editDto.UtilityId, editDto.Title);

            // 1. Загружаем "легковесное" entity без связанных деталей
            Complaint? entity = await FindEntityAsync(id, includeDetails: false, cancellationToken);
            if (entity is null)
            {
                LogComplaintNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Жалоба с ID {id} не найдена в базе данных.");
            }

            // 2. Обновляем entity и сохраняем в бд
            _complaintMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            // 3. Подгружаем связи к entity
            await LoadDetailsAsync(entity, cancellationToken);

            LogComplaintUpdatedInDb(_logger, id);
            return _complaintMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogComplaintDeletionRequested(_logger, id);

            // EF Core сразу генерирует SQL: DELETE FROM Complaints WHERE Id = @id
            int deletedRowsCount = await _context.Complaints
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogComplaintNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Не удалось удалить. Жалоба с ID {id} не найдена.");
            }

            LogComplaintDeletedFromDb(_logger, id);
            return true;
        }

        public async Task<IReadOnlyCollection<ComplaintDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingAllComplaintsFromDb(_logger);

            // Извлекаем данные из БД с жадной загрузкой (Eager Loading) связанных услуг
            List<Complaint> entities = await _context.Complaints
                .Include(c => c.Utility)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllComplaintsFromDbCount(_logger, entities.Count);

            return entities
                .Select(e => _complaintMapper.ToDto(e))
                .ToArray();
        }

        private async Task<Complaint?> FindEntityAsync(long id, bool includeDetails, CancellationToken cancellationToken)
        {
            IQueryable<Complaint> query = _context.Complaints;

            if (includeDetails)
            {
                query = query.Include(c => c.Utility);
            }

            return await query.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        private async Task LoadDetailsAsync(Complaint entity, CancellationToken cancellationToken)
        {
            await _context
                .Entry(entity)
                .Reference(e => e.Utility)
                .LoadAsync(cancellationToken);
        }
    }
}
