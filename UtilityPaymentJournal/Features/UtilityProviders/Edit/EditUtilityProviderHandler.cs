using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.UtilityProviders.Edit
{
    public partial class EditUtilityProviderHandler(
            ApplicationDbContext context,
            ILogger<EditUtilityProviderHandler> logger) : IRequestHandler<EditUtilityProviderCommand, EditUtilityProviderResponse>
    {
        public async Task<EditUtilityProviderResponse> Handle(EditUtilityProviderCommand command, CancellationToken cancellationToken)
        {
            LogUtilityProviderUpdateRequested(logger, command.Id, command.Name);

            UtilityProvider? entity = await context.UtilityProviders
                .SingleOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            if (entity == null)
            {
                LogUtilityProviderNotFoundInDb(logger, command.Id);
                throw new KeyNotFoundException($"Поставщик коммунальных услуг с ID {command.Id} не найден в базе данных.");
            }

            command.UpdateEntity(entity);
            await context.SaveChangesAsync(cancellationToken);

            LogUtilityProviderUpdatedInDb(logger, command.Id);
            return entity.ToResponse();
        }
    }
}
