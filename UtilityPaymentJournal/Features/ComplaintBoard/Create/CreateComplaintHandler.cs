using MediatR;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;

namespace UtilityPaymentJournal.Features.ComplaintBoard.Create
{
    public partial class CreateComplaintHandler(
        IApplicationDbContext context,
        ILogger<CreateComplaintHandler> logger) : IRequestHandler<CreateComplaintCommand, CreateComplaintResponse>
    {
        public async Task<CreateComplaintResponse> Handle(CreateComplaintCommand command, CancellationToken cancellationToken)
        {
            LogComplaintCreationRequested(logger, command.Title);
            Complaint entity = command.ToEntity();

            context.Complaints.Add(entity);
            await context.SaveChangesAsync(cancellationToken);

            LogComplaintCreatedInDb(logger, entity.Id);
            return entity.ToResponse();
        }
    }
}
