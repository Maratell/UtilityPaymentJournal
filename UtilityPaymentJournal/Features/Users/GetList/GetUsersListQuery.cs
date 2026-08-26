using MediatR;

namespace UtilityPaymentJournal.Features.Users.GetList
{
    /// <summary>
    /// Запрос на получение списка пользователей в системе.
    /// </summary>
    public record GetUsersListQuery : IRequest<GetUsersListResponse>;
}
