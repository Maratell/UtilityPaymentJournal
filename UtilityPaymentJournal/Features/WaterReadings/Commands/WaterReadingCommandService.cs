using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.WaterReadings;
using UtilityPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Features.WaterReadings.Commands
{
    /// <summary>
    /// Сервис команд (записи) показаний счетчиков воды.
    /// Отвечает исключительно за модификацию состояния базы данных (Create/Update/Delete).
    /// </summary>
    public partial class WaterReadingCommandService : IWaterReadingCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWaterReadingMapper _waterReadingMapper;
        private readonly ILogger<WaterReadingCommandService> _logger;

        public WaterReadingCommandService(
            ApplicationDbContext context,
            IWaterReadingMapper waterReadingMapper,
            ILogger<WaterReadingCommandService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _waterReadingMapper = waterReadingMapper ?? throw new ArgumentNullException(nameof(waterReadingMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<WaterReadingCommandResultDto> CreateAsync(CreateWaterReadingDto createDto, CancellationToken cancellationToken = default)
        {
            LogWaterReadingCreationRequested(_logger, createDto.CurrentValue);
            WaterReading entity = _waterReadingMapper.ToEntity(createDto);

            // Используем синхронный Add, так как операция происходит в памяти
            _context.WaterReadings.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogWaterReadingCreatedInDb(_logger, entity.Id);
            return _waterReadingMapper.ToCommandResultDto(entity);
        }

        public async Task<WaterReadingCommandResultDto> EditAsync(long id, EditWaterReadingDto editDto, CancellationToken cancellationToken = default)
        {
            LogWaterReadingUpdateRequested(_logger, id, editDto.CurrentValue);

            // Загружаем "легковесное" entity без связанных деталей по уникальному первичному ключу
            WaterReading? entity = await _context.WaterReadings
                .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (entity == null)
            {
                LogWaterReadingNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Показание счетчика воды с ID {id} не найдено в базе данных.");
            }

            _waterReadingMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogWaterReadingUpdatedInDb(_logger, id);
            return _waterReadingMapper.ToCommandResultDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogWaterReadingDeletionRequested(_logger, id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM WaterReadings WHERE Id = @id
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
    }
}
