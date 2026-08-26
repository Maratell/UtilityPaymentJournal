using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Features.Users.Create
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи создания пользователя в системе.
    /// Инкапсулирует преобразования между Command, Entity и Response внутри слайса.
    /// </summary>
    public static class CreateUserMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность пользователя в объект ответа API.
        /// </summary>
        /// <param name="entity">Доменная сущность <see cref="User"/>.</param>
        /// <returns>Заполненный DTO ответа <see cref="CreateUserResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если сущность равна null.</exception>
        public static CreateUserResponse ToResponse(this User entity, string? role)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new CreateUserResponse(
                Id: entity.Id,
                UserName: entity.UserName,
                FirstName: entity.FirstName,
                LastName: entity.LastName,
                Role: role
            );
        }

        /// <summary>
        /// Создает новую доменную сущность пользователя на основе команды запроса.
        /// </summary>
        /// <param name="createCommand">Команда с входными данными для создания записи.</param>
        /// <returns>Новый экземпляр <see cref="User"/> Готовая доменная сущность.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если команда равна null.</exception>
        public static User ToEntity(this CreateUserCommand createCommand)
        {
            ArgumentNullException.ThrowIfNull(createCommand);

            return new User
            {
                UserName = createCommand.UserName,
                FirstName = createCommand.FirstName,
                LastName = createCommand.LastName
                // Пароль здесь не маппится, так как UserManager принимает его отдельным аргументом для хэширования
            };
        }
    }
}
