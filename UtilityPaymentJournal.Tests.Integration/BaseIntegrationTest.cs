using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityPaymentJournal.Infrastructure.EF.Context;

namespace UtilityPaymentJournal.Tests.Integration
{
    public abstract class BaseIntegrationTest : IAsyncLifetime
    {
        protected readonly IntegrationTestWebAppFactory Factory;
        protected readonly HttpClient Client;
        protected readonly IServiceScope Scope;
        protected readonly ApplicationDbContext DbContext; // Для прямых проверок состояния БД в тестах

        protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
        {
            Factory = factory;
            Client = factory.HttpClient;
            Scope = factory.Services.CreateScope();
            DbContext = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        // Вызывается перед КАЖДЫМ тестовым методом
        public Task InitializeAsync() => Task.CompletedTask;

        // Вызывается после КАЖДОГО тестового метода — Respawn очищает таблицы за миллисекунды
        public async Task DisposeAsync()
        {
            Scope.Dispose();
            await Factory.ResetDatabaseAsync();
        }
    }
}
