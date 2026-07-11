using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.Interface.Service;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.DTOs.ComplaintBoard;
using UtilityPaymentJournal.EF.Entity.ComplaintBoard;
using Microsoft.EntityFrameworkCore;

namespace UtilityPaymentJournal.Services
{
    public class ComplaintService : IComplaintService
    {
        private readonly ApplicationDbContext _context;
        private readonly IComplaintMapper _complaintMapper;

        public ComplaintService(
            ApplicationDbContext context,
            IComplaintMapper complaintMapper)
        {
            _context = context;
            _complaintMapper = complaintMapper;
        }

        public async Task<ComplaintDTO> CreateAsync(CreateComplaintDTO createDto, CancellationToken cancellationToken = default)
        {
            Complaint entity = _complaintMapper.ToEntity(createDto);

            // Используем синхронный Add, так как операция происходит в памяти
            _context.Complaints.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            // Подтягиваем полные детали из бд 
            Complaint? savedEntity = await FindEntityAsync(entity.Id, includeDetails: true, cancellationToken);

            return _complaintMapper.ToDto(savedEntity ?? entity);
        }

        public async Task<ComplaintDTO?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            // Загружаем entity со всеми деталями для передачи клиенту
            Complaint? entity = await FindEntityAsync(id, includeDetails: true, cancellationToken);

            return entity is null ? null : _complaintMapper.ToDto(entity);
        }

        public async Task<ComplaintDTO?> EditAsync(long id, EditComplaintDTO editDto, CancellationToken cancellationToken = default)
        {
            // Подход с двумя загрузками:
            // 1. Загружаем "легковесное" entity без связанных деталей
            Complaint? entity = await FindEntityAsync(id, includeDetails: false, cancellationToken);
            if (entity is null)
                return null;

            _complaintMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            // 2. После SaveChangesAsync EF Core зануляет или оставляет устаревшими навигационные свойства связей в памяти (Identity Map).
            // Делаем повторный запрос с includeDetails: true, чтобы принудительно выкачать из бд актуальный объект 
            // с обновленными связанными данными для корректного маппинга на фронтенд.
            Complaint? updatedEntity = await FindEntityAsync(id, includeDetails: true, cancellationToken);
            return _complaintMapper.ToDto(updatedEntity ?? entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            // EF Core сразу генерирует SQL: DELETE FROM Complaints WHERE Id = @id
            // Метод возвращает количество удаленных строк (0 или 1) без предварительной выгрузки сущности в память
            int deletedRowsCount = await _context.Complaints
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            // Если удалена 1 строка — возвращаем true, если 0 (id не найден) — возвращаем false
            return deletedRowsCount > 0;
        }

        public async Task<IReadOnlyCollection<ComplaintDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Извлекаем данные из БД с жадной загрузкой (Eager Loading) связанных услуг
            List<Complaint> entities = await _context.Complaints
                .Include(c => c.Utility)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return entities
                .Select(e => _complaintMapper.ToDto(e))
                .ToList();
        }

        private async Task<Complaint?> FindEntityAsync(long id, bool includeDetails, CancellationToken cancellationToken)
        {
            IQueryable<Complaint> query = _context.Complaints;

            if (includeDetails)
            {
                query = query.Include(c => c.Utility);
            }

            return await query.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
