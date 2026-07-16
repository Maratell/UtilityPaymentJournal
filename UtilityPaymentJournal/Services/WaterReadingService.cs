using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTOs.WaterReadings;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.WaterReadings;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Services
{
    public partial class WaterReadingService : IWaterReadingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWaterReadingMapper _waterReadingMapper;
        private readonly ILogger<WaterReadingService> _logger;

        public WaterReadingService(
            ApplicationDbContext context,
            IWaterReadingMapper waterReadingMapper,
            ILogger<WaterReadingService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _waterReadingMapper = waterReadingMapper ?? throw new ArgumentNullException(nameof(waterReadingMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<WaterReadingDto> CreateAsync(CreateWaterReadingDto createDto, CancellationToken cancellationToken = default)
        {
            LogWaterReadingCreationRequested(_logger, createDto.CurrentValue);
            WaterReading entity = _waterReadingMapper.ToEntity(createDto);

            // Используем синхронный Add, так как операция происходит в памяти
            _context.WaterReadings.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            // Подгружаем к entity связанные свойства для актуализации данных в памяти
            await LoadDetailsAsync(entity, cancellationToken);

            LogWaterReadingCreatedInDb(_logger, entity.Id);
            return _waterReadingMapper.ToDto(entity);
        }

        public async Task<WaterReadingDto> EditAsync(long id, EditWaterReadingDto editDto, CancellationToken cancellationToken = default)
        {
            LogWaterReadingUpdateRequested(_logger, id, editDto.CurrentValue);

            // Подход с двумя загрузками:
            // 1. Загружаем "легковесное" entity без связанных деталей
            WaterReading? entity = await FindEntityAsync(id, includeDetails: false, cancellationToken: cancellationToken);
            if (entity == null)
            {
                LogWaterReadingNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Показание счетчика воды с ID {id} не найдено в базе данных.");
            }

            // 2. Обновляем и сохраняем данные в бд
            _waterReadingMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            // 3. Подгружаем к entity связанные свойства для актуализации данных в памяти
            await LoadDetailsAsync(entity, cancellationToken);

            LogWaterReadingUpdatedInDb(_logger, id);
            return _waterReadingMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogWaterReadingDeletionRequested(_logger, id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM WaterReadings WHERE Id = @id
            // Метод возвращает количество удаленных строк (0 или 1)
            int deletedRowsCount = await _context.WaterReadings
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogWaterReadingNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Не удалось удалить. Показание счетчика воды с ID {id} не найдено.");
            }

            LogWaterReadingDeletedFromDb(_logger, id);
            return true;
        }

        public async Task<IReadOnlyCollection<WaterReadingDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingAllWaterReadingsFromDb(_logger);

            // Извлекаем данные из БД с жадной загрузкой (Eager Loading) связанных объектов
            List<WaterReading> entities =  await _context.WaterReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllWaterReadingsFromDbCount(_logger, entities.Count);

            return entities
                .Select(w => _waterReadingMapper.ToDto(w))
                .ToArray();
        }

        public async Task<WaterReadingDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingWaterReadingByIdFromDb(_logger, id);

            // загружаем entity со всеми деталями для передачи клиенту в UI
            WaterReading? entity = await FindEntityAsync(id, includeDetails: true, cancellationToken: cancellationToken);
            if (entity is null)
            {
                LogWaterReadingNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Показание счетчика воды с ID {id} не найдено.");
            }

            return _waterReadingMapper.ToDto(entity);
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

            return await query.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        private async Task LoadDetailsAsync(WaterReading entity, CancellationToken cancellationToken)
        {
            // Последовательно дозагружаем связанные сущности по ссылке в рамках трекера
            await _context
                .Entry(entity)
                .Reference(e => e.Residence)
                .LoadAsync(cancellationToken);

            await _context
                .Entry(entity)
                .Reference(e => e.UtilityProvider)
                .LoadAsync(cancellationToken);
        }
    }
}
