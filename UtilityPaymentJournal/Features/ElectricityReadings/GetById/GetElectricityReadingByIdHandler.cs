using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ElectricityReadings;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UtilityPaymentJournal.Features.ElectricityReadings.GetById
{
    /// <summary>
    /// Обработчик запроса на получение деталей показания счетчика электроэнергии.
    /// Инкапсулирует логику эффективного чтения из PostgreSQL.
    /// </summary>
    public partial class GetElectricityReadingByIdHandler(
            ApplicationDbContext context,
            ILogger<GetElectricityReadingByIdHandler> logger) : IRequestHandler<GetElectricityReadingByIdQuery, GetElectricityReadingByIdResponse>
    {
        public async Task<GetElectricityReadingByIdResponse> Handle(GetElectricityReadingByIdQuery query, CancellationToken cancellationToken)
        {
            LogFetchingElectricityReadingByIdFromDb(logger, query.Id);

            // Загружаем entity со всеми деталями (Eager Loading)
            ElectricityReading? entity = await context.ElectricityReadings
                .Include(w => w.Residence)
                .Include(w => w.UtilityProvider)
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Id == query.Id, cancellationToken);

            if (entity is null)
            {
                LogElectricityReadingNotFoundInDb(logger, query.Id);
                throw new KeyNotFoundException($"Показание счетчика электроэнергии с ID {query.Id} не найдено.");
            }

            return entity.ToResponse();
        }
    }
}
