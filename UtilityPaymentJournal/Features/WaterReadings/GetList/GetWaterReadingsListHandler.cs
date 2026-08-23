using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.WaterReadings;

namespace UtilityPaymentJournal.Features.WaterReadings.GetList
{
    /// <summary>
    /// Обработчик запроса на получение списка показаний счетчиков воды.
    /// </summary>
    public partial class GetWaterReadingsListHandler(
            ApplicationDbContext context,
            ILogger<GetWaterReadingsListHandler> logger) : IRequestHandler<GetWaterReadingsListQuery, GetWaterReadingsListResponse>
    {
        public async Task<GetWaterReadingsListResponse> Handle(GetWaterReadingsListQuery query, CancellationToken cancellationToken)
        {
            LogFetchingAllWaterReadingsFromDb(logger);

            // Извлекаем данные из БД с жадной загрузкой (Eager Loading) связанных объектов
            List<WaterReading> entities = await context.WaterReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllWaterReadingsFromDbCount(logger, entities.Count);

            return entities.ToResponse();
        }
    }
}
