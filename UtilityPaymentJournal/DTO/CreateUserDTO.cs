using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.DTO
{
    public class CreateUserDTO
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
    }
}
