using MediatR;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.UtilityProviders.Create
{
    public partial class CreateUtilityProviderHandler(
        ApplicationDbContext context,
        ILogger<CreateUtilityProviderHandler> logger) : IRequestHandler<CreateUtilityProviderCommand, CreateUtilityProviderResponse>
    {
        public async Task<CreateUtilityProviderResponse> Handle(CreateUtilityProviderCommand command, CancellationToken cancellationToken)
        {
            LogUtilityProviderCreationRequested(logger, command.Name);
            UtilityProvider entity = command.ToEntity();

            // Используем синхронный Add, так как операция происходит в памяти
            context.UtilityProviders.Add(entity);
            await context.SaveChangesAsync(cancellationToken);

            LogUtilityProviderCreatedInDb(logger, entity.Id);
            return entity.ToResponse();
        }
    }
}
