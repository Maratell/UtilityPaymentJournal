using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using UtilityPaymentJournal.Features.Utilities.Create;
using UtilityPaymentJournal.Features.Utilities.Edit;
using UtilityPaymentJournal.Features.Utilities.GetById;
using UtilityPaymentJournal.Features.Utilities.GetList;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;
using UtilityPaymentJournal.Tests.Integration.Infrastructure;

namespace UtilityPaymentJournal.Tests.Integration.Controllers
{
    [Collection(nameof(IntegrationTestCollection))]
    public class UtilityApiControllerTests : BaseIntegrationTest
    {
        public UtilityApiControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Проверяет, что при отправке валидных данных контроллер успешно создает запись коммунальной услуги, 
        /// сохраняет её в PostgreSQL и возвращает статус 201 Created.
        /// </summary>
        [Fact]
        public async Task Create_Should_SaveUtilityInDatabase_And_ReturnCreatedStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных и окружения)
            // ==========================================

            // 1. Генерируем уникальное наименование для предотвращения конфликтов уникальности (Unique Constraints) в БД
            string uniqueUtilityName = $"Тестовая услуга {Guid.NewGuid().ToString("N")[..6]}";
            const string testIconClass = "fa-solid fa-bolt";

            // 2. Формируем тело запроса (DTO) согласно структуре CreateUtilityRequest
            CreateUtilityRequest request = new CreateUtilityRequest(uniqueUtilityName, testIconClass, true);

            // ==========================================
            // ACT (Выполнение целевого действия)
            // ==========================================

            // Отправляем запрос на создание коммунальной услуги. Метод расширения сам сериализует DTO в JSON.
            HttpResponseMessage response = await Client.PostAsJsonAsync("api/utilities", request);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Перехватываем 400 BadRequest и выводим детальную ошибку валидации для удобной отладки
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string errorText = await response.Content.ReadAsStringAsync();
                throw new Xunit.Sdk.XunitException($"[400 BadRequest] Ошибка валидации от API: {errorText}");
            }

            // Проверяем каноничный HTTP-статус ответа 201 Created
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Быстро десериализуем JSON-ответ прямо через ReadFromJsonAsync
            CreateUtilityResponse? responseContent = await response.Content.ReadFromJsonAsync<CreateUtilityResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().BeGreaterThan(0);

            // Сбрасываем кэш отслеживания (Change Tracker) EF Core перед прямым запросом в СУБД,
            // чтобы получить актуальные данные из PostgreSQL, а не локального кэша
            DbContext.ChangeTracker.Clear();

            // ГЛАВНАЯ ПРОВЕРКА В БАЗЕ ДАННЫХ POSTGRESQL
            // Так как сущность общая (нет привязки к UserId), запрашиваем запись напрямую по Id.
            Utility? utilityInDb = await DbContext.Utilities
                .FirstOrDefaultAsync(r => r.Id == responseContent.Id);

            // Проверяем, что запись физически создана в Docker-контейнере и ее поля заполнены корректно
            utilityInDb.Should().NotBeNull();
            utilityInDb!.Name.Should().Be(uniqueUtilityName);
            utilityInDb!.IconClass.Should().Be(testIconClass);
            utilityInDb!.IsActive.Should().BeTrue();
        }

        /// <summary>
        /// Проверяет, что существующая в базе данных запись коммунальной услуги 
        /// успешно возвращается по её идентификатору (ID) со статусом 200 OK.
        /// </summary>
        [Fact]
        public async Task GetById_Should_ReturnUtility_When_ItExistsInDatabase()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================

            // 1. Генерируем уникальное наименование для исключения конфликтов уникальности
            string uniqueUtilityName = $"Тестовая услуга {Guid.NewGuid().ToString("N")[..6]}";
            const string testIconClass = "fa-solid fa-bolt";

            // 2. Физически создаем коммунальную услугу в базе данных Docker-контейнера.
            // Привязка к пользователю не требуется, так как сущность является общей.
            Utility utility = new Utility
            {
                Name = uniqueUtilityName,
                IconClass = testIconClass,
                IsActive = true
            };

            await DbContext.Utilities.AddAsync(utility);
            await DbContext.SaveChangesAsync();

            // Сбрасываем кэш EF Core, чтобы тестовый веб-сервер делал честный запрос к СУБД PostgreSQL
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Отправляем GET-запрос по сгенерированному базой ID
            HttpResponseMessage response = await Client.GetAsync($"api/utilities/{utility.Id}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный статус 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем ответ в DTO получения услуги по ID
            GetUtilityByIdResponse? content = await response.Content.ReadFromJsonAsync<GetUtilityByIdResponse>();

            // Проверяем корректность возвращенных данных из API
            content.Should().NotBeNull();
            content!.Id.Should().Be(utility.Id);
            content.Name.Should().Be(uniqueUtilityName);

            // Если в вашем GetUtilityByIdResponse есть эти поля, раскомментируйте их:
            // content.IconClass.Should().Be(testIconClass);
            // content.IsActive.Should().BeTrue();
        }

        /// <summary>
        /// Проверяет, что при попытке получить коммунальную услугу по идентификатору, которого гарантированно 
        /// нет в базе данных, контроллер корректно обрабатывает ситуацию и возвращает статус 404 Not Found.
        /// </summary>
        [Fact]
        public async Task GetById_Should_ReturnNotFound_When_UtilityDoesNotExist()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================

            // Поскольку перед каждым тестом Respawn полностью очищает все таблицы,
            // идентификатор '1' гарантированно отсутствует в базе данных PostgreSQL.
            const int nonExistentUtilityId = 1;

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Пытаемся получить несуществующий объект через HttpClient
            HttpResponseMessage response = await Client.GetAsync($"api/utilities/{nonExistentUtilityId}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный статус 404 Not Found, который должен вернуть контроллер
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Проверяет, что при вызове без параметров метод возвращает полный список коммунальных услуг 
        /// со статусом 200 OK, включая как активные, так и неактивные (архивные) записи.
        /// </summary>
        [Fact]
        public async Task GetAll_Should_ReturnAllUtilities_When_NoFilterProvided()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            string activeName = $"Активная услуга {Guid.NewGuid().ToString("N")[..6]}";
            string inactiveName = $"Архивная услуга {Guid.NewGuid().ToString("N")[..6]}";
            const string testIconClass = "fa-solid fa-bolt";

            Utility activeUtility = new Utility { Name = activeName, IconClass = testIconClass, IsActive = true };
            Utility inactiveUtility = new Utility { Name = inactiveName, IconClass = testIconClass, IsActive = false };

            await DbContext.Utilities.AddRangeAsync(activeUtility, inactiveUtility);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса без query-параметров)
            // ==========================================
            HttpResponseMessage response = await Client.GetAsync("api/utilities");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            GetUtilitiesListResponse? content = await response.Content.ReadFromJsonAsync<GetUtilitiesListResponse>();
            content.Should().NotBeNull();

            // По умолчанию фильтр null, должны вернуться ОБЕ записи
            content!.Items.Should().Contain(u => u.Name == activeName);
            content.Items.Should().Contain(u => u.Name == inactiveName);
        }

        /// <summary>
        /// Проверяет, что при передаче параметра isActive=true метод возвращает 
        /// только активные коммунальные услуги и игнорирует архивные.
        /// </summary>
        [Fact]
        public async Task GetAll_Should_ReturnOnlyActiveUtilities_When_IsActiveFilterIsTrue()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            string activeName = $"Активная услуга {Guid.NewGuid().ToString("N")[..6]}";
            string inactiveName = $"Архивная услуга {Guid.NewGuid().ToString("N")[..6]}";
            const string testIconClass = "fa-solid fa-bolt";

            Utility activeUtility = new Utility { Name = activeName, IconClass = testIconClass, IsActive = true };
            Utility inactiveUtility = new Utility { Name = inactiveName, IconClass = testIconClass, IsActive = false };

            await DbContext.Utilities.AddRangeAsync(activeUtility, inactiveUtility);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса с фильтром ?isActive=true)
            // ==========================================
            HttpResponseMessage response = await Client.GetAsync("api/utilities?isActive=true");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            GetUtilitiesListResponse? content = await response.Content.ReadFromJsonAsync<GetUtilitiesListResponse>();
            content.Should().NotBeNull();

            // В списке должна быть только активная услуга
            content!.Items.Should().Contain(u => u.Name == activeName);
            content.Items.Should().NotContain(u => u.Name == inactiveName);
        }

        /// <summary>
        /// Проверяет, что при передаче параметра isActive=false метод возвращает 
        /// только деактивированные (архивные) коммунальные услуги.
        /// </summary>
        [Fact]
        public async Task GetAll_Should_ReturnOnlyInactiveUtilities_When_IsActiveFilterIsFalse()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            string activeName = $"Активная услуга {Guid.NewGuid().ToString("N")[..6]}";
            string inactiveName = $"Архивная услуга {Guid.NewGuid().ToString("N")[..6]}";
            const string testIconClass = "fa-solid fa-bolt";

            Utility activeUtility = new Utility { Name = activeName, IconClass = testIconClass, IsActive = true };
            Utility inactiveUtility = new Utility { Name = inactiveName, IconClass = testIconClass, IsActive = false };

            await DbContext.Utilities.AddRangeAsync(activeUtility, inactiveUtility);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса с фильтром ?isActive=false)
            // ==========================================
            HttpResponseMessage response = await Client.GetAsync("api/utilities?isActive=false");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            GetUtilitiesListResponse? content = await response.Content.ReadFromJsonAsync<GetUtilitiesListResponse>();
            content.Should().NotBeNull();

            // В списке должна быть только архивная услуга
            content!.Items.Should().NotContain(u => u.Name == activeName);
            content.Items.Should().Contain(u => u.Name == inactiveName);
        }

        /// <summary>
        /// Проверяет, что при отправке валидных данных для редактирования, контроллер успешно обновляет 
        /// данные коммунальной услуги в PostgreSQL и возвращает статус 200 OK с обновленными данными.
        /// </summary>
        [Fact]
        public async Task Edit_Should_UpdateUtilityInDatabase_And_ReturnOkStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            string initialUtilityName = $"Старая услуга {Guid.NewGuid().ToString("N")[..6]}";
            string updatedUtilityName = $"Новая услуга {Guid.NewGuid().ToString("N")[..6]}";

            const string initialIconClass = "fa-solid fa-bolt";
            const string updatedIconClass = "fa-solid fa-water";

            // Создаем исходную запись в БД (Услуга изначально активна)
            Utility utility = new Utility
            {
                Name = initialUtilityName,
                IconClass = initialIconClass,
                IsActive = true
            };
            await DbContext.Utilities.AddAsync(utility);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // Формируем DTO запроса на редактирование. 
            // Предполагается, что структура EditUtilityRequest принимает (Name, IconClass, IsActive) по аналогии с Create
            EditUtilityRequest request = new EditUtilityRequest(updatedUtilityName, updatedIconClass, false);

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            // ID передается в URL маршрута согласно REST-памятке контроллера
            HttpResponseMessage response = await Client.PutAsJsonAsync($"api/utilities/{utility.Id}", request);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            EditUtilityResponse? responseContent = await response.Content.ReadFromJsonAsync<EditUtilityResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().Be(utility.Id);

            // Проверяем, что API вернул измененные данные в ответе
            // (Если в EditUtilityResponse нет этих полей, закомментируйте проверки полей DTO)
            // responseContent.Name.Should().Be(updatedUtilityName);

            // Проверяем физическое изменение данных прямо в базе данных PostgreSQL
            DbContext.ChangeTracker.Clear();
            Utility? utilityInDb = await DbContext.Utilities
                .FirstOrDefaultAsync(u => u.Id == utility.Id);

            // Доказываем, что все поля успешно перезаписались в СУБД
            utilityInDb.Should().NotBeNull();
            utilityInDb!.Name.Should().Be(updatedUtilityName);
            utilityInDb.IconClass.Should().Be(updatedIconClass);
            utilityInDb.IsActive.Should().BeFalse(); // Проверяем, что статус активности тоже изменился
        }

        /// <summary>
        /// Проверяет, что метод удаления успешно деактивирует запись коммунальной услуги (Soft Delete) 
        /// в базе данных и возвращает каноничный REST-статус 204 No Content без тела ответа.
        /// </summary>
        [Fact]
        public async Task Delete_Should_DeactivateUtilityInDatabase_And_ReturnNoContentStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            string utilityNameToDelete = $"Услуга для удаления {Guid.NewGuid().ToString("N")[..6]}";
            const string testIconClass = "fa-solid fa-trash";

            // Создаем изначально активную услугу, которую будем "удалять"
            Utility utility = new Utility
            {
                Name = utilityNameToDelete,
                IconClass = testIconClass,
                IsActive = true
            };
            await DbContext.Utilities.AddAsync(utility);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            HttpResponseMessage response = await Client.DeleteAsync($"api/utilities/{utility.Id}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.NoContent); // Каноничный статус для DELETE без тела

            // Проверяем состояние записи прямо в базе данных PostgreSQL
            DbContext.ChangeTracker.Clear();

            // Если у вас в DbContext настроен глобальный фильтр на сокрытие удаленных записей (например, HasQueryFilter(u => u.IsActive)),
            // то здесь ОБЯЗАТЕЛЬНО оставляем .IgnoreQueryFilters(), чтобы EF Core смог найти деактивированную строку.
            Utility? utilityInDb = await DbContext.Utilities
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(u => u.Id == utility.Id);

            // ВНИМАНИЕ: Так как у вас используется Мягкое удаление (Soft Delete):
            // Запись НЕ должна удаляться физически (она не null), но её статус активности должен стать false.
            utilityInDb.Should().NotBeNull();
            utilityInDb!.IsActive.Should().BeFalse(); // Доказываем, что услуга успешно деактивирована / мягко удалена
        }
    }
}
