using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTOs.Utilities;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;


namespace UtilityPaymentJournal.Services
{
    public class UtilityService : IUtilityService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUtilityMapper _utilityMapper;

        public UtilityService(
            ApplicationDbContext context,
            IUtilityMapper utilityMapper)
        {
            _context = context;
            _utilityMapper = utilityMapper;
        }

        public async Task<UtilityDto> CreateAsync(CreateUtilityDto createDto, CancellationToken cancellationToken = default)
        {
            Utility entity = _utilityMapper.ToEntity(createDto);

            // используем синхронный Add, так как операция происходит в памяти
            _context.Utilities.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _utilityMapper.ToDto(entity);
        }

        public async Task<UtilityDto?> EditAsync(long id, EditUtilityDto editDto, CancellationToken cancellationToken = default)
        {
            Utility? entity = await FindEntityAsync(id, cancellationToken);
            if (entity == null)
                return null;

            _utilityMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _utilityMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            // EF Core сразу генерирует SQL-запрос: DELETE FROM Utilities WHERE Id = @id
            // Метод возвращает количество удаленных строк (0 или 1)
            int deletedRowsCount = await _context.Utilities
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            // Если удалена 1 строка — возвращаем true, если 0 (id не найден) — возвращаем false
            return deletedRowsCount > 0;
        }

        public async Task<IReadOnlyCollection<UtilityDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            List<Utility> entities = await _context.Utilities
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return entities
                .Select(e => _utilityMapper.ToDto(e))
                .ToList();
        }

        public async Task<UtilityDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            // загружаем entity со всеми деталями для передачи клиенту в UI
            Utility? entity = await FindEntityAsync(id, cancellationToken);

            return entity is null ? null : _utilityMapper.ToDto(entity);
        }

        private async Task<Utility?> FindEntityAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.Utilities
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
