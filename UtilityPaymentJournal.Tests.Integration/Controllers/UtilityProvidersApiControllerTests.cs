using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using UtilityPaymentJournal.Features.UtilityProviders.Create;
using UtilityPaymentJournal.Features.UtilityProviders.Edit;
using UtilityPaymentJournal.Features.UtilityProviders.GetById;
using UtilityPaymentJournal.Features.UtilityProviders.GetList;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;
using UtilityPaymentJournal.Tests.Integration.Infrastructure;

namespace UtilityPaymentJournal.Tests.Integration.Controllers
{
    [Collection(nameof(IntegrationTestCollection))]
    public class UtilityProvidersApiControllerTests : BaseIntegrationTest
    {
        public UtilityProvidersApiControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Проверяет, что при отправке валидных данных контроллер успешно создает запись поставщика услуг, 
        /// сохраняет её в PostgreSQL, автоматически привязывает UserId автора и возвращает статус 201 Created.
        /// </summary>
        [Fact]
        public async Task Create_Should_SaveUtilityProviderInDatabase_And_ReturnCreatedStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных и окружения)
            // ==========================================

            // 1. Идентификатор пользователя, зашитый в наш TestAuthHandler
            const string testUserId = "test-admin-id-123";

            // 2. Генерируем уникальное наименование для предотвращения конфликтов уникальности (Unique Constraints) в БД
            string uniqueUtilityProviderName = $"Тестовый поставщик услуг {Guid.NewGuid().ToString("N")[..6]}";

            // 3. Гарантируем наличие пользователя в Docker-базе, чтобы не нарушить Foreign Key в PostgreSQL
            // IgnoreQueryFilters() здесь для подстраховки от скрытых фильтраций
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                User testUser = new User
                {
                    Id = testUserId,
                    UserName = "testadmin",
                    FirstName = "Тест",
                    LastName = "Админ"
                };

                await DbContext.Users.AddAsync(testUser);
                await DbContext.SaveChangesAsync();
                DbContext.ChangeTracker.Clear(); // Очищаем кэш EF Core после сохранения
            }

            // 4. Формируем тело запроса (DTO)
            CreateUtilityProviderRequest request = new CreateUtilityProviderRequest(uniqueUtilityProviderName);

            // ==========================================
            // ACT (Выполнение целевого действия)
            // ==========================================

            // Отправляем запрос на создание. Метод расширения сам сериализует DTO в JSON.
            HttpResponseMessage response = await Client.PostAsJsonAsync("api/utility-providers", request);

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
            CreateUtilityProviderResponse? responseContent = await response.Content.ReadFromJsonAsync<CreateUtilityProviderResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().BeGreaterThan(0);

            // Сбрасываем кэш отслеживания (Change Tracker) EF Core перед прямым запросом в СУБД,
            // чтобы получить актуальные данные из PostgreSQL, а не локального кэша
            DbContext.ChangeTracker.Clear();

            // ГЛАВНАЯ ПРОВЕРКА В БАЗЕ ДАННЫХ POSTGRESQL
            //
            // ВНИМАНИЕ: Здесь ОБЯЗАТЕЛЬНО нужно использовать .IgnoreQueryFilters()!
            // Почему без него EF Core вернет null и тест упадет:
            // 
            // 1. В приложении настроен глобальный фильтр безопасности: он автоматически дописывает 
            //    к каждому SQL-запросу условие [WHERE "UserId" = текущий_пользовательский_ID].
            // 2. Когда запрос шел через HttpClient в контроллер, TestAuthHandler подставил пользователя, 
            //    и в базу данных Postgres честно записался жилой объект с UserId = "test-admin-id-123".
            // 3. Но сейчас мы находимся внутри ТЕСТОВОГО МЕТОДА. Здесь нет HTTP-запроса, нет заголовков,
            //    и для этого экземпляра DbContext текущий пользователь равен NULL.
            // 4. Если убрать .IgnoreQueryFilters(), EF Core сгенерирует SQL: [WHERE "Id" = 1 AND "UserId" IS NULL].
            //    Поскольку в базе у записи UserId заполнен ("test-admin-id-123"), Postgres вернет пустой результат.
            //
            // Метод .IgnoreQueryFilters() принудительно отключает скрытую фильтрацию EF Core, делая 
            // честный прямой запрос в базу, чтобы мы могли вытащить запись и проверить ее реальное содержимое.
            UtilityProvider? utilityProviderInDb = await DbContext.UtilityProviders
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == responseContent.Id);

            // Проверяем, что запись физически создана в Docker-контейнере и ее поля заполнены корректно
            utilityProviderInDb.Should().NotBeNull();
            utilityProviderInDb!.Name.Should().Be(uniqueUtilityProviderName);

            // КРИТИЧЕСКИ ВАЖНАЯ ПРОВЕРКА: Доказываем, что логика автоматической привязки владельца 
            // (ApplyUserOwnership) сработала корректно и привязала запись именно к нашему текущему 
            // пользователю из TestAuthHandler. Это гарантирует, что глобальные фильтры (Query Filters) 
            // увидят эту запись при последующих GET-запросах.
            //
            // ПОЧЕМУ ТУТ СВОЙСТВО UserId НЕ NULL И СОДЕРЖИТ РЕАЛЬНОЕ ЗНАЧЕНИЕ:
            // Благодаря .IgnoreQueryFilters() база данных PostgreSQL вернула нам строку целиком,
            // проигнорировав системный пустой контекст теста на этапе ПОИСКА записи.
            // EF Core взял эти сырые данные из Postgres и честно заполнил ими свойства C#-объекта. 
            // Поэтому в самом объекте residenceInDb поле UserId теперь заполнено и готово к проверке!
            utilityProviderInDb.UserId.Should().Be(testUserId);
        }

        /// <summary>
        /// Проверяет, что существующая в базе данных запись поставщика услуг успешно возвращается по её идентификатору (ID)
        /// со статусом 200 OK, при условии, что запись принадлежит текущему авторизованному пользователю.
        /// </summary>
        [Fact]
        public async Task GetById_Should_ReturnUtilityProvider_When_ItExistsInDatabase()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================

            // 1. Указываем ID пользователя из нашего TestAuthHandler
            const string testUserId = "test-admin-id-123";

            // 2. Генерируем уникальное наименование в едином стиле для исключения конфликтов уникальности
            string uniqueUtilityProviderName = $"Тестовый поставщик услуг {Guid.NewGuid().ToString("N")[..6]}";

            // 3. Гарантируем, что пользователь существует в Docker-базе для соблюдения Foreign Key
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = testUserId, UserName = "testadmin", FirstName = "Тест", LastName = "Админ" });
                await DbContext.SaveChangesAsync();
                DbContext.ChangeTracker.Clear();
            }

            // 4. Физически создаем дом и ЯВНО привязываем его к нашему тест-пользователю.
            // Это критически важно: если не прописать UserId, глобальный фильтр (Query Filter) 
            // на стороне веб-сервера просто скроет эту запись, и API вернет 404 Not Found!
            UtilityProvider utilityProvider = new UtilityProvider
            {
                Name = uniqueUtilityProviderName,
                UserId = testUserId // Жестко связываем запись с текущим авторизованным контекстом
            };

            await DbContext.UtilityProviders.AddAsync(utilityProvider);
            await DbContext.SaveChangesAsync();

            // Сбрасываем кэш EF Core, чтобы тестовый сервер делал честный запрос к дисковой СУБД, а не к памяти
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Отправляем GET-запрос по сгенерированному базой ID
            HttpResponseMessage response = await Client.GetAsync($"api/utility-providers/{utilityProvider.Id}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный статус 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем ответ (убедитесь, что имя вашего DTO совпадает)
            GetUtilityProviderByIdResponse? content = await response.Content.ReadFromJsonAsync<GetUtilityProviderByIdResponse>();

            content.Should().NotBeNull();
            content!.Id.Should().Be(utilityProvider.Id);
            content.Name.Should().Be(uniqueUtilityProviderName);
        }

        /// <summary>
        /// Проверяет, что при попытке получить gставщика услуг по идентификатору, которого гарантированно 
        /// нет в базе данных, контроллер корректно обрабатывает ситуацию и возвращает статус 404 Not Found.
        /// </summary>
        [Fact]
        public async Task GetById_Should_ReturnNotFound_When_UtilityProviderDoesNotExist()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================

            // Поскольку перед каждым тестом Respawn полностью очищает все таблицы,
            // идентификатор '1' гарантированно отсутствует в базе данных PostgreSQL.
            // Это гораздо надежнее, чем зашивать случайные числа вроде 99999.
            const int nonExistentUtilityProviderId = 1;

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Пытаемся получить несуществующий объект через HttpClient
            HttpResponseMessage response = await Client.GetAsync($"api/utility-providers/{nonExistentUtilityProviderId}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный статус 404 Not Found, который должен вернуть контроллер
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Проверяет, что метод получения списка успешно возвращает коллекцию поставщиков услуг со статусом 200 OK,
        /// причем в список попадают только те объекты, которые принадлежат текущему авторизованному пользователю.
        /// </summary>
        [Fact]
        public async Task GetAll_Should_ReturnUtilityProvidersList_OwnedByCurrentUser()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            const string currentUserId = "test-admin-id-123";
            const string otherUserId = "some-other-user-id-999";

            // Гарантируем наличие текущего и стороннего пользователей для проверки изоляции прав
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == currentUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = currentUserId, UserName = "current", FirstName = "Тест", LastName = "Админ" });
            }
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == otherUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = otherUserId, UserName = "other", FirstName = "Чужой", LastName = "Юзер" });
            }
            await DbContext.SaveChangesAsync();

            string myUtilityProviderName = $"Тестовый поставщик услуг мой {Guid.NewGuid().ToString("N")[..6]}";
            string otherUtilityProviderName = $"Тестовый поставщик улсуг чужой {Guid.NewGuid().ToString("N")[..6]}";

            UtilityProvider myUtilityProvider = new UtilityProvider { Name = myUtilityProviderName, UserId = currentUserId };
            UtilityProvider otherUtilityProvider = new UtilityProvider { Name = otherUtilityProviderName, UserId = otherUserId };

            await DbContext.UtilityProviders.AddRangeAsync(myUtilityProvider, otherUtilityProvider);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            HttpResponseMessage response = await Client.GetAsync("api/utility-providers");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            GetUtilityProvidersListResponse? content = await response.Content.ReadFromJsonAsync<GetUtilityProvidersListResponse>();
            content.Should().NotBeNull();
            content!.Items.Should().NotBeNull();

            // Проверяем, что глобальный фильтр на стороне сервера отдал НАШУ запись, но скрыл ЧУЖУЮ
            content.Items.Should().ContainSingle(r => r.Name == myUtilityProviderName);
            content.Items.Should().NotContain(r => r.Name == otherUtilityProviderName);
        }

        /// <summary>
        /// Проверяет, что при отправке валидных данных для редактирования, контроллер успешно обновляет 
        /// наименование поставщика услуг в PostgreSQL и возвращает статус 200 OK с обновленными данными.
        /// </summary>
        [Fact]
        public async Task Edit_Should_UpdateUtilityProviderInDatabase_And_ReturnOkStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            const string testUserId = "test-admin-id-123";
            string initialUtilityProviderName = $"Старый поставщик услуг {Guid.NewGuid().ToString("N")[..6]}";
            string updatedUtilityProviderName = $"Новый поставщик услуг {Guid.NewGuid().ToString("N")[..6]}";

            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = testUserId, UserName = "testadmin", FirstName = "Тест", LastName = "Админ" });
            }

            // Создаем исходную запись в БД
            UtilityProvider utilityProvider = new UtilityProvider { Name = initialUtilityProviderName, UserId = testUserId };
            await DbContext.UtilityProviders.AddAsync(utilityProvider);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // Формируем DTO запроса на редактирование (без ID, так как ID передается в URL)
            EditUtilityProviderRequest request = new EditUtilityProviderRequest(updatedUtilityProviderName);

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            HttpResponseMessage response = await Client.PutAsJsonAsync($"api/utility-providers/{utilityProvider.Id}", request);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            EditUtilityProviderResponse? responseContent = await response.Content.ReadFromJsonAsync<EditUtilityProviderResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().Be(utilityProvider.Id);
            responseContent.Name.Should().Be(updatedUtilityProviderName);

            // Проверяем физическое изменение данных прямо в базе PostgreSQL
            DbContext.ChangeTracker.Clear();
            UtilityProvider? residenceInDb = await DbContext.UtilityProviders
                .IgnoreQueryFilters() // Используем страховку поиска
                .FirstOrDefaultAsync(r => r.Id == utilityProvider.Id);

            residenceInDb.Should().NotBeNull();
            residenceInDb!.Name.Should().Be(updatedUtilityProviderName); // Доказываем, что адрес перезаписался
            residenceInDb.UserId.Should().Be(testUserId); // Проверяем, что владелец не изменился
        }

        /// <summary>
        /// Проверяет, что метод удаления успешно удаляет запись поставщика услуг из базы данных 
        /// и возвращает каноничный REST-статус 204 No Content без тела ответа.
        /// </summary>
        [Fact]
        public async Task Delete_Should_RemoveUtilityProviderFromDatabase_And_ReturnNoContentStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            const string testUserId = "test-admin-id-123";
            string utilityProviderNameToDelete = $"Поставщик услуг для удаления {Guid.NewGuid().ToString("N")[..6]}";

            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = testUserId, UserName = "testadmin", FirstName = "Тест", LastName = "Админ" });
            }

            UtilityProvider utilityProvider = new UtilityProvider { Name = utilityProviderNameToDelete, UserId = testUserId };
            await DbContext.UtilityProviders.AddAsync(utilityProvider);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            HttpResponseMessage response = await Client.DeleteAsync($"api/utility-providers/{utilityProvider.Id}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.NoContent); // Каноничный статус для DELETE без тела

            // Проверяем отсутствие записи в базе данных PostgreSQL
            DbContext.ChangeTracker.Clear();
            UtilityProvider? residenceInDb = await DbContext.UtilityProviders
                .IgnoreQueryFilters() // Отключаем фильтры, чтобы убедиться, что записи нет совсем (а не она просто скрыта)
                .FirstOrDefaultAsync(r => r.Id == utilityProvider.Id);

            residenceInDb.Should().BeNull(); // Запись должна полностью исчезнуть (или быть помечена как удаленная, если у вас Soft Delete)
        }
    }
}
