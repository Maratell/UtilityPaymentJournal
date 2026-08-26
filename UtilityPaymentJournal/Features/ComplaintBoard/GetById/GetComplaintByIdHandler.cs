using MediatR;
using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;

namespace UtilityPaymentJournal.Features.ComplaintBoard.GetById
{
    /// <summary>
    /// Обработчик запроса на получение деталей карточки жалобы.
    /// Инкапсулирует логику эффективного чтения из PostgreSQL.
    /// </summary>
    public partial class GetComplaintByIdHandler(
            ApplicationDbContext context,
            ILogger<GetComplaintByIdHandler> logger) : IRequestHandler<GetComplaintByIdQuery, GetComplaintByIdResponse>
    {
        public async Task<GetComplaintByIdResponse> Handle(GetComplaintByIdQuery query, CancellationToken cancellationToken)
        {
            LogFetchingComplaintByIdFromDb(logger, query.Id);

            Complaint? entity = await context.Complaints
                .Include(w => w.Utility)
                .AsNoTracking()
                .SingleOrDefaultAsync(r => r.Id == query.Id, cancellationToken);

            if (entity is null)
            {
                LogComplaintNotFoundInDb(logger, query.Id);
                throw new KeyNotFoundException($"Жалоба с ID {query.Id} не найдена.");
            }

            return entity.ToResponse();
        }
    }
}
