using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.UtilityProviders.GetList
{
    public partial class GetUtilityProvidersListHandler(
            IApplicationDbContext context,
            ILogger<GetUtilityProvidersListHandler> logger) : IRequestHandler<GetUtilityProvidersListQuery, GetUtilityProvidersListResponse>
    {
        public async Task<GetUtilityProvidersListResponse> Handle(GetUtilityProvidersListQuery query, CancellationToken cancellationToken)
        {
            LogFetchingAllUtilityProvidersFromDb(logger);

            List<UtilityProvider> entities = await context.UtilityProviders
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllUtilityProvidersFromDbCount(logger, entities.Count);

            return entities.ToResponse();
        }
    }
}
