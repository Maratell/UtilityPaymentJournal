using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UtilityPaymentJournal.Features.Residences.Create;
using UtilityPaymentJournal.Features.Residences.GetById;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Tests.Integration
{
    [Collection("Integration Tests Collection")]
    public class ResidencesControllerTests : BaseIntegrationTest
    {
        public ResidencesControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Create_Should_SaveResidenceInDatabase_And_ReturnCreatedStatus()
        {
            // Arrange
            // 1. Указываем ID, который наш TestAuthHandler подкладывает в HttpContext.User
            string testUserId = "test-admin-id-123";

            // 2. Генерируем уникальный адрес для исключения конфликтов уникальности при повторных запусках
            string uniqueAddress = $"Тестовый адрес {Guid.NewGuid().ToString("N")[..6]}";

            // 3. Создаем легитимного пользователя со всеми обязательными полями (FirstName, LastName) 
            // в Docker-базе перед тестом. Это гарантирует, что связь Foreign Key в PostgreSQL не нарушится!
            var testUser = new User
            {
                Id = testUserId,
                UserName = "testadmin",
                FirstName = "Тест",
                LastName = "Админ"
            };

            // Добавляем .IgnoreQueryFilters(), чтобы EF Core проверил реальное наличие 
            // пользователя в таблице Postgres в обход любых скрытых фильтров!
            if (!DbContext.Users.IgnoreQueryFilters().Any(u => u.Id == testUserId))
            {
                await DbContext.Users.AddAsync(testUser);
                await DbContext.SaveChangesAsync();
                DbContext.ChangeTracker.Clear();
            }

            // 4. Формируем тело запроса для CreateResidenceRequest. 
            // Передаем ТОЛЬКО адрес, как это делает фронтенд на реальном сайте!
            var request = new CreateResidenceRequest(uniqueAddress);

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json"
            );

            // Act — Отправляем запрос на создание через HttpClient базового класса
            var response = await Client.PostAsync("api/residences", content);

            // Assert
            // Если сервер неожиданно вернул 400 Bad Request, перехватываем и выводим ошибку валидации
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Xunit.Sdk.XunitException($"[400 BadRequest] Ошибка валидации: {errorText}");
            }

            // Проверяем каноничный статус 201 Created, который честно возвращает ваш метод CreatedAtAction
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Десериализуем JSON-ответ сервера, чтобы узнать сгенерированный базой данных ID
            var responseContent = await response.Content.ReadFromJsonAsync<CreateResidenceResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().BeGreaterThan(0);

            // Сбрасываем кэш отслеживания сущностей EF Core, чтобы сделать чистый запрос к СУБД
            DbContext.ChangeTracker.Clear();

            // 🚀 ГЛАВНАЯ ПРОВЕРКА В БАЗЕ ДАННЫХ ПОСТГРЕС
            // Мы принудительно используем .IgnoreQueryFilters(), так как контекст внутри теста 
            // не имеет текущего пользователя (UserId = null), и глобальный фильтр иначе скрыл бы запись!
            var residenceInDb = DbContext.Residences
                .IgnoreQueryFilters()
                .FirstOrDefault(r => r.Id == responseContent.Id);

            // Проверяем, что запись физически создана хендлером в Docker-контейнере
            residenceInDb.Should().NotBeNull();
            residenceInDb!.Address.Should().Be(uniqueAddress);

            // ПРОВЕРКА ТРАССИРОВКИ: доказываем, что ваш метод ChangeTracker.ApplyUserOwnership 
            // автоматически привязал к записи правильный ID текущего пользователя из TestAuthHandler!
            residenceInDb.UserId.Should().Be(testUserId);
        }

        [Fact]
        public async Task GetById_Should_ReturnResidence_When_ItExistsInDatabase()
        {
            // Arrange — Напрямую создаем сущность в пустой БД перед тестом
            // Замените 'Residence' на ваше реальное имя сущности (Entity), если оно отличается
            var residence = new Residence
            {
                Address = "Прибрежная, д. 5"
            };
            await DbContext.Residences.AddAsync(residence);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // Act — Делаем запрос на получение по ID
            var response = await Client.GetAsync($"api/residences/{residence.Id}");

            // Assert — Проверяем, что API вернуло объект из базы
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadFromJsonAsync<GetResidenceByIdResponse>();
            content.Should().NotBeNull();
            content!.Id.Should().Be(residence.Id);
            content.Address.Should().Be("Прибрежная, д. 5");
        }

        [Fact]
        public async Task GetById_Should_ReturnNotFound_When_ResidenceDoesNotExist()
        {
            // Act — Пытаемся получить несуществующий объект
            var response = await Client.GetAsync("api/residences/99999");

            // Assert — Проверяем обработку отсутствующих данных
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
