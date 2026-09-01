using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Common.Specifications;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.GetList
{
    public partial class GetUtilitiesListHandler(
            IApplicationDbContext context,
            ILogger<GetUtilitiesListHandler> logger) : IRequestHandler<GetUtilitiesListQuery, GetUtilitiesListResponse>
    {
        public async Task<GetUtilitiesListResponse> Handle(GetUtilitiesListQuery query, CancellationToken cancellationToken)
        {
            LogFetchingAllUtilitiesFromDb(logger);

            // Собираем фильтр и спецификацию (фильтруем по признаку активности услуги)
            UtilityQueryFilter filter = new UtilityQueryFilter(query.IsActive);
            ICriteriaSpecification<Utility> criteria = new UtilityFilterSpecification(filter);

            // Декларативно строим запрос к таблице, накладывая спецификацию критериев отбора
            List<Utility> entities = await context.Utilities
                .AsNoTracking()
                .FilterWith(criteria)
                .ToListAsync(cancellationToken);

            LogFetchedAllUtilitiesFromDbCount(logger, entities.Count);

            return entities.ToResponse();
        }
    }
}
