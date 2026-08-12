using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Features.Users.Models
{
    /// <summary>
    /// Развернутая модель представления пользователя для отображения на UI (ответ на GET).
    /// Содержит полную текстовую и идентификационную информацию о пользователе системы.
    /// </summary>
    public class UserDetailsViewModel
    {
        /// <summary>
        /// Уникальный идентификатор записи пользователя в БД (GUID)
        /// </summary>
        [Display(Name = "Идентификатор")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Имя пользователя (логин) для входа в систему
        /// </summary>
        [Display(Name = "Логин")]
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Имя пользователя, извлеченное из базы данных
        /// </summary>
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Фамилия пользователя, извлеченная из базы данных
        /// </summary>
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// Актуальное текстовое название назначенной системной роли пользователя
        /// </summary>
        [Display(Name = "Роль пользователя")]
        public string RoleName { get; set; } = string.Empty;
    }
}
