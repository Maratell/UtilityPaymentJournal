using Microsoft.EntityFrameworkCore;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Account.GetCurrentUser
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения детальной информации об аутентифицированном пользователе.
    /// </summary>
    public static class GetCurrentUserMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность поставщика услуг в объект ответа API в памяти приложения.
        /// </summary>
        /// <param name="entity">Доменная сущеость <see cref="User"/>.</param>
        /// <returns>Заполненный ДТО ответа <see cref="GetUtilityProviderByIdResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если доменная сущность равны null.</exception>
        public static GetCurrentUserResponse ToResponse(this User entity, string? role)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new GetCurrentUserResponse(
                Id: entity.Id,
                UserName: entity.UserName,
                FirstName: entity.FirstName,
                LastName: entity.LastName,
                Role: role
            );
        }
    }
}
