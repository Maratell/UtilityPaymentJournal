using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.UtilityProviders.GetById
{
    /// <summary>
    /// Обработчик запроса на получение деталей поставщика услуг.
    /// Инкапсулирует логику эффективного чтения из PostgreSQL.
    /// </summary>
    public partial class GetUtilityProviderByIdHandler(
            ApplicationDbContext context,
            ILogger<GetUtilityProviderByIdHandler> logger) : IRequestHandler<GetUtilityProviderByIdQuery, GetUtilityProviderByIdResponse>
    {
        public async Task<GetUtilityProviderByIdResponse> Handle(GetUtilityProviderByIdQuery query, CancellationToken cancellationToken)
        {
            LogFetchingUtilityProviderByIdFromDb(logger, query.Id);

            UtilityProvider? entity = await context.UtilityProviders
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == query.Id, cancellationToken);

            if (entity is null)
            {
                LogUtilityProviderNotFoundInDb(logger, query.Id);
                throw new KeyNotFoundException($"Поставщик коммунальных услуг с ID {query.Id} не найден.");
            }

            return entity.ToResponse();
        }
    }
}
