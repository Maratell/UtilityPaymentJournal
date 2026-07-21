using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.UtilityProviders.Commands
{
    /// <summary>
    /// Сервис команд (записи) для управления поставщиками коммунальных услуг.
    /// Отвечает исключительно за модификацию состояния базы данных (Create/Update/Delete).
    /// </summary>
    public partial class UtilityProviderCommandService : IUtilityProviderCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityProviderMapper _utilityProviderMapper;
        private readonly ILogger<UtilityProviderCommandService> _logger;

        public UtilityProviderCommandService(
            ApplicationDbContext context,
            IUtilityProviderMapper utilityProviderMapper,
            ILogger<UtilityProviderCommandService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _utilityProviderMapper = utilityProviderMapper ?? throw new ArgumentNullException(nameof(utilityProviderMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UtilityProviderCommandResultDto> CreateAsync(CreateUtilityProviderDto createDto, CancellationToken cancellationToken = default)
        {
            LogUtilityProviderCreationRequested(_logger, createDto.Name);
            UtilityProvider entity = _utilityProviderMapper.ToEntity(createDto);

            // Используем синхронный Add, так как операция происходит в памяти
            _context.UtilityProviders.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogUtilityProviderCreatedInDb(_logger, entity.Id);
            return _utilityProviderMapper.ToCommandResultDto(entity);
        }

        public async Task<UtilityProviderCommandResultDto> EditAsync(long id, EditUtilityProviderDto editDto, CancellationToken cancellationToken = default)
        {
            LogUtilityProviderUpdateRequested(_logger, id, editDto.Name);

            UtilityProvider? entity = await _context.UtilityProviders
                .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (entity == null)
            {
                LogUtilityProviderNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Поставщик коммунальных услуг с ID {id} не найден в базе данных.");
            }

            _utilityProviderMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogUtilityProviderUpdatedInDb(_logger, id);
            return _utilityProviderMapper.ToCommandResultDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogUtilityProviderDeletionRequested(_logger, id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM UtilityProviders WHERE Id = @id
            int deletedRowsCount = await _context.UtilityProviders
                .Where(p => p.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogUtilityProviderNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Не удалось удалить. Поставщик коммунальных услуг с ID {id} не найден.");
            }

            LogUtilityProviderDeletedFromDb(_logger, id);
            return true;
        }
    }
}
