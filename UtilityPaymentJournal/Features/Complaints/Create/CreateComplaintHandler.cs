using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Complaints.Create
{
    public partial class CreateComplaintHandler(
        ApplicationDbContext context,
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
