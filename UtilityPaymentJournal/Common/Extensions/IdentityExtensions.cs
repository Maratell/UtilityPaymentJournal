using System.Security.Claims;
using System.Security.Principal;
using UtilityPaymentJournal.Common.Constants;

namespace UtilityPaymentJournal.Common.Extensions
{
    /// <summary>
    /// Предоставляет методы расширения для интерфейса <see cref="IPrincipal"/>.
    /// Позволяет удобно и безопасно извлекать персональные данные пользователя (Имя, Фамилию, ...)
    /// из контекста авторизации (Claims) в контроллерах, сервисах и Razor-представлениях.
    /// </summary>
    public static class IdentityExtensions
    {
        /// <summary>
        /// Безопасно извлекает имя авторизованного пользователя из его утверждений (Claims).
        /// </summary>
        /// <param name="principal">Текущий контекст пользователя (например, объект User в контроллере).</param>
        /// <returns>Строку с именем пользователя, либо пустую строку <see cref="string.Empty"/>, если имя не найдено или пользователь не авторизован.</returns>
        public static string GetFirstName(this IPrincipal principal)
        {
            if (principal is ClaimsPrincipal claimsPrincipal)
            {
                return claimsPrincipal.FindFirst(ClaimConstants.FirstName)?.Value ?? string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// Безопасно извлекает фамилию авторизованного пользователя из его утверждений (Claims).
        /// </summary>
        /// <param name="principal">Текущий контекст пользователя (например, объект User в контроллере).</param>
        /// <returns>Строку с фамилией пользователя, либо пустую строку <see cref="string.Empty"/>, если фамилия не найдена или пользователь не авторизован.</returns>
        public static string GetLastName(this IPrincipal principal)
        {
            if (principal is ClaimsPrincipal claimsPrincipal)
            {
                return claimsPrincipal.FindFirst(ClaimConstants.LastName)?.Value ?? string.Empty;
            }
            return string.Empty;
        }

        /// <summary>
        /// Получить полное имя пользователя "Имя Фамилия".
        /// </summary>
        /// <param name="principal">Текущий контекст пользователя (например, объект User в контроллере).</param>
        /// <returns>Строку формата "Имя Фамилия".</returns>
        public static string GetFullName(this IPrincipal principal)
        {
            string firstName = principal.GetFirstName();
            string lastName = principal.GetLastName();

            return $"{firstName} {lastName}".Trim();
        }
    }
}
