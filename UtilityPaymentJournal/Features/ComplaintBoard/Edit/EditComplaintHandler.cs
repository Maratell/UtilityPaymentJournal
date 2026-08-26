using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;

namespace UtilityPaymentJournal.Features.ComplaintBoard.Edit
{
    public partial class EditComplaintHandler(
            ApplicationDbContext context,
            ILogger<EditComplaintHandler> logger) : IRequestHandler<EditComplaintCommand, EditComplaintResponse>
    {
        public async Task<EditComplaintResponse> Handle(EditComplaintCommand command, CancellationToken cancellationToken)
        {
            LogComplaintUpdateRequested(logger, command.Id, command.Title);

            Complaint? entity = await context.Complaints
                .SingleOrDefaultAsync(r => r.Id == command.Id, cancellationToken);

            if (entity == null)
            {
                LogComplaintNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Жалоба с ID {command.Id} не найдена в базе данных.");
            }

            command.UpdateEntity(entity);
            await context.SaveChangesAsync(cancellationToken);

            LogComplaintUpdatedInDb(logger, entity.Id);
            return entity.ToResponse();
        }
    }
}
