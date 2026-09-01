using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ElectricityReadings;

namespace UtilityPaymentJournal.Features.ElectricityReadings.GetList
{
    /// <summary>
    /// Обработчик запроса на получение списка показаний счетчиков электроэнергии.
    /// </summary>
    public partial class GetElectricityReadingsListHandler(
            IApplicationDbContext context,
            ILogger<GetElectricityReadingsListHandler> logger) : IRequestHandler<GetElectricityReadingsListQuery, GetElectricityReadingsListResponse>
    {
        public async Task<GetElectricityReadingsListResponse> Handle(GetElectricityReadingsListQuery query, CancellationToken cancellationToken)
        {
            LogFetchingAllElectricityReadingsFromDb(logger);

            // Извлекаем данные из БД с жадной загрузкой (Eager Loading) связанных объектов
            List<ElectricityReading> entities = await context.ElectricityReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            LogFetchedAllElectricityReadingsFromDbCount(logger, entities.Count);

            return entities.ToResponse();
        }
    }
}
