using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Models.Authentication
{
    /// <summary>
    /// Модель входящего запроса для авторизации пользователя по логину и паролю.
    /// </summary>
    public class SignInRequestViewModel
    {
        /// <summary>
        /// Имя пользователя (логин) в системе.
        /// </summary>
        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Логин")]
        public string UserName { get; set; } // Вместо Email

        /// <summary>
        /// Секретный пароль учетной записи.
        /// </summary>
        [Required(ErrorMessage = "Введите пароль")]
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
