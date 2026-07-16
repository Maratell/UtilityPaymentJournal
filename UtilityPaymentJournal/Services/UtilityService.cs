using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTOs.Utilities;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;


namespace UtilityPaymentJournal.Services
{
    public partial class UtilityService : IUtilityService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityMapper _utilityMapper;
        private readonly ILogger<UtilityService> _logger;

        public UtilityService(
            ApplicationDbContext context,
            IUtilityMapper utilityMapper,
            ILogger<UtilityService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _utilityMapper = utilityMapper ?? throw new ArgumentNullException(nameof(utilityMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UtilityDto> CreateAsync(CreateUtilityDto createDto, CancellationToken cancellationToken = default)
        {
            LogUtilityCreationRequested(_logger, createDto.Name);
            Utility entity = _utilityMapper.ToEntity(createDto);

            // используем синхронный Add, так как операция происходит в памяти
            _context.Utilities.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogUtilityCreatedInDb(_logger, entity.Id);
            return _utilityMapper.ToDto(entity);
        }

        public async Task<UtilityDto> EditAsync(long id, EditUtilityDto editDto, CancellationToken cancellationToken = default)
        {
            LogUtilityUpdateRequested(_logger, id, editDto.Name);

            Utility? entity = await FindEntityAsync(id, cancellationToken);
            if (entity == null)
            {
                LogUtilityNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Коммунальная услуга с ID {id} не найдена в базе данных.");
            }

            _utilityMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogUtilityUpdatedInDb(_logger, id);
            return _utilityMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogUtilityDeletionRequested(_logger, id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM Utilities WHERE Id = @id
            // Метод возвращает количество удаленных строк (0 или 1)
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

        public async Task<IReadOnlyCollection<UtilityDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingAllUtilitiesFromDb(_logger);

            List<Utility> entities = await _context.Utilities
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllUtilitiesFromDbCount(_logger, entities.Count);

            return entities
                .Select(e => _utilityMapper.ToDto(e))
                .ToArray();
        }

        public async Task<UtilityDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingUtilityByIdFromDb(_logger, id);

            // загружаем entity со всеми деталями для передачи клиенту в UI
            Utility? entity = await FindEntityAsync(id, cancellationToken);
            if (entity is null)
            {
                LogUtilityNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Коммунальная услуга с ID {id} не найдена.");
            }

            return _utilityMapper.ToDto(entity);
        }

        private async Task<Utility?> FindEntityAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.Utilities
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
