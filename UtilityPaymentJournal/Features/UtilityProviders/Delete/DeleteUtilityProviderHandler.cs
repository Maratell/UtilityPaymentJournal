using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;

namespace UtilityPaymentJournal.Features.UtilityProviders.Delete
{
    /// <summary>
    /// Обработчик команды удаления поставщика услуг.
    /// Напрямую удаляет запись из PostgreSQL без предварительной загрузки в память.
    /// </summary>
    public partial class DeleteUtilityProviderHandler(
            IApplicationDbContext context,
            ILogger<DeleteUtilityProviderHandler> logger) : IRequestHandler<DeleteUtilityProviderCommand>
    {
        public async Task Handle(DeleteUtilityProviderCommand command, CancellationToken cancellationToken)
        {
            LogUtilityProviderDeletionRequested(logger, command.Id);

            // EF Core сразу генерирует SQL-запрос: DELETE FROM UtilityProviders WHERE Id = @id
            int deletedRowsCount = await context.UtilityProviders
                .Where(p => p.Id == command.Id)
                .ExecuteDeleteAsync(cancellationToken);

            if (deletedRowsCount == 0)
            {
                LogUtilityProviderNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Не удалось удалить. Поставщик коммунальных услуг с ID {command.Id} не найден.");
            }

            LogUtilityProviderDeletedFromDb(logger, command.Id);
        }
    }
}
