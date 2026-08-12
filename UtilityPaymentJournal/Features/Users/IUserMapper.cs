using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Features.Users.Commands;
using UtilityPaymentJournal.Features.Users.Models;
using UtilityPaymentJournal.Features.Users.Queries;

namespace UtilityPaymentJournal.Features.Users
{
    /// <summary>
    /// Интерфейс маппера для преобразования моделей данных пользователя между слоями.
    /// </summary>
    public interface IUserMapper
    {
        /// <summary>
        /// Преобразовать входящую модель создания во входной ДТО бизнес-логики.
        /// </summary>
        CreateUserDto ToDto(CreateUserViewModel createViewModel);
        /// <summary>
        /// Преобразовать входной ДТО создания в доменную сущность для базы данных.
        /// </summary>
        User ToEntity(CreateUserDto createDto);
        /// <summary>
        /// Преобразовать сущность после сохранения в плоский ДТО результата команды записи.
        /// </summary>
        UserCommandResultDto ToCommandResultDto(User entity, string roleName);
        /// <summary>
        /// Преобразовать сущность в ДТО результата запроса чтения.
        /// </summary>
        UserQueryResultDto ToQueryResultDto(User entity, string roleName);
        /// <summary>
        /// Преобразовать плоский ДТО записи в модель ответа API создания (для POST).
        /// </summary>
        UserCreatedViewModel ToCreatedViewModel(UserCommandResultDto dto);
        /// <summary>
        /// Преобразовать ДТО чтения в детальную модель представления для UI (для GET).
        /// </summary>
        UserDetailsViewModel ToViewModel(UserQueryResultDto dto);
    }
}
