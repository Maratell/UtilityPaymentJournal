using MediatR;

namespace UtilityPaymentJournal.Features.Account.GetCurrentUser
{
    /// <summary>
    /// Запрос на получение детальной информации об аутентифицированном пользователе.
    /// </summary>
    public record GetCurrentUserQuery : IRequest<GetCurrentUserResponse>;
}
