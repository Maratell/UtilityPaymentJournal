using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTOs.ElectricityReadings;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Services
{
    public partial class ElectricityReadingService : IElectricityReadingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IElectricityReadingMapper _electricityReadingMapper;
        private readonly ILogger<ElectricityReadingService> _logger;

        public ElectricityReadingService(
            ApplicationDbContext context,
            IElectricityReadingMapper electricityReadingMapper,
            ILogger<ElectricityReadingService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _electricityReadingMapper = electricityReadingMapper ?? throw new ArgumentNullException(nameof(electricityReadingMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ElectricityReadingDto> CreateAsync(CreateElectricityReadingDto createDto, CancellationToken cancellationToken = default)
        {
            LogElectricityReadingCreationRequested(_logger, createDto.CurrentValue);
            ElectricityReading entity = _electricityReadingMapper.ToEntity(createDto);

            // Используем синхронный Add, так как операция происходит в памяти
            _context.ElectricityReadings.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            // Подгружаем к entity связанные свойства для актуализации данных в памяти
            await LoadDetailsAsync(entity, cancellationToken);

            LogElectricityReadingCreatedInDb(_logger, entity.Id);
            return _electricityReadingMapper.ToDto(entity);
        }

        public async Task<ElectricityReadingDto> EditAsync(long id, EditElectricityReadingDto editDto, CancellationToken cancellationToken = default)
        {
            LogElectricityReadingUpdateRequested(_logger, id, editDto.CurrentValue);

            // 1. Загружаем "легковесное" entity без связанных деталей
            ElectricityReading? entity = await FindEntityAsync(id, includeDetails: false, cancellationToken: cancellationToken);
            if (entity == null)
            {
                LogElectricityReadingNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Показание счетчика электроэнергии с ID {id} не найдено в базе данных.");
            }

            // 2. Обновляем и сохраняем данные в бд
            _electricityReadingMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            // 3. Подгружаем к entity связанные свойства для актуализации данных в памяти
            await LoadDetailsAsync(entity, cancellationToken);

            LogElectricityReadingUpdatedInDb(_logger, id);
            return _electricityReadingMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogElectricityReadingDeletionRequested(_logger, id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM ElectricityReadings WHERE Id = @id
            // Метод возвращает количество удаленных строк (0 или 1)
            int deletedRowsCount = await _context.ElectricityReadings
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogElectricityReadingNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Не удалось удалить. Показание счетчика электроэнергии с ID {id} не найдено.");
            }

            LogElectricityReadingDeletedFromDb(_logger, id);
            return true;
        }

        public async Task<IReadOnlyCollection<ElectricityReadingDto>> GetAllAsync(CancellationToken cancellationToken = default)
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
                .Select(w => _electricityReadingMapper.ToDto(w))
                .ToArray();
        }

        public async Task<ElectricityReadingDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingElectricityReadingByIdFromDb(_logger, id);

            // загружаем entity со всеми деталями для передачи клиенту в UI
            ElectricityReading? entity = await FindEntityAsync(id, includeDetails: true, cancellationToken: cancellationToken);
            if (entity is null)
            {
                LogElectricityReadingNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Показание счетчика электроэнергии с ID {id} не найдено.");
            }

            return _electricityReadingMapper.ToDto(entity);
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

            return await query.SingleOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        private async Task LoadDetailsAsync(ElectricityReading entity, CancellationToken cancellationToken)
        {
            // Последовательно дозагружаем обе связанные сущности по ссылке в рамках одной транзакции трекера
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
