using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTOs.UtilityProviders;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Interface.Service;
using UtilityProviderPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Services
{
    public partial class UtilityProviderService : IUtilityProviderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityProviderMapper _utilityProviderMapper;
        private readonly ILogger<UtilityProviderService> _logger;

        public UtilityProviderService(
            ApplicationDbContext context,
            IUtilityProviderMapper utilityProviderMapper,
            ILogger<UtilityProviderService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _utilityProviderMapper = utilityProviderMapper ?? throw new ArgumentNullException(nameof(utilityProviderMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UtilityProviderDto> CreateAsync(CreateUtilityProviderDto createDto, CancellationToken cancellationToken = default)
        {
            LogUtilityProviderCreationRequested(_logger, createDto.Name);
            UtilityProvider entity = _utilityProviderMapper.ToEntity(createDto);

            // используем синхронный Add, так как операция происходит в памяти
            _context.UtilityProviders.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogUtilityProviderCreatedInDb(_logger, entity.Id);
            return _utilityProviderMapper.ToDto(entity);
        }

        public async Task<UtilityProviderDto> EditAsync(long id, EditUtilityProviderDto editDto, CancellationToken cancellationToken = default)
        {
            LogUtilityProviderUpdateRequested(_logger, id, editDto.Name);

            UtilityProvider? entity = await FindEntityAsync(id, cancellationToken);
            if (entity == null)
            {
                LogUtilityProviderNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Поставщик коммунальных услуг с ID {id} не найден в базе данных.");
            }

            _utilityProviderMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogUtilityProviderUpdatedInDb(_logger, id);
            return _utilityProviderMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogUtilityProviderDeletionRequested(_logger, id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM UtilityProviders WHERE Id = @id
            // Метод возвращает количество удаленных строк (0 или 1)
            int deletedRowsCount = await _context.UtilityProviders
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogUtilityProviderNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Не удалось удалить. Поставщик коммунальных услуг с ID {id} не найден.");
            }

            LogUtilityProviderDeletedFromDb(_logger, id);
            return true;
        }

        public async Task<IReadOnlyCollection<UtilityProviderDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingAllUtilityProvidersFromDb(_logger);

            List<UtilityProvider> entities = await _context.UtilityProviders
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllUtilityProvidersFromDbCount(_logger, entities.Count);

            return entities
                .Select(e => _utilityProviderMapper.ToDto(e))
                .ToArray();
        }

        public async Task<UtilityProviderDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingUtilityProviderByIdFromDb(_logger, id);

            // загружаем entity со всеми деталями для передачи клиенту в UI
            UtilityProvider? entity = await FindEntityAsync(id, cancellationToken);
            if (entity is null)
            {
                LogUtilityProviderNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Поставщик коммунальных услуг с ID {id} не найден.");
            }

            return _utilityProviderMapper.ToDto(entity);
        }

        private async Task<UtilityProvider?> FindEntityAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.UtilityProviders
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
