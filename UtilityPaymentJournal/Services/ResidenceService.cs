using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTOs.Residences;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Services
{
    public class ResidenceService : IResidenceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResidenceMapper _residenceMapper;

        public ResidenceService(
            ApplicationDbContext context,
            IResidenceMapper residenceMapper)
        {
            _context = context;
            _residenceMapper = residenceMapper;
        }

        public async Task<ResidenceDto> CreateAsync(CreateResidenceDto createDto, CancellationToken cancellationToken = default)
        {
            Residence entity = _residenceMapper.ToEntity(createDto);

            // используем синхронный Add, так как операция происходит в памяти
            _context.Residences.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _residenceMapper.ToDto(entity);
        }

        public async Task<ResidenceDto?> EditAsync(long id, EditResidenceDto editDto, CancellationToken cancellationToken = default)
        {
            Residence? entity = await FindEntityAsync(id, cancellationToken);
            if (entity == null)
                return null;

            _residenceMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _residenceMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            // EF Core сразу генерирует SQL-запрос: DELETE FROM Residences WHERE Id = @id
            // Метод возвращает количество удаленных строк (0 или 1)
            int deletedRowsCount = await _context.Residences
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            // Если удалена 1 строка — возвращаем true, если 0 (id не найден) — возвращаем false
            return deletedRowsCount > 0;
        }



        public async Task<IReadOnlyCollection<ResidenceDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            List<Residence> entities = await _context.Residences
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return entities
                .Select(e => _residenceMapper.ToDto(e))
                .ToList();
        }

        public async Task<ResidenceDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            // загружаем entity со всеми деталями для передачи клиенту в UI
            Residence? entity = await FindEntityAsync(id, cancellationToken);

            return entity is null ? null : _residenceMapper.ToDto(entity);
        }

        private async Task<Residence?> FindEntityAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.Residences
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
