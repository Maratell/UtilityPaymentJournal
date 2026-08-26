using MediatR;

namespace UtilityPaymentJournal.Features.Users.GetById
{
    /// <summary>
    /// Запрос на получение полной информации одного пользователя в системе.
    /// </summary>
    /// <param name="Id">Уникальный строковый идентификатор пользователя (GUID).</param>
    public record GetUserByIdQuery(string Id) : IRequest<GetUserByIdResponse>;
}
