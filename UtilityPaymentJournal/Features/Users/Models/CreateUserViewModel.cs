using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Users.Models
{
    /// <summary>
    /// Модель представления для создания нового пользователя в системе.
    /// </summary>
    public class CreateUserViewModel
    {
        /// <summary>
        /// Уникальное имя пользователя (логин) для входа в систему (используется вместо Email).
        /// </summary>
        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        public string UserName { get; set; } = string.Empty;
        /// <summary>
        /// Имя пользователя для отображения в профиле.
        /// </summary>
        [Required(ErrorMessage = "Введите имя пользователя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; } = string.Empty;
        /// <summary>
        /// Фамилия пользователя для отображения в профиле.
        /// </summary>
        [Required(ErrorMessage = "Введите фамилию пользователя")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; } = string.Empty;
        /// <summary>
        /// Пароль учетной записи, соответствующий системным политикам безопасности.
        /// </summary>
        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// Системная роль, которая будет автоматически присвоена пользователю при создании.
        /// </summary>
        [Required(ErrorMessage = "Выберите роль пользователя")]
        [Display(Name = "Роль")]
        public UserRole Role { get; set; }
    }
}
