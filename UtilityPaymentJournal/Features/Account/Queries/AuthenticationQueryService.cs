using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;
using UtilityPaymentJournal.Infrastructure.Identity;

namespace UtilityPaymentJournal.Features.Account.Queries
{
    /// <summary>
    /// Сервис запросов (чтения) данных аутентификации и текущей сессии.
    /// Реализует логику эффективного извлечения данных без изменения состояния системы.
    /// </summary>
    public partial class AuthenticationQueryService(
            UserManager<User> userManager,
            ICurrentUserService currentUserService,
            ILogger<AuthenticationQueryService> logger) : IAuthenticationQueryService
    {
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        private readonly ILogger<AuthenticationQueryService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Проверяет статус аутентификации пользователя в текущем HTTP-контексте.
        /// </summary>
        /// <returns>Значение true, если текущий пользователь успешно вошел в систему; иначе — false</returns>
        public bool IsAuthenticated()
        {
            LogCheckingAuthenticationStatus(_logger);
            bool isAuthenticated = !string.IsNullOrEmpty(_currentUserService.UserId);

            LogAuthenticationStatusChecked(_logger, isAuthenticated);
            return isAuthenticated;
        }

        /// <summary>
        /// Извлекает уникальный идентификатор (ID) текущего пользователя из данных сессии.
        /// </summary>
        /// <returns>Строковый идентификатор пользователя или null, если запрос отправлен анонимным гостем</returns>
        public string? GetCurrentUserId()
        {
            LogFetchingCurrentUserId(_logger);
            string? userId = _currentUserService.UserId;

            LogCurrentUserIdFetched(_logger, userId);
            return userId;
        }

        /// <summary>
        /// Загружает из базы данных развернутую информацию о текущем вошедшем пользователе.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>ДТО с детальными данными пользователя, оптимизированное для вывода на UI</returns>
        /// <exception cref="UnauthorizedAccessException">Выбрасывается, если метод вызван неавторизованным пользователем</exception>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если учетная запись с полученным ID отсутствует в базе данных</exception>
        public async Task<CurrentUserQueryResultDto> GetCurrentUserDetailsAsync(CancellationToken cancellationToken = default)
        {
            LogFetchingCurrentUserDetails(_logger);
            string? userId = GetCurrentUserId();
            if (string.IsNullOrEmpty(userId))
            {
                LogUnauthorizedDetailsRequest(_logger);
                throw new UnauthorizedAccessException("Операция получения данных профиля доступна только для аутентифицированных пользователей.");
            }

            LogFetchingUserFromDb(_logger, userId);
            User? dbUser = await _userManager.FindByIdAsync(userId);
            if (dbUser is null)
            {
                LogUserNotFoundInDb(_logger, userId);
                throw new KeyNotFoundException($"Учетная запись пользователя с идентификатором {userId} не найдена в системе.");
            }

            LogUserSuccessfullyFetchedFromDb(_logger, userId);
            return new CurrentUserQueryResultDto(
                Id: dbUser.Id,
                UserName: dbUser.UserName ?? string.Empty
            );
        }
    }
}
