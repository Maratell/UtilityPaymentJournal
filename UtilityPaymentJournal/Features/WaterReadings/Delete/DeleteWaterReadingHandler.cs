using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;

namespace UtilityPaymentJournal.Features.WaterReadings.Delete
{
    /// <summary>
    /// Обработчик команды удаления показания счетчика воды.
    /// Напрямую удаляет запись из PostgreSQL без предварительной загрузки в память.
    /// </summary>
    public partial class DeleteWaterReadingHandler(
            IApplicationDbContext context,
            ILogger<DeleteWaterReadingHandler> logger) : IRequestHandler<DeleteWaterReadingCommand>
    {
        public async Task Handle(DeleteWaterReadingCommand command, CancellationToken cancellationToken)
        {
            LogWaterReadingDeletionRequested(logger, command.Id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM WaterReadings WHERE Id = @id
            int deletedRowsCount = await context.WaterReadings
                .Where(w => w.Id == command.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogWaterReadingNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Не удалось удалить. Показание счетчика воды с ID {command.Id} не найдено.");
            }

            LogWaterReadingDeletedFromDb(logger, command.Id);
        }
    }
}
