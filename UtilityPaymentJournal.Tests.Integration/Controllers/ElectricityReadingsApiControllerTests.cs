using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using UtilityPaymentJournal.Features.ElectricityReadings.Create;
using UtilityPaymentJournal.Features.ElectricityReadings.Edit;
using UtilityPaymentJournal.Features.ElectricityReadings.GetById;
using UtilityPaymentJournal.Features.ElectricityReadings.GetList;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;
using UtilityPaymentJournal.Tests.Integration.Infrastructure;

namespace UtilityPaymentJournal.Tests.Integration.Controllers
{
    [Collection(nameof(IntegrationTestCollection))]
    public class ElectricityReadingsApiControllerTests : BaseIntegrationTest
    {
        public ElectricityReadingsApiControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Проверяет, что при отправке валидных данных контроллер успешно создает запись, 
        /// сохраняет её в PostgreSQL, автоматически привязывает UserId автора и возвращает статус 201 Created.
        /// </summary>
        [Fact]
        public async Task Create_Should_SaveElectricityReadingInDatabase_And_ReturnCreatedStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных и окружения)
            // ==========================================

            // 1. Идентификатор пользователя, зашитый в наш TestAuthHandler
            const string testUserId = "test-admin-id-123";

            // 2. Гарантируем наличие пользователя в Docker-базе, чтобы не нарушить Foreign Key (UserId) в PostgreSQL
            // IgnoreQueryFilters() защищает от скрытых глобальных фильтров при первичной проверке базы
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

            // 3. Создаем тестового Поставщика услуг (UtilityProvider) для соблюдения Foreign Key в PostgreSQL.
            // Привязываем его к нашему пользователю на случай, если у поставщиков тоже включена фильтрация по UserId.
            UtilityProvider testProvider = new UtilityProvider
            {
                Name = $"Тестовый поставщик {Guid.NewGuid().ToString("N")[..6]}",
                UserId = testUserId
            };
            await DbContext.UtilityProviders.AddAsync(testProvider);

            // 4. Создаем тестовый Жилой объект (Residence) для соблюдения Foreign Key в PostgreSQL.
            // Наличие адреса страхует тест от падения, если поле помечено как [Required] или проверяется валидатором.
            Residence testResidence = new Residence
            {
                Address = $"Тестовый адрес {Guid.NewGuid().ToString("N")[..6]}",
                UserId = testUserId
            };
            await DbContext.Residences.AddAsync(testResidence);

            // Сохраняем все подготовленные зависимости в базу и очищаем Change Tracker
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // 5. Генерируем уникальные тестовые значения для показаний и дат.
            // Текущие показания делаем гарантированно больше предыдущих для прохождения логических валидаторов.
            long testPreviousValue = Random.Shared.Next(100, 500);
            long testCurrentValue = testPreviousValue + Random.Shared.Next(5, 20);
            long testResultValue = testCurrentValue - testPreviousValue;
            decimal testPaymentAmount = testResultValue * 5.50m; // Условный тариф за кВт*ч

            DateTime testSubmissionDate = DateTime.UtcNow.Date;
            DateTime testPaymentDate = DateTime.UtcNow.Date.AddDays(1);

            // 6. Формируем тело запроса (DTO) без поля WaterType, так как это электроэнергия
            CreateElectricityReadingRequest request = new CreateElectricityReadingRequest(
                ResidenceId: testResidence.Id,
                UtilityProviderId: testProvider.Id,
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

            // Отправляем POST-запрос на создание на целевой адрес "api/electricity-readings"
            HttpResponseMessage response = await Client.PostAsJsonAsync("api/electricity-readings", request);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Перехватываем 400 BadRequest и выводим детальную ошибку валидации от FluentValidation/API для удобной отладки
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string errorText = await response.Content.ReadAsStringAsync();
                throw new Xunit.Sdk.XunitException($"[400 BadRequest] Ошибка валидации от API: {errorText}");
            }

            // Проверяем каноничный HTTP-статус ответа 201 Created и наличие заголовка Location (благодаря CreatedAtAction)
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();

            // Быстро десериализуем JSON-ответ прямо через ReadFromJsonAsync
            CreateElectricityReadingResponse? responseContent = await response.Content.ReadFromJsonAsync<CreateElectricityReadingResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().BeGreaterThan(0);

            // Сбрасываем кэш отслеживания (Change Tracker) EF Core перед прямым запросом в СУБД,
            // чтобы получить актуальные данные из PostgreSQL, а не локального кэша
            DbContext.ChangeTracker.Clear();

            // ГЛАВНАЯ ПРОВЕРКА В БАЗЕ ДАННЫХ POSTGRESQL
            // Используем .IgnoreQueryFilters(), так как в тестовом контексте у DbContext нет HTTP-пользователя (UserId равен null)
            ElectricityReading? electricityReadingInDb = await DbContext.ElectricityReadings
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == responseContent.Id);

            // Проверяем, что запись физически создана в Docker-контейнере и все переданные поля маппировались корректно
            electricityReadingInDb.Should().NotBeNull();
            electricityReadingInDb!.ResidenceId.Should().Be(testResidence.Id);
            electricityReadingInDb!.UtilityProviderId.Should().Be(testProvider.Id);
            electricityReadingInDb!.SubmissionDate.Should().Be(testSubmissionDate);
            electricityReadingInDb!.PaymentDate.Should().Be(testPaymentDate);
            electricityReadingInDb!.CurrentValue.Should().Be(testCurrentValue);
            electricityReadingInDb!.PreviousValue.Should().Be(testPreviousValue);
            electricityReadingInDb!.ResultValue.Should().Be(testResultValue);
            electricityReadingInDb!.PaymentAmount.Should().Be(testPaymentAmount);

            // КРИТИЧЕСКИ ВАЖНАЯ ПРОВЕРКА: Доказываем, что логика автоматической привязки владельца (ApplyUserOwnership)
            // сработала корректно и привязала запись именно к нашему текущему пользователю из TestAuthHandler.
            electricityReadingInDb.UserId.Should().Be(testUserId);
        }

        /// <summary>
        /// Проверяет, что существующая в базе данных запись показания счетчиков электроэнергии успешно возвращается по её идентификатору (ID)
        /// со статусом 200 OK, при условии, что запись принадлежит текущему авторизованному пользователю.
        /// </summary>
        [Fact]
        public async Task GetById_Should_ReturnElectricityReading_When_ItExistsInDatabase()
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

            // 3. Создаем тестового Поставщика услуг (UtilityProvider) для проверки подтягивания текстового названия (UtilityProviderName)
            string uniqueProviderName = $"Тестовый поставщик {Guid.NewGuid().ToString("N")[..6]}";
            UtilityProvider testProvider = new UtilityProvider 
            { 
                Name = uniqueProviderName, 
                UserId = testUserId 
            };
            await DbContext.UtilityProviders.AddAsync(testProvider);

            // 4. Создаем тестовый Жилой объект (Residence) для проверки подтягивания текстового адреса (ResidenceAddress)
            string uniqueAddress = $"Тестовый адрес {Guid.NewGuid().ToString("N")[..6]}";
            Residence testResidence = new Residence
            { 
                Address = uniqueAddress,
                UserId = testUserId 
            };
            await DbContext.Residences.AddAsync(testResidence);

            // Сохраняем зависимости, чтобы получить их сгенерированные базой ID
            await DbContext.SaveChangesAsync();

            // 5. Генерируем тестовые значения показаний и дат
            long testPreviousValue = Random.Shared.Next(100, 500);
            long testCurrentValue = testPreviousValue + Random.Shared.Next(5, 20);
            long testResultValue = testCurrentValue - testPreviousValue;
            decimal testPaymentAmount = testResultValue * 5.50m;
            DateTime testSubmissionDate = DateTime.UtcNow.Date;
            DateTime testPaymentDate = DateTime.UtcNow.Date.AddDays(1);

            // 6. Физически создаем показание электроэнергии в БД и ЯВНО привязываем его к нашему тест-пользователю.
            // Если не прописать UserId, глобальный Query Filter на стороне веб-сервера скроет эту запись и API вернет 404!
            ElectricityReading electricityReading = new ElectricityReading
            {
                ResidenceId = testResidence.Id,
                UtilityProviderId = testProvider.Id,
                UserId = testUserId, // Жестко связываем запись с текущим авторизованным контекстом
                CurrentValue = testCurrentValue,
                PreviousValue = testPreviousValue,
                ResultValue = testResultValue,
                PaymentAmount = testPaymentAmount,
                SubmissionDate = testSubmissionDate,
                PaymentDate = testPaymentDate
            };

            await DbContext.ElectricityReadings.AddAsync(electricityReading);
            await DbContext.SaveChangesAsync();

            // Сбрасываем кэш EF Core, чтобы тестовый сервер делал честный запрос к дисковой СУБД, а не к памяти
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Отправляем GET-запрос по сгенерированному базой ID на эндпоинт api/electricity-readings/{id}
            HttpResponseMessage response = await Client.GetAsync($"api/electricity-readings/{electricityReading.Id}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный статус 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем ответ строго по вашему record DTO
            GetElectricityReadingByIdResponse? content = await response.Content.ReadFromJsonAsync<GetElectricityReadingByIdResponse>();

            // Проверяем, что API вернуло именно те данные, которые мы сохранили в базу
            content.Should().NotBeNull();
            content!.Id.Should().Be(electricityReading.Id);
            content.ResidenceId.Should().Be(testResidence.Id);
            content.UtilityProviderId.Should().Be(testProvider.Id);

            // ПРОВЕРКА СВЯЗЕЙ: Убеждаемся, что MediatR/AutoMapper корректно подтянул текстовые поля из соседних таблиц через Include/проекции
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
        /// Проверяет, что при попытке получить показание счетчиков электроэнергии по идентификатору, которого гарантированно 
        /// нет в базе данных, контроллер корректно обрабатывает ситуацию и возвращает статус 404 Not Found.
        /// </summary>
        [Fact]
        public async Task GetById_Should_ReturnNotFound_When_ElectricityReadingDoesNotExist()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================

            // Поскольку перед каждым тестом Respawn полностью очищает все таблицы PostgreSQL,
            // идентификатор '1' гарантированно отсутствует в базе данных. Это гораздо надежнее случайных чисел вроде 9999.
            const int nonExistentReadingId = 1;

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Пытаемся получить несуществующий объект через HttpClient
            HttpResponseMessage response = await Client.GetAsync($"api/electricity-readings/{nonExistentReadingId}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный статус 404 Not Found, который должен вернуть контроллер или Middleware
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Проверяет, что метод получения списка успешно возвращает коллекцию показаний счетчиков электроэнергии со статусом 200 OK,
        /// причем в список попадают только те объекты, которые принадлежат текущему авторизованному пользователю.
        /// </summary>
        [Fact]
        public async Task GetAll_Should_ReturnElectricityReadingsList_OwnedByCurrentUser()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            const string currentUserId = "test-admin-id-123";
            const string otherUserId = "some-other-user-id-999";

            // 1. Гарантируем наличие текущего и стороннего пользователей для проверки изоляции прав в БД
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == currentUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = currentUserId, UserName = "current", FirstName = "Тест", LastName = "Админ" });
            }
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == otherUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = otherUserId, UserName = "other", FirstName = "Чужой", LastName = "Юзер" });
            }
            await DbContext.SaveChangesAsync();

            // 2. Создаем окружение зависимостей для ТЕКУЩЕГО пользователя (Жилье и Поставщик)
            Residence myResidence = new Residence { Address = $"Мой адрес {Guid.NewGuid().ToString("N")[..6]}", UserId = currentUserId };
            UtilityProvider myProvider = new UtilityProvider { Name = $"Мой поставщик {Guid.NewGuid().ToString("N")[..6]}", UserId = currentUserId };
            await DbContext.Residences.AddAsync(myResidence);
            await DbContext.UtilityProviders.AddAsync(myProvider);

            // 3. Создаем окружение зависимостей для ЧУЖОГО пользователя (Жилье и Поставщик) для исключения пересечений бизнес-логики
            Residence otherResidence = new Residence { Address = $"Чужой адрес {Guid.NewGuid().ToString("N")[..6]}", UserId = otherUserId };
            UtilityProvider otherProvider = new UtilityProvider { Name = $"Чужой поставщик {Guid.NewGuid().ToString("N")[..6]}", UserId = otherUserId };
            await DbContext.Residences.AddAsync(otherResidence);
            await DbContext.UtilityProviders.AddAsync(otherProvider);

            await DbContext.SaveChangesAsync();

            // 4. Генерируем уникальные значения для показаний, чтобы однозначно идентифицировать их в ассертах списка (где может не быть UserId)
            long myUniqueValue = Random.Shared.Next(1000, 5000);
            long otherUniqueValue = Random.Shared.Next(6000, 9000);

            // 5. Физически создаем две записи показаний: одну нашу (связанную с текущим UserId), одну чужую (с чужим UserId)
            ElectricityReading myReading = new ElectricityReading
            {
                ResidenceId = myResidence.Id,
                UtilityProviderId = myProvider.Id,
                UserId = currentUserId,
                CurrentValue = myUniqueValue,
                PreviousValue = 100,
                ResultValue = myUniqueValue - 100,
                PaymentAmount = 150.00m
            };

            ElectricityReading otherReading = new ElectricityReading
            {
                ResidenceId = otherResidence.Id,
                UtilityProviderId = otherProvider.Id,
                UserId = otherUserId,
                CurrentValue = otherUniqueValue,
                PreviousValue = 100,
                ResultValue = otherUniqueValue - 100,
                PaymentAmount = 300.00m
            };

            await DbContext.ElectricityReadings.AddRangeAsync(myReading, otherReading);
            await DbContext.SaveChangesAsync();

            // Сбрасываем кэш EF Core перед отправкой HTTP-запроса
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Отправляем GET-запрос на получение списка показаний электроэнергии
            HttpResponseMessage response = await Client.GetAsync("api/electricity-readings");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем коллекцию объектов-оберток списка
            GetElectricityReadingsListResponse? content = await response.Content.ReadFromJsonAsync<GetElectricityReadingsListResponse>();
            content.Should().NotBeNull();
            content!.Items.Should().NotBeNull();

            // ГЛАВНОЕ УТВЕРЖДЕНИЕ: Проверяем, что глобальный Query Filter на стороне сервера отдал НАШУ запись, но скрыл ЧУЖУЮ
            content.Items.Should().ContainSingle(r => r.CurrentValue == myUniqueValue);
            content.Items.Should().NotContain(r => r.CurrentValue == otherUniqueValue);
        }

        /// <summary>
        /// Проверяет, что при отправке валидных данных для редактирования, контроллер успешно обновляет 
        /// показания счетчиков электроэнергии в PostgreSQL и возвращает статус 200 OK с обновленными данными.
        /// </summary>
        [Fact]
        public async Task Edit_Should_UpdateElectricityReadingInDatabase_And_ReturnOkStatus()
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

            // 2. Создаем инфраструктуру внешних ключей (Жилье и Поставщик)
            Residence testResidence = new Residence { Address = $"Тестовый адрес {Guid.NewGuid().ToString("N")[..6]}", UserId = testUserId };
            UtilityProvider testProvider = new UtilityProvider { Name = $"Тестовый поставщик {Guid.NewGuid().ToString("N")[..6]}", UserId = testUserId };
            await DbContext.Residences.AddAsync(testResidence);
            await DbContext.UtilityProviders.AddAsync(testProvider);
            await DbContext.SaveChangesAsync();

            // 3. Физически создаем ИСХОДНУЮ (старую) запись показаний в БД, которую будем редактировать
            ElectricityReading electricityReading = new ElectricityReading
            {
                ResidenceId = testResidence.Id,
                UtilityProviderId = testProvider.Id,
                UserId = testUserId,
                CurrentValue = 200,
                PreviousValue = 180,
                ResultValue = 20,
                PaymentAmount = 110.00m,
                SubmissionDate = DateTime.UtcNow.Date.AddDays(-30),
                PaymentDate = DateTime.UtcNow.Date.AddDays(-28)
            };
            await DbContext.ElectricityReadings.AddAsync(electricityReading);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // 4. Готовим ОБНОВЛЕННЫЕ значения для отправки в запросе (имитируем внесение нового расчетного периода)
            long updatedPreviousValue = 200;
            long updatedCurrentValue = 225;
            long updatedResultValue = updatedCurrentValue - updatedPreviousValue;
            decimal updatedPaymentAmount = updatedResultValue * 6.00m;
            DateTime updatedSubmissionDate = DateTime.UtcNow.Date;
            DateTime updatedPaymentDate = DateTime.UtcNow.Date.AddDays(1);

            // 5. Формируем DTO запроса на редактирование (без ID в теле, так как ID передается в URL маршрута)
            EditElectricityReadingRequest request = new EditElectricityReadingRequest(
                ResidenceId: testResidence.Id,
                UtilityProviderId: testProvider.Id,
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

            // Передаем ID в строке запроса, а обновленный DTO — в теле (BODY) через PUT
            HttpResponseMessage response = await Client.PutAsJsonAsync($"api/electricity-readings/{electricityReading.Id}", request);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Перехватываем 400 BadRequest для удобной отладки при несовпадении логических проверок хендлера
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                string errorText = await response.Content.ReadAsStringAsync();
                throw new Xunit.Sdk.XunitException($"[400 BadRequest] Ошибка валидации: {errorText}");
            }

            // Проверяем каноничный статус 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем и проверяем контент ответа API (маппинг возвращаемого DTO)
            EditElectricityReadingResponse? responseContent = await response.Content.ReadFromJsonAsync<EditElectricityReadingResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().Be(electricityReading.Id);
            responseContent.CurrentValue.Should().Be(updatedCurrentValue);
            responseContent.PaymentAmount.Should().Be(updatedPaymentAmount);

            // 6. ГЛАВНАЯ ПРОВЕРКА: Извлечение записи напрямую из СУБД PostgreSQL в обход локального кэша EF
            DbContext.ChangeTracker.Clear();
            ElectricityReading? readingInDb = await DbContext.ElectricityReadings
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == electricityReading.Id);

            // Доказываем, что все ключевые поля физически перезаписались новыми значениями на уровне диска СУБД
            readingInDb.Should().NotBeNull();
            readingInDb!.CurrentValue.Should().Be(updatedCurrentValue);
            readingInDb!.PreviousValue.Should().Be(updatedPreviousValue);
            readingInDb!.ResultValue.Should().Be(updatedResultValue);
            readingInDb!.PaymentAmount.Should().Be(updatedPaymentAmount);
            readingInDb!.SubmissionDate.Should().Be(updatedSubmissionDate);
            readingInDb!.PaymentDate.Should().Be(updatedPaymentDate);

            // Доказываем, что критически важный владелец записи (UserId) остался прежним и не стерся при обновлении
            readingInDb.UserId.Should().Be(testUserId);
        }

        /// <summary>
        /// Проверяет, что метод удаления успешно удаляет запись показания счетчиков электроэнергии из базы данных 
        /// и возвращает каноничный REST-статус 204 No Content без тела ответа.
        /// </summary>
        [Fact]
        public async Task Delete_Should_RemoveElectricityReadingFromDatabase_And_ReturnNoContentStatus()
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

            // 2. Создаем запись показаний в БД только с обязательными свойствами для легкости и скорости теста
            ElectricityReading electricityReading = new ElectricityReading
            {
                UserId = testUserId,
                CurrentValue = 300,
                PreviousValue = 280,
                ResultValue = 20,
                PaymentAmount = 110.00m
            };

            await DbContext.ElectricityReadings.AddAsync(electricityReading);
            await DbContext.SaveChangesAsync();

            // Очищаем кэш отслеживания, чтобы EF Core делал честный SQL-запрос к PostgreSQL при проверке удаления
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Отправляем DELETE-запрос по ID созданной записи на целевой адрес контроллера
            HttpResponseMessage response = await Client.DeleteAsync($"api/electricity-readings/{electricityReading.Id}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Ожидаем каноничный статус для успешного удаления без тела ответа — 204 No Content
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Проверяем физическое отсутствие записи в базе данных PostgreSQL
            DbContext.ChangeTracker.Clear();
            ElectricityReading? readingInDb = await DbContext.ElectricityReadings
                .IgnoreQueryFilters() // Отключаем фильтры, чтобы убедиться, что записи нет совсем (а не она просто скрыта фильтром по юзеру)
                .FirstOrDefaultAsync(r => r.Id == electricityReading.Id);

            // Главное утверждение: объект должен полностью отсутствовать в базе данных
            readingInDb.Should().BeNull();
        }
    }
}
