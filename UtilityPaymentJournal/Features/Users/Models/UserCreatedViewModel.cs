using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Features.Users.Models
{
    /// <summary>
    /// Модель представления результата выполнения операции создания пользователя (ответ на POST).
    /// Строго не содержит навигационных свойств, отражая полный плоский результат успешной генерации записи.
    /// </summary>
    public class UserCreatedViewModel
    {
        /// <summary>
        /// Уникальный идентификатор созданной записи пользователя в БД (GUID).
        /// </summary>
        [Display(Name = "Идентификатор")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// Уникальное имя пользователя (логин), под которым он был зарегистрирован.
        /// </summary>
        [Display(Name = "Логин")]
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Имя созданного пользователя.
        /// </summary>
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Фамилия созданного пользователя.
        /// </summary>
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// Название системной роли, которая была успешно присвоена пользователю при создании.
        /// </summary>
        [Display(Name = "Роль пользователя")]
        public string RoleName { get; set; } = string.Empty;
    }
}
