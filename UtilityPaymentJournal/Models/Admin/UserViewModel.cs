using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Models.Admin
{
    public class UserViewModel
    {
        [Display(Name = "Идентификатор")]
        public string Id { get; set; }

        [Display(Name = "Логин")]
        public string UserName { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Роль пользователя")]
        public string RoleName { get; set; }
    }
}
