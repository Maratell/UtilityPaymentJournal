using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.WaterReadings;

namespace UtilityPaymentJournal.Features.WaterReadings.Create
{
    /// <summary>
    /// Обработчик команды создания показания счетчика воды.
    /// Инкапсулирует в себе всю бизнес-логику и запись в базу данных PostgreSQL для этой фичи.
    /// </summary>
    public partial class CreateWaterReadingHandler(
        IApplicationDbContext context,
        ILogger<CreateWaterReadingHandler> logger) : IRequestHandler<CreateWaterReadingCommand, CreateWaterReadingResponse>
    {
        public async Task<CreateWaterReadingResponse> Handle(CreateWaterReadingCommand command, CancellationToken cancellationToken)
        {
            LogWaterReadingCreationRequested(logger, command.CurrentValue);
            WaterReading entity = command.ToEntity();

            // Используем синхронный Add, так как операция происходит в памяти
            context.WaterReadings.Add(entity);
            await context.SaveChangesAsync(cancellationToken);

            LogWaterReadingCreatedInDb(logger, entity.Id);
            return entity.ToResponse();
        }
    }
}
