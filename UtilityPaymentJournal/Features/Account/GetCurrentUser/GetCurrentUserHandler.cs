using MediatR;
using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;
using UtilityPaymentJournal.Infrastructure.Identity;

namespace UtilityPaymentJournal.Features.Account.GetCurrentUser
{
    /// <summary>
    /// Обработчик запроса на получение деталей поставщика услуг.
    /// Инкапсулирует логику эффективного чтения из PostgreSQL.
    /// </summary>
    public partial class GetCurrentUserHandler(
            UserManager<User> userManager,
            ICurrentUserService currentUserService,
            ILogger<GetCurrentUserHandler> logger) : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponse>
    {
        public async Task<GetCurrentUserResponse> Handle(GetCurrentUserQuery query, CancellationToken cancellationToken)
        {
            // Извлекаем ID из CurrentUserService
            LogFetchingCurrentUserDetails(logger);
            string? userId = currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                LogUnauthorizedDetailsRequest(logger);
                throw new UnauthorizedAccessException("Операция получения данных профиля доступна только для аутентифицированных пользователей.");
            }

            // Ищем пользователя в базе данных
            LogFetchingUserFromDb(logger, userId);
            User? dbUser = await userManager.FindByIdAsync(userId);
            if (dbUser is null)
            {
                LogUserNotFoundInDb(logger, userId);
                throw new KeyNotFoundException($"Учетная запись пользователя с идентификатором {userId} не найдена в системе.");
            }

            // Получаем роли пользователя (берем первую/основную роль)
            IList<string> roles = await userManager.GetRolesAsync(dbUser);
            string? primaryRole = roles.FirstOrDefault();

            LogUserSuccessfullyFetchedFromDb(logger, userId);
            // Маппим данные через локальное расширение и возвращаем результат
            return dbUser.ToResponse(primaryRole);
        }
    }
}
