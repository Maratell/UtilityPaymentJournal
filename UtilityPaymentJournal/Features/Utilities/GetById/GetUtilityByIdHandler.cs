using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Common.Interfaces;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.GetById
{
    /// <summary>
    /// Обработчик запроса на получение деталей услуги.
    /// Инкапсулирует логику эффективного чтения из PostgreSQL.
    /// </summary>
    public partial class GetUtilityByIdHandler(
            IApplicationDbContext context,
            ILogger<GetUtilityByIdHandler> logger) : IRequestHandler<GetUtilityByIdQuery, GetUtilityByIdResponse>
    {
        public async Task<GetUtilityByIdResponse> Handle(GetUtilityByIdQuery query, CancellationToken cancellationToken)
        {
            LogFetchingUtilityByIdFromDb(logger, query.Id);

            Utility? entity = await context.Utilities
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Id == query.Id, cancellationToken);

            if (entity is null)
            {
                LogUtilityNotFoundInDb(logger, query.Id);
                throw new KeyNotFoundException($"Коммунальной услуги с ID {query.Id} не найдено.");
            }

            return entity.ToResponse();
        }
    }
}
