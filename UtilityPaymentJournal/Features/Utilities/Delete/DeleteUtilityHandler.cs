using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.Delete
{
    /// <summary>
    /// Обработчик команды удаления коммунальной услуги.
    /// Реализует паттерн "Мягкое удаление" (Soft Delete): вместо физического удаления строки (DELETE) 
    /// из базы данных PostgreSQL, у записи сбрасывается флаг активности в false. Это позволяет 
    /// сохранить целостность исторических данных и связанных транзакций.
    /// </summary>
    public partial class DeleteUtilityHandler(
            ApplicationDbContext context,
            ILogger<DeleteUtilityHandler> logger) : IRequestHandler<DeleteUtilityCommand>
    {
        public async Task Handle(DeleteUtilityCommand command, CancellationToken cancellationToken)
        {
            LogUtilityDeletionRequested(logger, command.Id);

            // Загружаем сущность по уникальному первичному ключу (отслеживание изменений включено)
            Utility? entity = await context.Utilities
                .SingleOrDefaultAsync(u => u.Id == command.Id, cancellationToken);

            if (entity == null)
            {
                LogUtilityNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Не удалось удалить. Коммунальная услуга с ID {command.Id} не найдена.");
            }

            // Переводим в неактивный статус (Мягкое удаление / Soft Delete) и сохраняем изменения
            entity.IsActive = false;
            await context.SaveChangesAsync(cancellationToken);

            LogUtilityDeletedFromDb(logger, entity.Id);
        }
    }
}
