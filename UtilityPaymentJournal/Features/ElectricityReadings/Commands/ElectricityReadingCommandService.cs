using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.Features.ElectricityReadings.Queries;
using UtilityPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Features.ElectricityReadings.Commands
{
    /// <summary>
    /// Сервис команд (записи) показаний счетчиков электроэнергии.
    /// Отвечает исключительно за модификацию состояния базы данных (Create/Update/Delete).
    /// </summary>
    public partial class ElectricityReadingCommandService : IElectricityReadingCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly IElectricityReadingMapper _electricityReadingMapper;
        private readonly ILogger<ElectricityReadingCommandService> _logger;

        public ElectricityReadingCommandService(
            ApplicationDbContext context,
            IElectricityReadingMapper electricityReadingMapper,
            ILogger<ElectricityReadingCommandService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _electricityReadingMapper = electricityReadingMapper ?? throw new ArgumentNullException(nameof(electricityReadingMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ElectricityReadingCommandResultDto> CreateAsync(CreateElectricityReadingDto createDto, CancellationToken cancellationToken = default)
        {
            LogElectricityReadingCreationRequested(_logger, createDto.CurrentValue);
            ElectricityReading entity = _electricityReadingMapper.ToEntity(createDto);

            // Используем синхронный Add, так как операция происходит в памяти
            _context.ElectricityReadings.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogElectricityReadingCreatedInDb(_logger, entity.Id);
            return _electricityReadingMapper.ToCommandResultDto(entity);
        }

        public async Task<ElectricityReadingCommandResultDto> EditAsync(long id, EditElectricityReadingDto editDto, CancellationToken cancellationToken = default)
        {
            LogElectricityReadingUpdateRequested(_logger, id, editDto.CurrentValue);

            // Загружаем "легковесное" entity без связанных деталей по уникальному первичному ключу
            ElectricityReading? entity = await _context.ElectricityReadings
                .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (entity == null)
            {
                LogElectricityReadingNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Показание счетчика электроэнергии с ID {id} не найдено в базе данных.");
            }

            _electricityReadingMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogElectricityReadingUpdatedInDb(_logger, id);
            return _electricityReadingMapper.ToCommandResultDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogElectricityReadingDeletionRequested(_logger, id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM ElectricityReadings WHERE Id = @id
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
    }
}
