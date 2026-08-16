using Microsoft.AspNetCore.Identity;
using UtilityPaymentJournal.Common.Interfaces;

namespace UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication
{
    public class Role : IdentityRole<string>, IAuditable
    {
        public Role() 
        { }

        public Role(string roleName) : base(roleName)
        {
            this.Id = Guid.NewGuid().ToString(); // Явно генерируем строковый ID
        }

        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
