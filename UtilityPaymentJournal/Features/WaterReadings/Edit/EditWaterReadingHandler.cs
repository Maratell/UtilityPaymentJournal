using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.WaterReadings;

namespace UtilityPaymentJournal.Features.WaterReadings.Edit
{
    /// <summary>
    /// Обработчик команды редактирования показания счетчика воды.
    /// </summary>
    public partial class EditWaterReadingHandler(
            ApplicationDbContext context,
            ILogger<EditWaterReadingHandler> logger) : IRequestHandler<EditWaterReadingCommand, EditWaterReadingResponse>
    {
        public async Task<EditWaterReadingResponse> Handle(EditWaterReadingCommand command, CancellationToken cancellationToken)
        {
            LogWaterReadingUpdateRequested(logger, command.Id, command.CurrentValue);

            // Загружаем "легковесное" entity без связанных деталей по уникальному первичному ключу
            WaterReading? entity = await context.WaterReadings
                .SingleOrDefaultAsync(r => r.Id == command.Id, cancellationToken);

            if (entity == null)
            {
                LogWaterReadingNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Показание счетчика воды с ID {command.Id} не найдено в базе данных.");
            }

            command.UpdateEntity(entity);
            await context.SaveChangesAsync(cancellationToken);

            LogWaterReadingUpdatedInDb(logger, entity.Id);
            return entity.ToResponse();
        }
    }
}
