using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Infrastructure.EF.Configurations
{
    /// <summary>
    /// Настройка таблицы услуг. 
    /// Этот класс автоматически добавляет базовые услуги в базу данных при накате миграции.
    /// </summary>
    public class UtilityConfiguration : IEntityTypeConfiguration<Utility>
    {
        public void Configure(EntityTypeBuilder<Utility> builder)
        {
            // Указываем PostgreSQL автоматически заполнять дату создания при генерации миграции
            builder.Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasData(
                new Utility
                { 
                    Id = 1L,
                    Name = "Водоснабжение",
                    IconClass = "bi bi-droplet-fill text-primary", 
                    IsActive = true
                },

                new Utility 
                { 
                    Id = 2L, 
                    Name = "Электроэнергия",
                    IconClass = "bi bi-lightning-charge-fill text-warning", 
                    IsActive = true
                },

                new Utility
                {
                    Id = 3L,
                    Name = "Отопление",
                    IconClass = "bi bi-sun-fill text-danger",
                    IsActive = true
                }
            );
        }
    }
}
