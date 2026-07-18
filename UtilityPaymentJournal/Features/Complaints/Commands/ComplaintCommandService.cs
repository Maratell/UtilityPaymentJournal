using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.ComplaintBoard;

namespace UtilityPaymentJournal.Features.Complaints.Commands
{
    /// <summary>
    /// Сервис команд (записи) для управления жалобами.
    /// Отвечает исключительно за модификацию состояния базы данных (Create/Update/Delete).
    /// </summary>
    public partial class ComplaintCommandService : IComplaintCommandService
    {
        private readonly ApplicationDbContext _context;
        private readonly IComplaintMapper _complaintMapper;
        private readonly ILogger<ComplaintCommandService> _logger;

        public ComplaintCommandService(
            ApplicationDbContext context,
            IComplaintMapper complaintMapper,
            ILogger<ComplaintCommandService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _complaintMapper = complaintMapper ?? throw new ArgumentNullException(nameof(complaintMapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ComplaintCommandResultDto> CreateAsync(CreateComplaintDto createDto, CancellationToken cancellationToken = default)
        {
            LogComplaintCreationRequested(_logger, createDto.Title);
            Complaint entity = _complaintMapper.ToEntity(createDto);

            _context.Complaints.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogComplaintCreatedInDb(_logger, entity.Id);
            return _complaintMapper.ToCommandResultDto(entity);
        }

        public async Task<ComplaintCommandResultDto> EditAsync(long id, EditComplaintDto editDto, CancellationToken cancellationToken = default)
        {
            LogComplaintUpdateRequested(_logger, id, editDto.Title);

            Complaint? entity = await _context.Complaints
                .SingleOrDefaultAsync(r => r.Id == id, cancellationToken);

            if (entity == null)
            {
                LogComplaintNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Жалоба с ID {id} не найдена в базе данных.");
            }

            _complaintMapper.UpdateEntity(editDto, entity);
            await _context.SaveChangesAsync(cancellationToken);

            LogComplaintUpdatedInDb(_logger, id);
            return _complaintMapper.ToCommandResultDto(entity);
        }

        public async Task<ComplaintCommandResultDto> ChangeStatusAsync(ChangeComplaintStatusDto changeStatusDto, CancellationToken cancellationToken = default)
        {
            LogComplaintStatusChangeRequested(_logger, changeStatusDto.Id, changeStatusDto.NewStatus);

            Complaint? entity = await _context.Complaints
                .SingleOrDefaultAsync(r => r.Id == changeStatusDto.Id, cancellationToken);

            if (entity == null)
            {
                LogComplaintNotFoundInDb(_logger, changeStatusDto.Id);
                throw new KeyNotFoundException($"Не удалось изменить статус. Жалоба с ID {changeStatusDto.Id} не найдена.");
            }

            entity.Status = changeStatusDto.NewStatus;
            if (changeStatusDto.NewStatus == ComplaintStatus.Resolved)
            {
                entity.IssueResolutionDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(cancellationToken);

            LogComplaintStatusChangedInDb(_logger, changeStatusDto.Id, changeStatusDto.NewStatus);
            return _complaintMapper.ToCommandResultDto(entity);
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
        {
            LogComplaintDeletionRequested(_logger, id);

            int deletedRowsCount = await _context.Complaints
                .Where(w => w.Id == id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogComplaintNotFoundInDb(_logger, id);
                throw new KeyNotFoundException($"Не удалось удалить. Жалоба с ID {id} не найдена.");
            }

            LogComplaintDeletedFromDb(_logger, id);
            return true;
        }
    }
}
