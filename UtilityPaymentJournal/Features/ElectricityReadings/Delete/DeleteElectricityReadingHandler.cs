using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using UtilityPaymentJournal.Infrastructure.EF.Context;

namespace UtilityPaymentJournal.Features.ElectricityReadings.Delete
{
    /// <summary>
    /// Обработчик команды удаления показания счетчика электроэнергии.
    /// Напрямую удаляет запись из PostgreSQL без предварительной загрузки в память.
    /// </summary>
    public partial class DeleteElectricityReadingHandler(
            ApplicationDbContext context,
            ILogger<DeleteElectricityReadingHandler> logger) : IRequestHandler<DeleteElectricityReadingCommand>
    {
        public async Task Handle(DeleteElectricityReadingCommand command, CancellationToken cancellationToken)
        {
            LogElectricityReadingDeletionRequested(logger, command.Id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM ElectricityReadings WHERE Id = @id
            int deletedRowsCount = await context.ElectricityReadings
                .Where(w => w.Id == command.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogElectricityReadingNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Не удалось удалить. Показание счетчика электроэнергии с ID {command.Id} не найдено.");
            }

            LogElectricityReadingDeletedFromDb(logger, command.Id);
        }
    }
}
