using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTOs.ElectricityReadings;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.EF.Entity.WaterReadings;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Services
{
    public class ElectricityReadingService : IElectricityReadingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IElectricityReadingMapper _electricityReadingMapper;

        public ElectricityReadingService(
            ApplicationDbContext context,
            IElectricityReadingMapper electricityReadingMapper)
        {
            _context = context;
            _electricityReadingMapper = electricityReadingMapper;
        }

        public async Task<ElectricityReadingDTO> CreateAsync(CreateElectricityReadingDTO createDto, CancellationToken cancellationToken = default)
        {
            ElectricityReading entity = _electricityReadingMapper.ToEntity(createDto);

            // используем синхронный Add, так как операция происходит в памяти
            _context.ElectricityReadings.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            // Пытаемся подтянуть полные детали из бд
            ElectricityReading? savedEntity = await FindEntityAsync(entity.Id, includeDetails: true, cancellationToken);

            // Если бд вернула объект с деталями — маппим его. 
            // Если произошел сбой и вернулся null — маппим исходный entity из памяти.
            return _electricityReadingMapper.ToDto(savedEntity ?? entity);
        }

        public async Task<ElectricityReadingDTO?> EditAsync(long id, EditElectricityReadingDTO editDto, CancellationToken cancellationToken = default)
        {
            // Подход с двумя загрузками:
            // 1. Загружаем "легковесное" entity без связанных деталей
            ElectricityReading? entity = await FindEntityAsync(id, includeDetails: false, cancellationToken: cancellationToken);
            if (entity == null)
                return null;

            _electricityReadingMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            // 2. После SaveChangesAsync EF Core зануляет или оставляет устаревшими навигационные свойства связей в памяти (Identity Map).
            // Делаем повторный запрос с includeDetails: true, чтобы принудительно выкачать из бд актуальный объект 
            // с обновленными связанными данными для корректного маппинга на фронтенд.
            ElectricityReading? updatedEntity = await FindEntityAsync(id, includeDetails: true, cancellationToken);
            return _electricityReadingMapper.ToDto(updatedEntity ?? entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            // EF Core сразу генерирует SQL-запрос: DELETE FROM ElectricityReadings WHERE Id = @id
            // Метод возвращает количество удаленных строк (0 или 1)
            int deletedRowsCount = await _context.ElectricityReadings
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            // Если удалена 1 строка — возвращаем true, если 0 (id не найден) — возвращаем false
            return deletedRowsCount > 0;
        }

        public async Task<IReadOnlyCollection<ElectricityReadingDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Извлекаем данные из БД с жадной загрузкой (Eager Loading) связанных объектов
            List<ElectricityReading> entities = await _context.ElectricityReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync(cancellationToken);

            return entities
                .Select(w => _electricityReadingMapper.ToDto(w))
                .ToList();
        }

        public async Task<ElectricityReadingDTO?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            // загружаем entity со всеми деталями для передачи клиенту в UI
            ElectricityReading? entity = await FindEntityAsync(id, includeDetails: true, cancellationToken: cancellationToken);

            return entity is null ? null : _electricityReadingMapper.ToDto(entity);
        }

        private async Task<ElectricityReading?> FindEntityAsync(long id, bool includeDetails, CancellationToken cancellationToken)
        {
            IQueryable<ElectricityReading> query = _context.ElectricityReadings;

            if (includeDetails)
            {
                query = query
                    .Include(w => w.Residence)
                    .Include(w => w.UtilityProvider);
            }

            return await query.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
