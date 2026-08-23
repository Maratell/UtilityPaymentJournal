using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.Edit
{
    public partial class EditUtilityHandler(
            ApplicationDbContext context,
            ILogger<EditUtilityHandler> logger) : IRequestHandler<EditUtilityCommand, EditUtilityResponse>
    {
        public async Task<EditUtilityResponse> Handle(EditUtilityCommand command, CancellationToken cancellationToken)
        {
            LogUtilityUpdateRequested(logger, command.Id, command.Name);

            // Загружаем "легковесное" entity без связанных деталей по уникальному первичному ключу
            Utility? entity = await context.Utilities
                .SingleOrDefaultAsync(u => u.Id == command.Id, cancellationToken);

            if (entity == null)
            {
                LogUtilityNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Коммунальной услуги с ID {command.Id} не найдено.");
            }

            command.UpdateEntity(entity);
            await context.SaveChangesAsync(cancellationToken);

            LogUtilityUpdatedInDb(logger, entity.Id);
            return entity.ToResponse();
        }
    }
}
