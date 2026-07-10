using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTOs.UtilityProviders;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Interface.Service;
using UtilityProviderPaymentJournal.Interface.Mapping;

namespace UtilityPaymentJournal.Services
{
    public class UtilityProviderService : IUtilityProviderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityProviderMapper _utilityProviderMapper;

        public UtilityProviderService(
            ApplicationDbContext context,
            IUtilityProviderMapper utilityProviderMapper)
        {
            _context = context;
            _utilityProviderMapper = utilityProviderMapper;
        }

        public async Task<UtilityProviderDTO> CreateAsync(CreateUtilityProviderDTO createDto, CancellationToken cancellationToken = default)
        {
            UtilityProvider entity = _utilityProviderMapper.ToEntity(createDto);

            // используем синхронный Add, так как операция происходит в памяти
            _context.UtilityProviders.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _utilityProviderMapper.ToDto(entity);
        }

        public async Task<UtilityProviderDTO?> EditAsync(long id, EditUtilityProviderDTO editDto, CancellationToken cancellationToken = default)
        {
            UtilityProvider? entity = await FindEntityAsync(id, cancellationToken);
            if (entity == null)
                return null;

            _utilityProviderMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _utilityProviderMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            // EF Core сразу генерирует SQL-запрос: DELETE FROM UtilityProviders WHERE Id = @id
            // Метод возвращает количество удаленных строк (0 или 1)
            int deletedRowsCount = await _context.UtilityProviders
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            // Если удалена 1 строка — возвращаем true, если 0 (id не найден) — возвращаем false
            return deletedRowsCount > 0;
        }

        public async Task<IReadOnlyCollection<UtilityProviderDTO>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            List<UtilityProvider> entities = await _context.UtilityProviders
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return entities
                .Select(e => _utilityProviderMapper.ToDto(e))
                .ToList();
        }

        public async Task<UtilityProviderDTO?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            // загружаем entity со всеми деталями для передачи клиенту в UI
            UtilityProvider? entity = await FindEntityAsync(id, cancellationToken);

            return entity is null ? null : _utilityProviderMapper.ToDto(entity);
        }

        private async Task<UtilityProvider?> FindEntityAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.UtilityProviders
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
