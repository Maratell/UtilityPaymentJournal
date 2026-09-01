using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;

namespace UtilityPaymentJournal.Features.ComplaintBoard.GetList
{
    public partial class GetComplaintsListHandler(
            IApplicationDbContext context,
            ILogger<GetComplaintsListHandler> logger) : IRequestHandler<GetComplaintsListQuery, GetComplaintsListResponse>
    {
        public async Task<GetComplaintsListResponse> Handle(GetComplaintsListQuery query, CancellationToken cancellationToken)
        {
            LogFetchingAllComplaintsFromDb(logger);

            List<Complaint> entities = await context.Complaints
                .Include(w => w.Utility)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllComplaintsFromDbCount(logger, entities.Count);

            return entities.ToResponse();
        }
    }
}
