using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.GetList
{
    /// <summary>
    /// Обработчик запроса на получение списка объектов недвижимости.
    /// </summary>
    public partial class GetResidencesListHandler(
            ApplicationDbContext context,
            ILogger<GetResidencesListHandler> logger) : IRequestHandler<GetResidencesListQuery, GetResidencesListResponse>
    {
        public async Task<GetResidencesListResponse> Handle(GetResidencesListQuery query, CancellationToken cancellationToken)
        {
            LogFetchingAllResidencesFromDb(logger);

            List<Residence> entities = await context.Residences
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllResidencesFromDbCount(logger, entities.Count);

            return entities.ToResponse();
        }
    }
}
