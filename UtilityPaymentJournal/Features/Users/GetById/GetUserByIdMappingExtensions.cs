using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Features.Users.GetById
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения расширенной информации пользователя в системе.
    /// </summary>
    public static class GetUserByIdMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность пользователя в объект ответа API в памяти приложения.
        /// </summary>
        /// <param name="entity">Доменная сущеость <see cref="User"/>.</param>
        /// <returns>Заполненный ДТО ответа <see cref="GetUserByIdResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если доменная сущность равны null.</exception>
        public static GetUserByIdResponse ToResponse(this User entity, string? role)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new GetUserByIdResponse(
                Id: entity.Id,
                UserName: entity.UserName,
                FirstName: entity.FirstName,
                LastName: entity.LastName,
                Role: role
            );
        }
    }
}
