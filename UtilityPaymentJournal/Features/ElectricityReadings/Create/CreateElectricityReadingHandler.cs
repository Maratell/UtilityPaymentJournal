using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ElectricityReadings;

namespace UtilityPaymentJournal.Features.ElectricityReadings.Create
{
    /// <summary>
    /// Обработчик команды создания показания счетчика электроэнергии.
    /// Инкапсулирует в себе всю бизнес-логику и запись в базу данных PostgreSQL для этой фичи.
    /// </summary>
    public partial class CreateElectricityReadingHandler(
        ApplicationDbContext context,
        ILogger<CreateElectricityReadingHandler> logger) : IRequestHandler<CreateElectricityReadingCommand, CreateElectricityReadingResponse>
    {
        public async Task<CreateElectricityReadingResponse> Handle(CreateElectricityReadingCommand command, CancellationToken cancellationToken)
        {
            LogElectricityReadingCreationRequested(logger, command.CurrentValue);
            ElectricityReading entity = command.ToEntity();

            // Используем синхронный Add, так как операция происходит в памяти
            context.ElectricityReadings.Add(entity);
            await context.SaveChangesAsync(cancellationToken);

            LogElectricityReadingCreatedInDb(logger, entity.Id);
            return entity.ToResponse();
        }
    }
}
