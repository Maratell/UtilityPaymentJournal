using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.Commands
{
    /// <summary>
    /// Сервис команд (записи) коммунальных услуг.
    /// Отвечает исключительно за модификацию состояния базы данных (Create/Update/Delete).
    /// </summary>
    public partial class UtilityCommandService : IUtilityCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityMapper _utilityMapper;
        private readonly ILogger<UtilityCommandService> _logger;

        public UtilityCommandService(
            ApplicationDbContext context,
            IUtilityMapper utilityMapper,
            ILogger<UtilityCommandService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _utilityMapper = utilityMapper ?? throw new ArgumentNullException(nameof(utilityMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UtilityCommandResultDto> CreateAsync(CreateUtilityDto createDto, CancellationToken cancellationToken = default)
        {
            LogUtilityCreationRequested(_logger, createDto.Name);
            Utility entity = _utilityMapper.ToEntity(createDto);

            // Используем синхронный Add, так как операция добавления в DbSet происходит в памяти
            _context.Utilities.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogUtilityCreatedInDb(_logger, entity.Id);
            return _utilityMapper.ToCommandResultDto(entity);
        }

        public async Task<UtilityCommandResultDto> EditAsync(long id, EditUtilityDto editDto, CancellationToken cancellationToken = default)
        {
            LogUtilityUpdateRequested(_logger, id, editDto.Name);

            // Загружаем "легковесное" entity без связанных деталей по уникальному первичному ключу
            Utility? entity = await _context.Utilities
                .SingleOrDefaultAsync(u => u.Id == id, cancellationToken);

            if (entity == null)
            {
                LogUtilityNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Коммунальной услуги с ID {id} не найдено.");
            }

            _utilityMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogUtilityUpdatedInDb(_logger, id);
            return _utilityMapper.ToCommandResultDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogUtilityDeletionRequested(_logger, id);

            // Высокопроизводительное удаление: EF Core сразу генерирует SQL-запрос DELETE без загрузки сущности в память
            int deletedRowsCount = await _context.Utilities
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogUtilityNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Не удалось удалить. Коммунальная услуга с ID {id} не найдена.");
            }

            LogUtilityDeletedFromDb(_logger, id);
            return true;
        }
    }
}
