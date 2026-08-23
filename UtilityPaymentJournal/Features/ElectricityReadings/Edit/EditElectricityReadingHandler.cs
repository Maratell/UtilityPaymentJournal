using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ElectricityReadings;

namespace UtilityPaymentJournal.Features.ElectricityReadings.Edit
{
    /// <summary>
    /// Обработчик команды редактирования показания счетчика электроэнергии.
    /// </summary>
    public partial class EditElectricityReadingHandler(
            ApplicationDbContext context,
            ILogger<EditElectricityReadingHandler> logger) : IRequestHandler<EditElectricityReadingCommand, EditElectricityReadingResponse>
    {
        public async Task<EditElectricityReadingResponse> Handle(EditElectricityReadingCommand command, CancellationToken cancellationToken)
        {
            LogElectricityReadingUpdateRequested(logger, command.Id, command.CurrentValue);

            // Загружаем "легковесное" entity без связанных деталей по уникальному первичному ключу
            ElectricityReading? entity = await context.ElectricityReadings
                .SingleOrDefaultAsync(r => r.Id == command.Id, cancellationToken);

            if (entity == null)
            {
                LogElectricityReadingNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Показание счетчика электроэнергии с ID {command.Id} не найдено в базе данных.");
            }

            command.UpdateEntity(entity);
            await context.SaveChangesAsync(cancellationToken);

            LogElectricityReadingUpdatedInDb(logger, entity.Id);
            return entity.ToResponse();
        }
    }
}
