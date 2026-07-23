using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.EF.Entity;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.EF.Entity.ComplaintBoard;
using UtilityPaymentJournal.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.EF.Entity.WaterReadings;
using UtilityPaymentJournal.Infrastructure.Identity;

namespace UtilityPaymentJournal.EF.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        private readonly ICurrentUserService _currentUserService;

        public DbSet<Residence> Residences { get; set; }
        public DbSet<Utility> Utilities { get; set; }
        public DbSet<UtilityProvider> UtilityProviders { get; set; }
        public DbSet<WaterReading> WaterReadings { get; set; }
        public DbSet<ElectricityReading> ElectricityReadings { get; set; }
        public DbSet<Complaint> Complaints { get; set; }

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService)
            : base(options)
        {
            _currentUserService = currentUserService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Настраиваем фильтр запросов по Id пользователя
            builder.ApplyUserGlobalFilters(() => _currentUserService.UserId);

            // Задаем схему по умолчанию (опционально, если нужно хранить все в 'public')
            builder.HasDefaultSchema("public");

            // Создаем уникальный индекс для пары колонок провайдера и услуги
            builder.Entity<UtilityProviderLink>()
                .HasIndex(l => new { l.UtilityProviderId, l.UtilityId })
                .IsUnique();

            // Переводим элементы базыы данных в нижний регистр (для упрощения запросов к Postgre SQL)
            builder.UseLowerCaseNamingConvention();
        }

        /// <summary>
        /// Синхронный метод сохранения
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            ApplyInterceptors();
            return base.SaveChanges();
        }

        /// <summary>
        /// Асинхронный метод сохранения (используется чаще всего)
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyInterceptors();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyInterceptors()
        {
            ChangeTracker.ApplyUserOwnership(_currentUserService.UserId);
            ChangeTracker.ApplyAuditDates();
        }
    }
}
