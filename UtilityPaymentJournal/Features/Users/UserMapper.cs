using UtilityPaymentJournal.Features.Users.Commands;
using UtilityPaymentJournal.Features.Users.Models;
using UtilityPaymentJournal.Features.Users.Queries;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Features.Users
{
    public class UserMapper : IUserMapper
    {
        /// <summary>
        /// Преобразовать входящую модель создания во входной ДТО бизнес-логики.
        /// </summary>
        public CreateUserDto ToDto(CreateUserViewModel createViewModel)
        {
            ArgumentNullException.ThrowIfNull(createViewModel);

            return new CreateUserDto(
                UserName: createViewModel.UserName,
                FirstName: createViewModel.FirstName,
                LastName: createViewModel.LastName,
                Password: createViewModel.Password,
                Role: createViewModel.Role
            );
        }

        /// <summary>
        /// Преобразовать входной ДТО создания в доменную сущность для базы данных.
        /// </summary>
        public User ToEntity(CreateUserDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            return new User
            {
                UserName = createDto.UserName,
                FirstName = createDto.FirstName,
                LastName = createDto.LastName
                // Пароль здесь не маппится, так как UserManager принимает его отдельным аргументом для хэширования
            };
        }

        /// <summary>
        /// Преобразовать сущность после сохранения в плоский ДТО результата команды записи.
        /// </summary>
        public UserCommandResultDto ToCommandResultDto(User entity, string roleName)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentException.ThrowIfNullOrWhiteSpace(roleName);

            return new UserCommandResultDto(
                Id: entity.Id,
                UserName: entity.UserName ?? string.Empty,
                FirstName: entity.FirstName,
                LastName: entity.LastName,
                RoleName: roleName
            );
        }

        /// <summary>
        /// Преобразовать сущность в ДТО результата запроса чтения.
        /// </summary>
        public UserQueryResultDto ToQueryResultDto(User entity, string roleName)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentException.ThrowIfNullOrWhiteSpace(roleName);

            return new UserQueryResultDto(
                Id: entity.Id,
                UserName: entity.UserName ?? string.Empty,
                FirstName: entity.FirstName,
                LastName: entity.LastName,
                RoleName: roleName
            );
        }

        /// <summary>
        /// Преобразовать плоский ДТО записи в модель ответа API создания (для POST).
        /// </summary>
        public UserCreatedViewModel ToCreatedViewModel(UserCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new UserCreatedViewModel
            {
                Id = dto.Id,
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                RoleName = dto.RoleName
            };
        }

        /// <summary>
        /// Преобразовать ДТО чтения в детальную модель представления для UI (для GET).
        /// </summary>
        public UserDetailsViewModel ToViewModel(UserQueryResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new UserDetailsViewModel
            {
                Id = dto.Id,
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                RoleName = dto.RoleName
            };
        }
    }
}
