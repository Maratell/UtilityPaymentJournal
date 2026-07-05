using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTO.WaterReadings;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.WaterReadings;
using WaterReadingPaymentJournal.Interface.Mapping;
using WaterReadingPaymentJournal.Interface.Service;

namespace WaterReadingPaymentJournal.Services
{
    public class WaterReadingService : IWaterReadingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWaterReadingMapper _waterReadingMapper;

        public WaterReadingService(
            ApplicationDbContext context,
            IWaterReadingMapper waterReadingMapper)
        {
            _context = context;
            _waterReadingMapper = waterReadingMapper;
        }

        public async Task<WaterReadingDTO> CreateAsync(CreateWaterReadingDTO createDto, CancellationToken cancellationToken = default)
        {
            WaterReading entity = _waterReadingMapper.ToEntity(createDto);

            // используем синхронный Add, так как операция происходит в памяти
            _context.WaterReadings.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            // Пытаемся подтянуть полные детали из бд
            WaterReading? savedEntity = await FindEntityAsync(entity.Id, includeDetails: true, cancellationToken);

            // Если бд вернула объект с деталями — маппим его. 
            // Если произошел сбой и вернулся null — маппим исходный entity из памяти.
            return _waterReadingMapper.ToDto(savedEntity ?? entity);
        }

        public async Task<WaterReadingDTO?> EditAsync(long id, EditWaterReadingDTO editDto, CancellationToken cancellationToken = default)
        {
            // загружаем entity с деталями, чтобы вернуть клиенту полный обновленный DTO
            WaterReading? entity = await FindEntityAsync(id, includeDetails: true, cancellationToken: cancellationToken);
            if (entity == null)
                return null;

            _waterReadingMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _waterReadingMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            // EF Core сразу генерирует SQL-запрос: DELETE FROM WaterReadings WHERE Id = @id
            // Метод возвращает количество удаленных строк (0 или 1)
            int deletedRowsCount = await _context.WaterReadings
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            // Если удалена 1 строка — возвращаем true, если 0 (id не найден) — возвращаем false
            return deletedRowsCount > 0;
        }

        public async Task<IReadOnlyCollection<WaterReadingDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            // Извлекаем данные из БД с жадной загрузкой (Eager Loading) связанных объектов
            List<WaterReading> entities =  await _context.WaterReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync(cancellationToken);

            return entities
                .Select(w => _waterReadingMapper.ToDto(w))
                .ToList();
        }

        public async Task<WaterReadingDTO?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            // загружаем entity со всеми деталями для передачи клиенту в UI
            WaterReading? entity = await FindEntityAsync(id, includeDetails: true, cancellationToken: cancellationToken);

            return entity is null ? null : _waterReadingMapper.ToDto(entity);
        }

        private async Task<WaterReading?> FindEntityAsync(long id, bool includeDetails, CancellationToken cancellationToken)
        {
            IQueryable<WaterReading> query = _context.WaterReadings;

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
