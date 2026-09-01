using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;

namespace UtilityPaymentJournal.Features.ComplaintBoard.ChangeStatus
{
    public partial class ChangeComplaintStatusHandler(
        IApplicationDbContext context,
        ILogger<ChangeComplaintStatusHandler> logger) : IRequestHandler<ChangeComplaintStatusCommand, ChangeComplaintStatusResponse>
    {
        public async Task<ChangeComplaintStatusResponse> Handle(ChangeComplaintStatusCommand command, CancellationToken cancellationToken)
        {
            LogComplaintStatusChangeRequested(logger, command.Id, command.NewStatus);

            Complaint? entity = await context.Complaints
                .SingleOrDefaultAsync(r => r.Id == command.Id, cancellationToken);

            if (entity == null)
            {
                LogComplaintNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Не удалось изменить статус. Жалоба с ID {command.Id} не найдена.");
            }

            entity.Status = command.NewStatus;
            if (command.NewStatus == ComplaintStatus.Resolved)
            {
                entity.IssueResolutionDate = DateTime.UtcNow;
            }

            await context.SaveChangesAsync(cancellationToken);

            LogComplaintStatusChangedInDb(logger, entity.Id, entity.Status);
            return entity.ToResponse();
        }
    }
}
