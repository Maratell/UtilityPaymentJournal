using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using UtilityPaymentJournal.Common.Constants;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Infrastructure.Identity
{

    /// <summary>
    /// Фабрика для формирования основного удостоверения пользователя (ClaimsPrincipal).
    /// Расширяет стандартный набор утверждений (Claims) Identity, автоматически добавляя 
    /// в авторизационную куку персональные данные профиля (Имя и Фамилию).
    /// Это позволяет избегать лишних запросов к базе данных при каждом обращении к текущему пользователю.
    /// </summary>
    public class UserProfileClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
    {
        public UserProfileClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        /// <summary>
        /// Генерирует набор утверждений (Claims) для указанного пользователя.
        /// </summary>
        /// <param name="user">Объект авторизуемого пользователя из базы данных.</param>
        /// <returns>Объект ClaimsIdentity, содержащий системные и пользовательские утверждения.</returns>
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Невозможно сгенерировать Claims для неопределенного пользователя.");
            }

            // Получаем базовый набор клеймов (ID пользователя, Username, Роли)
            var identity = await base.GenerateClaimsAsync(user);

            // Добавляем Имя в контекст авторизации, если оно заполнено
            if (!string.IsNullOrEmpty(user.FirstName))
            {
                identity.AddClaim(new Claim(ClaimConstants.FirstName, user.FirstName.Trim()));
            }

            // Добавляем Фамилию в контекст авторизации, если она заполнена
            if (!string.IsNullOrEmpty(user.LastName))
            {
                identity.AddClaim(new Claim(ClaimConstants.LastName, user.LastName.Trim()));
            }

            return identity;
        }
    }
}
