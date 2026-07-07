using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Common.Enumerations
{
    /// <summary>
    /// Тип пользовательской роли
    /// </summary>
    public enum UserRole
    {
        [Display(Name = "Пользователь")]
        User,
        [Display(Name = "Администратор")]
        Admin,
        [Display(Name = "Менеджер")]
        Manager
    }
}
