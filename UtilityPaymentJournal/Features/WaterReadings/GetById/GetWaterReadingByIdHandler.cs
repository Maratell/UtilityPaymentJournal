using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.WaterReadings;

namespace UtilityPaymentJournal.Features.WaterReadings.GetById
{
    /// <summary>
    /// Обработчик запроса на получение деталей показания счетчика воды.
    /// Инкапсулирует логику эффективного чтения из PostgreSQL.
    /// </summary>
    public partial class GetWaterReadingByIdHandler(
            ApplicationDbContext context,
            ILogger<GetWaterReadingByIdHandler> logger) : IRequestHandler<GetWaterReadingByIdQuery, GetWaterReadingByIdResponse>
    {
        public async Task<GetWaterReadingByIdResponse> Handle(GetWaterReadingByIdQuery query, CancellationToken cancellationToken)
        {
            LogFetchingWaterReadingByIdFromDb(logger, query.Id);

            // Загружаем entity со всеми деталями (Eager Loading) для передачи клиенту в UI
            WaterReading? entity = await context.WaterReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Id == query.Id, cancellationToken);

            if (entity is null)
            {
                LogWaterReadingNotFoundInDb(logger, query.Id);
                throw new KeyNotFoundException($"Показание счетчика воды с ID {query.Id} не найдено.");
            }

            return entity.ToResponse();
        }
    }
}
