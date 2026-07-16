using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.DTOs.Residences;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Interface.Service;

namespace UtilityPaymentJournal.Services
{
    public partial class ResidenceService : IResidenceService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResidenceMapper _residenceMapper;
        private readonly ILogger<ResidenceService> _logger;

        public ResidenceService(
            ApplicationDbContext context,
            IResidenceMapper residenceMapper,
            ILogger<ResidenceService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _residenceMapper = residenceMapper ?? throw new ArgumentNullException(nameof(residenceMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResidenceDto> CreateAsync(CreateResidenceDto createDto, CancellationToken cancellationToken = default)
        {
            LogResidenceCreationRequested(_logger, createDto.Address);
            Residence entity = _residenceMapper.ToEntity(createDto);

            // используем синхронный Add, так как операция происходит в памяти
            _context.Residences.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogResidenceCreatedInDb(_logger, entity.Id);
            return _residenceMapper.ToDto(entity);
        }

        public async Task<ResidenceDto> EditAsync(long id, EditResidenceDto editDto, CancellationToken cancellationToken = default)
        {
            LogResidenceUpdateRequested(_logger, id, editDto.Address);

            Residence? entity = await FindEntityAsync(id, cancellationToken);
            if (entity == null)
            {
                LogResidenceNotFoundInDb(_logger, id);
                // Исключение автоматически перехватит кастомный NotFoundExceptionHandler
                throw new KeyNotFoundException($"Жилой объект с ID {id} не найден в базе данных.");
            }

            _residenceMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogResidenceUpdatedInDb(_logger, id);
            return _residenceMapper.ToDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogResidenceDeletionRequested(_logger, id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM Residences WHERE Id = @id
            // Метод возвращает количество удаленных строк (0 или 1)
            int deletedRowsCount = await _context.Residences
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogResidenceNotFoundInDb(_logger, id);
                // Исключение автоматически перехватит кастомный NotFoundExceptionHandler
                throw new KeyNotFoundException($"Не удалось удалить. Жилой объект с ID {id} не найден.");
            }

            LogResidenceDeletedFromDb(_logger, id);
            return true;
        }



        public async Task<IReadOnlyCollection<ResidenceDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingAllResidencesFromDb(_logger);

            List<Residence> entities = await _context.Residences
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllResidencesFromDbCount(_logger, entities.Count);

            return entities
                .Select(e => _residenceMapper.ToDto(e))
                .ToArray();
        }

        public async Task<ResidenceDto> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            LogFetchingResidenceByIdFromDb(_logger, id);

            // загружаем entity со всеми деталями для передачи клиенту в UI
            Residence? entity = await FindEntityAsync(id, cancellationToken);
            if (entity is null)
            {
                LogResidenceNotFoundInDb(_logger, id);
                // Исключение автоматически перехватит кастомный NotFoundExceptionHandler
                throw new KeyNotFoundException($"Жилой объект с ID {id} не найден.");
            }

            return _residenceMapper.ToDto(entity);
        }

        private async Task<Residence?> FindEntityAsync(long id, CancellationToken cancellationToken)
        {
            return await _context.Residences
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }
    }
}
