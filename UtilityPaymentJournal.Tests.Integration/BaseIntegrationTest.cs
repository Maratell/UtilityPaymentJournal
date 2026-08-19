using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityPaymentJournal.Infrastructure.EF.Context;

namespace UtilityPaymentJournal.Tests.Integration
{
    /// <summary>
    /// Базовый класс для всех интеграционных тестов.
    /// Гарантирует изоляцию: каждый тест запускается в чистом окружении и со своей базой данных.
    /// Интерфейс IAsyncLifetime управляет асинхронной инициализацией и очисткой ресурсов.
    /// </summary>
    public abstract class BaseIntegrationTest(IntegrationTestWebAppFactory factory) : IAsyncLifetime
    {
        /// <summary>
        /// Общая фабрика (одна на весь запуск тестов), управляющая Docker-контейнером и сервером в памяти
        /// </summary>
        protected IntegrationTestWebAppFactory Factory { get; } = factory;
        /// <summary>
        /// HTTP-клиент для отправки запросов в тестируемый контроллер
        /// </summary>
        protected HttpClient Client { get; } = factory.HttpClient;
        /// <summary>
        /// Область видимости (Scope) для создания Scope-зависимостей внутри конкретного теста
        /// </summary>
        protected IServiceScope Scope { get; private set; } = null!;
        /// <summary>
        /// Контекст БД для предварительного заполнения данных (Arrange) или прямых проверок в базе (Assert)
        /// </summary>
        protected ApplicationDbContext DbContext { get; private set; } = null!;

        /// <summary>
        /// Вызывается xUnit автоматически ПЕРЕД каждым тестовым методом.
        /// </summary>
        public async Task InitializeAsync()
        {
            // 1. Сбрасываем состояние базы данных (удаляем данные из таблиц с помощью Respawn)
            // Делаем это ДО теста, чтобы гарантировать абсолютно чистую БД на старте.
            await Factory.ResetDatabaseAsync();

            // 2. Создаем изолированный Scope для текущего теста
            Scope = Factory.Services.CreateScope();

            // 3. Получаем чистый экземпляр DbContext, не привязанный к предыдущим тестам
            DbContext = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        /// <summary>
        /// Вызывается xUnit автоматически ПОСЛЕ каждого тестового метода.
        /// </summary>
        public Task DisposeAsync()
        {
            // Освобождаем Scope текущего теста, что автоматически закроет соединение DbContext с базой данных
            Scope.Dispose();

            return Task.CompletedTask;
        }
    }

    //public abstract class BaseIntegrationTest : IAsyncLifetime
    //{
    //    protected readonly IntegrationTestWebAppFactory Factory;
    //    protected readonly HttpClient Client;
    //    protected readonly IServiceScope Scope;
    //    protected readonly ApplicationDbContext DbContext; // Для прямых проверок состояния БД в тестах

    //    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    //    {
    //        Factory = factory;
    //        Client = factory.HttpClient;
    //        Scope = factory.Services.CreateScope();
    //        DbContext = Scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //    }

    //    // Вызывается перед КАЖДЫМ тестовым методом
    //    public Task InitializeAsync() => Task.CompletedTask;

    //    // Вызывается после КАЖДОГО тестового метода — Respawn очищает таблицы за миллисекунды
    //    public async Task DisposeAsync()
    //    {
    //        Scope.Dispose();
    //        await Factory.ResetDatabaseAsync();
    //    }
    //}
}
