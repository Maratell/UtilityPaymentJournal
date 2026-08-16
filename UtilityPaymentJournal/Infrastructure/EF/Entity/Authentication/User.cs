using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.Common.Interfaces;

namespace UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication
{
    public class User : IdentityUser, IAuditable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //// Тип ключа должен быть string (совпадает со string в DbContext), а не int?
        //public string RoleId { get; set; }
        //[ForeignKey("RoleId")]
        //public Role Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
