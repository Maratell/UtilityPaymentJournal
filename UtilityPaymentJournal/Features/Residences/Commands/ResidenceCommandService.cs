using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.Commands
{
    /// <summary>
    /// Сервис команд (записи) показаний счетчиков электроэнергии.
    /// Отвечает исключительно за модификацию состояния базы данных (Create/Update/Delete).
    /// </summary>
    public partial class ResidenceCommandService : IResidenceCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly IResidenceMapper _residenceMapper;
        private readonly ILogger<ResidenceCommandService> _logger;

        public ResidenceCommandService(
            ApplicationDbContext context,
            IResidenceMapper residenceMapper,
            ILogger<ResidenceCommandService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _residenceMapper = residenceMapper ?? throw new ArgumentNullException(nameof(residenceMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ResidenceCommandResultDto> CreateAsync(CreateResidenceDto createDto, CancellationToken cancellationToken = default)
        {
            LogResidenceCreationRequested(_logger, createDto.Address);
            Residence entity = _residenceMapper.ToEntity(createDto);

            // Используем синхронный Add, так как операция происходит в памяти
            _context.Residences.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogResidenceCreatedInDb(_logger, entity.Id);
            return _residenceMapper.ToCommandResultDto(entity);
        }

        public async Task<ResidenceCommandResultDto> EditAsync(long id, EditResidenceDto editDto, CancellationToken cancellationToken = default)
        {
            LogResidenceUpdateRequested(_logger, id, editDto.Address);

            Residence? entity = await _context.Residences
                .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (entity == null)
            {
                LogResidenceNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Показание счетчика электроэнергии с ID {id} не найдено в базе данных.");
            }

            _residenceMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogResidenceUpdatedInDb(_logger, id);
            return _residenceMapper.ToCommandResultDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogResidenceDeletionRequested(_logger, id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM ElectricityReadings WHERE Id = @id
            int deletedRowsCount = await _context.Residences
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogResidenceNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Не удалось удалить. Показание счетчика электроэнергии с ID {id} не найдено.");
            }

            LogResidenceDeletedFromDb(_logger, id);
            return true;
        }
    }
}
