using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Features.WaterReadings.Create;
using UtilityPaymentJournal.Features.WaterReadings.Edit;
using UtilityPaymentJournal.Features.WaterReadings.GetById;
using UtilityPaymentJournal.Features.WaterReadings.GetList;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;
using UtilityPaymentJournal.Infrastructure.EF.Entity.WaterReadings;
using UtilityPaymentJournal.Tests.Integration.Infrastructure;

namespace UtilityPaymentJournal.Tests.Integration.Controllers
{
    [Collection(nameof(IntegrationTestCollection))]
    public class WaterReadingsControllerTests : BaseIntegrationTest
    {
        public WaterReadingsControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Проверяет, что при отправке валидных данных контроллер успешно создает запись, 
        /// сохраняет её в PostgreSQL, автоматически привязывает UserId автора и возвращает статус 201 Created.
        /// </summary>
        [Fact]
        public async Task Create_Should_SaveWaterReadingInDatabase_And_ReturnCreatedStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных и окружения)
            // ==========================================

            // 1. Идентификатор пользователя, зашитый в наш TestAuthHandler
            const string testUserId = "test-admin-id-123";

            // 2. Гарантируем наличие пользователя в Docker-базе (Foreign Key для UserId)
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
            }

            // 3. Создаем тестового Поставщика услуг (UtilityProvider) для Foreign Key
            UtilityProvider testProvider = new UtilityProvider
            {
                Name = $"Тестовый поставщик {Guid.NewGuid().ToString("N")[..6]}",
                UserId = testUserId
            };
            await DbContext.UtilityProviders.AddAsync(testProvider);

            // 4. Создаем тестовый Жилой объект (Residence) для Foreign Key
            // Примечание: Если в вашей сущности Residence обязательны другие поля, заполните их здесь
            Residence testResidence = new Residence
            {
                Address = $"Тестовый адрес {Guid.NewGuid().ToString("N")[..6]}",
                UserId = testUserId
            };
            await DbContext.Residences.AddAsync(testResidence);

            // Сохраняем зависимости в базу и очищаем Change Tracker
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // 5. Генерируем тестовые значения для показаний и дат
            long testPreviousValue = Random.Shared.Next(100, 500);
            long testCurrentValue = testPreviousValue + Random.Shared.Next(5, 20);
            long testResultValue = testCurrentValue - testPreviousValue;
            decimal testPaymentAmount = testResultValue * 45.20m;

            DateTime testSubmissionDate = DateTime.UtcNow.Date;
            DateTime testPaymentDate = DateTime.UtcNow.Date.AddDays(1);

            // 6. Формируем тело запроса (DTO) строго по вашему record
            CreateWaterReadingRequest request = new CreateWaterReadingRequest(
                ResidenceId: testResidence.Id,
                UtilityProviderId: testProvider.Id,
                WaterType: WaterType.Cold, // Предполагаем, что Cold есть в вашем WaterType enum
                SubmissionDate: testSubmissionDate,
                PaymentDate: testPaymentDate,
                CurrentValue: testCurrentValue,
                PreviousValue: testPreviousValue,
                ResultValue: testResultValue,
                PaymentAmount: testPaymentAmount
            );

            // ==========================================
            // ACT (Выполнение целевого действия)
            // ==========================================

            HttpResponseMessage response = await Client.PostAsJsonAsync("api/water-readings", request);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Перехватываем 400 BadRequest для удобной отладки
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string errorText = await response.Content.ReadAsStringAsync();
                throw new Xunit.Sdk.XunitException($"[400 BadRequest] Ошибка валидации от API: {errorText}");
            }

            // Проверяем каноничный статус 201 Created и заголовок Location
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

            // Десериализуем ответ API
            CreateWaterReadingResponse? responseContent = await response.Content.ReadFromJsonAsync<CreateWaterReadingResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().BeGreaterThan(0);

            // Сбрасываем кэш EF Core перед проверкой в PostgreSQL
            DbContext.ChangeTracker.Clear();

            // ГЛАВНАЯ ПРОВЕРКА В БАЗЕ ДАННЫХ
            WaterReading? waterReadingInDb = await DbContext.WaterReadings
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == responseContent.Id);

            // Проверяем физическое существование и корректность маппинга всех полей
            waterReadingInDb.Should().NotBeNull();
            waterReadingInDb!.ResidenceId.Should().Be(testResidence.Id);
            waterReadingInDb!.UtilityProviderId.Should().Be(testProvider.Id);
            waterReadingInDb!.WaterType.Should().Be(WaterType.Cold);
            waterReadingInDb!.SubmissionDate.Should().Be(testSubmissionDate);
            waterReadingInDb!.PaymentDate.Should().Be(testPaymentDate);
            waterReadingInDb!.CurrentValue.Should().Be(testCurrentValue);
            waterReadingInDb!.PreviousValue.Should().Be(testPreviousValue);
            waterReadingInDb!.ResultValue.Should().Be(testResultValue);
            waterReadingInDb!.PaymentAmount.Should().Be(testPaymentAmount);

            // Проверяем корректность работы механизма ApplyUserOwnership
            waterReadingInDb.UserId.Should().Be(testUserId);
        }

        /// <summary>
        /// Проверяет, что существующая в базе данных запись показания счетчиков воды успешно возвращается по её идентификатору (ID)
        /// со статусом 200 OK, при условии, что запись принадлежит текущему авторизованному пользователю.
        /// </summary>
        [Fact]
        public async Task GetById_Should_ReturnWaterReading_When_ItExistsInDatabase()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================

            // 1. Указываем ID пользователя из нашего TestAuthHandler
            const string testUserId = "test-admin-id-123";

            // 2. Гарантируем, что пользователь существует в Docker-базе для соблюдения Foreign Key
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = testUserId, UserName = "testadmin", FirstName = "Тест", LastName = "Админ" });
                await DbContext.SaveChangesAsync();
            }

            // 3. Создаем тестового Поставщика услуг (UtilityProvider) для Foreign Key и текстового названия
            string uniqueProviderName = $"Тестовый поставщик {Guid.NewGuid().ToString("N")[..6]}";
            UtilityProvider testProvider = new UtilityProvider
            {
                Name = uniqueProviderName,
                UserId = testUserId
            };
            await DbContext.UtilityProviders.AddAsync(testProvider);

            // 4. Создаем тестовый Жилой объект (Residence) для Foreign Key и текстового адреса
            string uniqueAddress = $"Тестовый адрес {Guid.NewGuid().ToString("N")[..6]}";
            Residence testResidence = new Residence
            {
                Address = uniqueAddress,
                UserId = testUserId
            };
            await DbContext.Residences.AddAsync(testResidence);

            // Сохраняем зависимости, чтобы получить их сгенерированные базой ID
            await DbContext.SaveChangesAsync();

            // 5. Генерируем тестовые значения показаний
            long testPreviousValue = Random.Shared.Next(100, 500);
            long testCurrentValue = testPreviousValue + Random.Shared.Next(5, 20);
            long testResultValue = testCurrentValue - testPreviousValue;
            decimal testPaymentAmount = testResultValue * 45.20m;

            DateTime testSubmissionDate = DateTime.UtcNow.Date;
            DateTime testPaymentDate = DateTime.UtcNow.Date.AddDays(1);

            // 6. Физически создаем показание счетчика воды и ЯВНО привязываем его к нашему тест-пользователю.
            // Это критически важно: если не прописать UserId, глобальный фильтр (Query Filter) 
            // на стороне веб-сервера просто скроет эту запись, и API вернет 404 Not Found!
            WaterReading waterReading = new WaterReading
            {
                ResidenceId = testResidence.Id,
                UtilityProviderId = testProvider.Id,
                UserId = testUserId, // Жестко связываем запись с текущим авторизованным контекстом
                WaterType = WaterType.Cold,
                CurrentValue = testCurrentValue,
                PreviousValue = testPreviousValue,
                ResultValue = testResultValue,
                PaymentAmount = testPaymentAmount,
                SubmissionDate = testSubmissionDate,
                PaymentDate = testPaymentDate
            };

            await DbContext.WaterReadings.AddAsync(waterReading);
            await DbContext.SaveChangesAsync();

            // Сбрасываем кэш EF Core, чтобы тестовый сервер делал честный запрос к дисковой СУБД, а не к памяти
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Отправляем GET-запрос по сгенерированному базой ID на новый эндпоинт api/water-readings/{id}
            HttpResponseMessage response = await Client.GetAsync($"api/water-readings/{waterReading.Id}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный статус 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем ответ строго по вашему record DTO
            GetWaterReadingByIdResponse? content = await response.Content.ReadFromJsonAsync<GetWaterReadingByIdResponse>();

            // Проверяем, что API вернуло именно те данные, которые мы сохранили в базу
            content.Should().NotBeNull();
            content!.Id.Should().Be(waterReading.Id);
            content.ResidenceId.Should().Be(testResidence.Id);
            content.UtilityProviderId.Should().Be(testProvider.Id);
            content.WaterType.Should().Be(WaterType.Cold);

            // ПРОВЕРКА ПРАВИЛЬНОСТИ СВЯЗЕЙ (Проверяем, что MediatR/AutoMapper подтянул данные из соседних таблиц)
            content.ResidenceAddress.Should().Be(uniqueAddress);
            content.UtilityProviderName.Should().Be(uniqueProviderName);

            // Проверяем сохраненные значения
            content.SubmissionDate.Should().Be(testSubmissionDate);
            content.PaymentDate.Should().Be(testPaymentDate);
            content.CurrentValue.Should().Be(testCurrentValue);
            content.PreviousValue.Should().Be(testPreviousValue);
            content.ResultValue.Should().Be(testResultValue);
            content.PaymentAmount.Should().Be(testPaymentAmount);
        }

        /// <summary>
        /// Проверяет, что при попытке получить показание счетчиков воды по идентификатору, которого гарантированно 
        /// нет в базе данных, контроллер корректно обрабатывает ситуацию и возвращает статус 404 Not Found.
        /// </summary>
        [Fact]
        public async Task GetById_Should_ReturnNotFound_When_WaterReadingDoesNotExist()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================

            // Поскольку перед каждым тестом Respawn полностью очищает все таблицы,
            // идентификатор '1' гарантированно отсутствует в базе данных PostgreSQL.
            // Это гораздо надежнее, чем зашивать случайные числа вроде 99999.
            const int nonExistentWaterReadingId = 1;

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Пытаемся получить несуществующий объект через HttpClient на новом эндпоинте
            HttpResponseMessage response = await Client.GetAsync($"api/water-readings/{nonExistentWaterReadingId}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный статус 404 Not Found, который должен вернуть контроллер
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Проверяет, что метод получения списка успешно возвращает коллекцию показаний счетчиков воды со статусом 200 OK,
        /// причем в список попадают только те объекты, которые принадлежат текущему авторизованному пользователю.
        /// </summary>
        [Fact]
        public async Task GetAll_Should_ReturnWaterReadingsList_OwnedByCurrentUser()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            const string currentUserId = "test-admin-id-123";
            const string otherUserId = "some-other-user-id-999";

            // 1. Гарантируем наличие текущего и стороннего пользователей для проверки изоляции прав
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == currentUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = currentUserId, UserName = "current", FirstName = "Тест", LastName = "Админ" });
            }
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == otherUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = otherUserId, UserName = "other", FirstName = "Чужой", LastName = "Юзер" });
            }
            await DbContext.SaveChangesAsync();

            // 2. Создаем окружение для ТЕКУЩЕГО пользователя (Жилье и Поставщик)
            Residence myResidence = new Residence { Address = $"Мой адрес {Guid.NewGuid().ToString("N")[..6]}", UserId = currentUserId };
            UtilityProvider myProvider = new UtilityProvider { Name = $"Мой поставщик {Guid.NewGuid().ToString("N")[..6]}", UserId = currentUserId };
            await DbContext.Residences.AddAsync(myResidence);
            await DbContext.UtilityProviders.AddAsync(myProvider);

            // 3. Создаем окружение для ЧУЖОГО пользователя (Жилье и Поставщик)
            Residence otherResidence = new Residence { Address = $"Чужой адрес {Guid.NewGuid().ToString("N")[..6]}", UserId = otherUserId };
            UtilityProvider otherProvider = new UtilityProvider { Name = $"Чужой поставщик {Guid.NewGuid().ToString("N")[..6]}", UserId = otherUserId };
            await DbContext.Residences.AddAsync(otherResidence);
            await DbContext.UtilityProviders.AddAsync(otherProvider);

            // Сохраняем зависимости, чтобы сгенерировать ID для внешних ключей показаний
            await DbContext.SaveChangesAsync();

            // 4. Генерируем уникальные значения для показаний, чтобы однозначно идентифицировать их в ассертах
            long myUniqueValue = Random.Shared.Next(1000, 5000);
            long otherUniqueValue = Random.Shared.Next(6000, 9000);

            // 5. Физически создаем две записи показаний: одну нашу, одну чужую
            WaterReading myWaterReading = new WaterReading
            {
                ResidenceId = myResidence.Id,
                UtilityProviderId = myProvider.Id,
                UserId = currentUserId, // Жестко привязываем к текущему юзеру
                WaterType = WaterType.Cold,
                CurrentValue = myUniqueValue,
                PreviousValue = 100,
                ResultValue = myUniqueValue - 100,
                PaymentAmount = 150.00m
            };

            WaterReading otherWaterReading = new WaterReading
            {
                ResidenceId = otherResidence.Id,
                UtilityProviderId = otherProvider.Id,
                UserId = otherUserId, // Жестко привязываем к чужому юзеру
                WaterType = WaterType.Hot,
                CurrentValue = otherUniqueValue,
                PreviousValue = 100,
                ResultValue = otherUniqueValue - 100,
                PaymentAmount = 300.00m
            };

            await DbContext.WaterReadings.AddRangeAsync(myWaterReading, otherWaterReading);
            await DbContext.SaveChangesAsync();

            // Сбрасываем кэш EF Core перед вызовом API
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Отправляем GET-запрос на получение списка показаний
            HttpResponseMessage response = await Client.GetAsync("api/water-readings");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем ответ (убедитесь, что имя вашего DTO совпадает)
            GetWaterReadingsListResponse? content = await response.Content.ReadFromJsonAsync<GetWaterReadingsListResponse>();
            content.Should().NotBeNull();
            content!.Items.Should().NotBeNull();

            // Проверяем, что глобальный фильтр на стороне сервера отдал НАШУ запись, но скрыл ЧУЖУЮ
            // Проверку делаем по уникальному значению CurrentValue (или любому другому удобному полю вашего элемента списка)
            content.Items.Should().ContainSingle(r => r.CurrentValue == myUniqueValue);
            content.Items.Should().NotContain(r => r.CurrentValue == otherUniqueValue);
        }

        /// <summary>
        /// Проверяет, что при отправке валидных данных для редактирования, контроллер успешно обновляет 
        /// показания счетчиков воды в PostgreSQL и возвращает статус 200 OK с обновленными данными.
        /// </summary>
        [Fact]
        public async Task Edit_Should_UpdateWaterReadingInDatabase_And_ReturnOkStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            const string testUserId = "test-admin-id-123";

            // 1. Гарантируем наличие пользователя в Docker-базе
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = testUserId, UserName = "testadmin", FirstName = "Тест", LastName = "Админ" });
                await DbContext.SaveChangesAsync();
            }

            // 2. Создаем инфраструктуру (Жилье и Поставщик) для Foreign Keys
            Residence testResidence = new Residence { Address = $"Тестовый адрес {Guid.NewGuid().ToString("N")[..6]}", UserId = testUserId };
            UtilityProvider testProvider = new UtilityProvider { Name = $"Тестовый поставщик {Guid.NewGuid().ToString("N")[..6]}", UserId = testUserId };
            await DbContext.Residences.AddAsync(testResidence);
            await DbContext.UtilityProviders.AddAsync(testProvider);
            await DbContext.SaveChangesAsync();

            // 3. Физически создаем ИСХОДНУЮ запись показаний в БД
            WaterReading waterReading = new WaterReading
            {
                ResidenceId = testResidence.Id,
                UtilityProviderId = testProvider.Id,
                UserId = testUserId,
                WaterType = WaterType.Cold,
                CurrentValue = 150,
                PreviousValue = 140,
                ResultValue = 10,
                PaymentAmount = 450.00m,
                SubmissionDate = DateTime.UtcNow.Date.AddDays(-30),
                PaymentDate = DateTime.UtcNow.Date.AddDays(-28)
            };
            await DbContext.WaterReadings.AddAsync(waterReading);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // 4. Готовим ОБНОВЛЕННЫЕ значения для отправки в запросе
            long updatedPreviousValue = 150; // Прошлые показания стали равны старым текущим
            long updatedCurrentValue = 165;
            long updatedResultValue = updatedCurrentValue - updatedPreviousValue;
            decimal updatedPaymentAmount = updatedResultValue * 48.50m; // Изменился тариф/сумма
            DateTime updatedSubmissionDate = DateTime.UtcNow.Date;
            DateTime updatedPaymentDate = DateTime.UtcNow.Date.AddDays(1);

            // 5. Формируем DTO запроса на редактирование (без ID, так как ID извлекается из URL маршрута)
            // Подставьте ваши реальные свойства рекорда EditWaterReadingRequest
            EditWaterReadingRequest request = new EditWaterReadingRequest(
                ResidenceId: testResidence.Id,
                UtilityProviderId: testProvider.Id,
                WaterType: WaterType.Cold,
                SubmissionDate: updatedSubmissionDate,
                PaymentDate: updatedPaymentDate,
                CurrentValue: updatedCurrentValue,
                PreviousValue: updatedPreviousValue,
                ResultValue: updatedResultValue,
                PaymentAmount: updatedPaymentAmount
            );

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            // Передаем ID в строке запроса, а DTO — в теле (BODY)
            HttpResponseMessage response = await Client.PutAsJsonAsync($"api/water-readings/{waterReading.Id}", request);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Перехватываем 400 BadRequest для удобной отладки при несовпадении валидации
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string errorText = await response.Content.ReadAsStringAsync();
                throw new Xunit.Sdk.XunitException($"[400 BadRequest] Ошибка валидации от API: {errorText}");
            }

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем и проверяем контент ответа API
            EditWaterReadingResponse? responseContent = await response.Content.ReadFromJsonAsync<EditWaterReadingResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().Be(waterReading.Id);

            // Проверяем маппинг обновленных полей в самом DTO ответа
            responseContent.CurrentValue.Should().Be(updatedCurrentValue);
            responseContent.PreviousValue.Should().Be(updatedPreviousValue);
            responseContent.ResultValue.Should().Be(updatedResultValue);
            responseContent.PaymentAmount.Should().Be(updatedPaymentAmount);

            // 6. ГЛАВНАЯ ПРОВЕРКА: Извлечение записи напрямую из PostgreSQL в обход кэша EF
            DbContext.ChangeTracker.Clear();
            WaterReading? waterReadingInDb = await DbContext.WaterReadings
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == waterReading.Id);

            // Доказываем, что все ключевые поля физически перезаписались новыми значениями
            waterReadingInDb.Should().NotBeNull();
            waterReadingInDb!.CurrentValue.Should().Be(updatedCurrentValue);
            waterReadingInDb!.PreviousValue.Should().Be(updatedPreviousValue);
            waterReadingInDb!.ResultValue.Should().Be(updatedResultValue);
            waterReadingInDb!.PaymentAmount.Should().Be(updatedPaymentAmount);
            waterReadingInDb!.SubmissionDate.Should().Be(updatedSubmissionDate);
            waterReadingInDb!.PaymentDate.Should().Be(updatedPaymentDate);

            // Проверяем, что критически важный владелец записи (UserId) остался прежним
            waterReadingInDb.UserId.Should().Be(testUserId);
        }

        /// <summary>
        /// Проверяет, что метод удаления успешно удаляет запись показания счетчиков воды из базы данных 
        /// и возвращает каноничный REST-статус 204 No Content без тела ответа.
        /// </summary>
        [Fact]
        public async Task Delete_Should_RemoveWaterReadingFromDatabase_And_ReturnNoContentStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            const string testUserId = "test-admin-id-123";

            // 1. Гарантируем наличие пользователя в Docker-базе для соблюдения Foreign Key владельца
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = testUserId, UserName = "testadmin", FirstName = "Тест", LastName = "Админ" });
            }

            // 2. Создаем в базе объект WaterReading, который будем удалять.
            // Заполняем только обязательные свойства сущности
            WaterReading waterReading = new WaterReading
            {
                UserId = testUserId,
                WaterType = WaterType.Cold,
                CurrentValue = 150,
                PreviousValue = 140,
                ResultValue = 10,
                PaymentAmount = 450.00m
            };

            await DbContext.WaterReadings.AddAsync(waterReading);
            await DbContext.SaveChangesAsync();

            // Очищаем кэш отслеживания, чтобы EF Core делал честный запрос к PostgreSQL
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            // Отправляем DELETE-запрос по ID созданной записи на адрес нового контроллера
            HttpResponseMessage response = await Client.DeleteAsync($"api/water-readings/{waterReading.Id}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.NoContent); // Каноничный статус для DELETE без тела

            // Проверяем отсутствие записи в базе данных PostgreSQL
            DbContext.ChangeTracker.Clear();
            WaterReading? waterReadingInDb = await DbContext.WaterReadings
                .IgnoreQueryFilters() // Отключаем фильтры, чтобы убедиться, что записи нет совсем (а не она просто скрыта)
                .FirstOrDefaultAsync(r => r.Id == waterReading.Id);

            // Главное утверждение: запись должна полностью исчезнуть
            waterReadingInDb.Should().BeNull();
        }
    }
}
