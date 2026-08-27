using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Http.Json;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Features.ComplaintBoard.ChangeStatus;
using UtilityPaymentJournal.Features.ComplaintBoard.Create;
using UtilityPaymentJournal.Features.ComplaintBoard.Edit;
using UtilityPaymentJournal.Features.ComplaintBoard.GetById;
using UtilityPaymentJournal.Features.ComplaintBoard.GetList;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;
using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Utilities;
using UtilityPaymentJournal.Tests.Integration.Infrastructure;

namespace UtilityPaymentJournal.Tests.Integration.Controllers
{
    [Collection(nameof(IntegrationTestCollection))]
    public class ComplaintBoardApiControllerTests : BaseIntegrationTest
    {
        public ComplaintBoardApiControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Проверяет, что при отправке валидных данных контроллер успешно создает запись жалобы, 
        /// сохраняет её в PostgreSQL, автоматически привязывает UserId автора и возвращает статус 201 Created.
        /// </summary>
        [Fact]
        public async Task Create_Should_SaveComplaintInDatabase_And_ReturnCreatedStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных и окружения)
            // ==========================================

            // 1. Идентификатор пользователя, зашитый в наш TestAuthHandler
            const string testUserId = "test-admin-id-123";

            // 2. Генерируем уникальный заголовок для предотвращения конфликтов уникальности в БД
            string uniqueComplaintTitle = $"Тестовая жалоба {Guid.NewGuid().ToString("N")[..6]}";

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

            // Гарантируем наличие услуги (Utility) в базе, так как UtilityId обязателен!
            Utility? utility = await DbContext.Utilities.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (utility == null)
            {
                utility = new Utility
                {
                    Name = "Тестовая коммунальная услуга"
                };
                await DbContext.Utilities.AddAsync(utility);
                await DbContext.SaveChangesAsync();
                DbContext.ChangeTracker.Clear();
            }

            // 4. Формируем тело запроса (DTO) с реальным UtilityId вместо null
            CreateComplaintRequest request = new CreateComplaintRequest(
                Title: uniqueComplaintTitle,
                Description: "Описание тестовой проблемы с коммунальными услугами",
                UtilityId: utility.Id, // Передаем ID созданной услуги для прохождения валидации
                SubmissionDate: DateTime.UtcNow,
                IssueResolutionDate: null,
                Status: ComplaintStatus.New
            );

            // ==========================================
            // ACT (Выполнение целевого действия)
            // ==========================================

            // Отправляем запрос на создание. Метод расширения сам сериализует DTO в JSON.
            HttpResponseMessage response = await Client.PostAsJsonAsync("api/complaint-board", request);

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
            CreateComplaintResponse? responseContent = await response.Content.ReadFromJsonAsync<CreateComplaintResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().BeGreaterThan(0);
            responseContent.Title.Should().Be(uniqueComplaintTitle);

            DbContext.ChangeTracker.Clear();

            Complaint? complaintInDb = await DbContext.Complaints
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == responseContent.Id);

            complaintInDb.Should().NotBeNull();
            complaintInDb!.Title.Should().Be(uniqueComplaintTitle);
            complaintInDb.Description.Should().Be(request.Description);
            complaintInDb.UtilityId.Should().Be(utility.Id); // Дополнительно проверяем привязку услуги в БД

            complaintInDb.UserId.Should().Be(testUserId);
        }

        /// <summary>
        /// Проверяет, что существующая в базе данных запись жалобы успешно возвращается по её идентификатору (ID)
        /// со статусом 200 OK, при условии, что запись принадлежит текущему авторизованному пользователю.
        /// </summary>
        [Fact]
        public async Task GetById_Should_ReturnComplaint_When_ItExistsInDatabase()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================

            // 1. Указываем ID пользователя из нашего TestAuthHandler
            const string testUserId = "test-admin-id-123";

            // 2. Генерируем уникальный заголовок в едином стиле для исключения конфликтов уникальности
            string uniqueComplaintTitle = $"Тестовая жалоба {Guid.NewGuid().ToString("N")[..6]}";

            // 3. Гарантируем, что пользователь существует в Docker-базе для соблюдения Foreign Key
            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = testUserId, UserName = "testadmin", FirstName = "Тест", LastName = "Админ" });
                await DbContext.SaveChangesAsync();
                DbContext.ChangeTracker.Clear();
            }

            // 3.1. Гарантируем наличие услуги (Utility) в базе, так как UtilityId обязателен!
            // Пытаемся взять любую существующую или создаем новую, если база пуста
            Utility? utility = await DbContext.Utilities.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (utility == null)
            {
                utility = new Utility
                {
                    Name = "Тестовая коммунальная услуга"
                };
                await DbContext.Utilities.AddAsync(utility);
                await DbContext.SaveChangesAsync();
                DbContext.ChangeTracker.Clear();
            }

            // 4. Физически создаем жалобу и ЯВНО привязываем её к нашему тест-пользователю и услуге.
            // Это критически важно: если не прописать UserId, глобальный фильтр (Query Filter) 
            // на стороне веб-сервера просто скроет эту запись, и API вернет 404 Not Found!
            Complaint complaint = new Complaint
            {
                Title = uniqueComplaintTitle,
                Description = "Детальное описание проблемы для теста получения по ID",
                UtilityId = utility.Id, // Привязываем обязательный внешний ключ
                CreatedAt = DateTime.UtcNow,
                SubmissionDate = DateTime.UtcNow,
                IssueResolutionDate = null,
                Status = ComplaintStatus.New,
                UserId = testUserId // Жестко связываем запись с текущим авторизованным контекстом
            };

            await DbContext.Complaints.AddAsync(complaint);
            await DbContext.SaveChangesAsync();

            // Сбрасываем кэш EF Core, чтобы тестовый сервер делал честный запрос к дисковой СУБД, а не к памяти
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Отправляем GET-запрос по сгенерированному базой ID
            HttpResponseMessage response = await Client.GetAsync($"api/complaint-board/{complaint.Id}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный статус 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем ответ с использованием вашего DTO GetComplaintByIdResponse
            GetComplaintByIdResponse? content = await response.Content.ReadFromJsonAsync<GetComplaintByIdResponse>();

            content.Should().NotBeNull();
            content!.Id.Should().Be(complaint.Id);
            content.Title.Should().Be(uniqueComplaintTitle);
            content.Description.Should().Be(complaint.Description);
            content.Status.Should().Be(complaint.Status);
            content.UtilityId.Should().Be(utility.Id);
        }

        /// <summary>
        /// Проверяет, что при попытке получить жалобу по идентификатору, которого гарантированно 
        /// нет в базе данных, контроллер корректно обрабатывает ситуацию и возвращает статус 404 Not Found.
        /// </summary>
        [Fact]
        public async Task GetById_Should_ReturnNotFound_When_ComplaintDoesNotExist()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================

            // Поскольку перед каждым тестом Respawn полностью очищает все таблицы,
            // идентификатор '1' гарантированно отсутствует в базе данных PostgreSQL.
            // Это гораздо надежнее, чем зашивать случайные числа вроде 99999.
            const long nonExistentComplaintId = 1;

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================

            // Пытаемся получить несуществующий объект через HttpClient
            HttpResponseMessage response = await Client.GetAsync($"api/complaint-board/{nonExistentComplaintId}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================

            // Проверяем каноничный статус 404 Not Found, который должен вернуть контроллер
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Проверяет, что метод получения списка успешно возвращает коллекцию жалоб со статусом 200 OK,
        /// причем в список попадают только те объекты, которые принадлежат текущему авторизованному пользователю.
        /// </summary>
        [Fact]
        public async Task GetAll_Should_ReturnComplaintsList_OwnedByCurrentUser()
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

            // Гарантируем наличие коммунальной услуги (Utility) в базе, так как UtilityId обязателен!
            Utility? utility = await DbContext.Utilities.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (utility == null)
            {
                utility = new Utility { Name = "Тестовая коммунальная услуга" };
                await DbContext.Utilities.AddAsync(utility);
                await DbContext.SaveChangesAsync();
            }

            string myComplaintTitle = $"Тестовая жалоба моя {Guid.NewGuid().ToString("N")[..6]}";
            string otherComplaintTitle = $"Тестовая жалоба чужая {Guid.NewGuid().ToString("N")[..6]}";

            // Создаем две жалобы: одну привязываем к себе, вторую — к чужому аккаунту
            Complaint myComplaint = new Complaint
            {
                Title = myComplaintTitle,
                Description = "Моя проблема",
                UtilityId = utility.Id,
                UserId = currentUserId,
                Status = ComplaintStatus.New,
                CreatedAt = DateTime.UtcNow
            };
            Complaint otherComplaint = new Complaint
            {
                Title = otherComplaintTitle,
                Description = "Чужая проблема",
                UtilityId = utility.Id,
                UserId = otherUserId,
                Status = ComplaintStatus.New,
                CreatedAt = DateTime.UtcNow
            };

            await DbContext.Complaints.AddRangeAsync(myComplaint, otherComplaint);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            HttpResponseMessage response = await Client.GetAsync("api/complaint-board");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            GetComplaintsListResponse? content = await response.Content.ReadFromJsonAsync<GetComplaintsListResponse>();
            content.Should().NotBeNull();
            content!.Items.Should().NotBeNull();

            // Проверяем, что глобальный фильтр на стороне сервера отдал НАШУ запись, но скрыл ЧУЖУЮ
            content.Items.Should().ContainSingle(r => r.Title == myComplaintTitle);
            content.Items.Should().NotContain(r => r.Title == otherComplaintTitle);
        }

        /// <summary>
        /// Проверяет, что при отправке валидных данных для редактирования, контроллер успешно обновляет 
        /// данные жалобы в PostgreSQL и возвращает статус 200 OK с обновленными данными.
        /// </summary>
        [Fact]
        public async Task Edit_Should_UpdateComplaintInDatabase_And_ReturnOkStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            const string testUserId = "test-admin-id-123";
            string initialComplaintTitle = $"Старая жалоба {Guid.NewGuid().ToString("N")[..6]}";
            string updatedComplaintTitle = $"Новая жалоба {Guid.NewGuid().ToString("N")[..6]}";

            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = testUserId, UserName = "testadmin", FirstName = "Тест", LastName = "Админ" });
            }

            // Гарантируем наличие коммунальной услуги (Utility) в базе, так как UtilityId обязателен!
            Utility? utility = await DbContext.Utilities.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (utility == null)
            {
                utility = new Utility { Name = "Тестовая коммунальная услуга" };
                await DbContext.Utilities.AddAsync(utility);
            }
            await DbContext.SaveChangesAsync();

            // Создаем исходную запись жалобы в БД
            Complaint complaint = new Complaint
            {
                Title = initialComplaintTitle,
                Description = "Исходное описание проблемы",
                UtilityId = utility.Id,
                UserId = testUserId,
                Status = ComplaintStatus.New,
                SubmissionDate = DateTime.UtcNow.AddDays(-1),
                IssueResolutionDate = null,
                CreatedAt = DateTime.UtcNow
            };
            await DbContext.Complaints.AddAsync(complaint);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // Задаем тестовые временные метки для обновления
            DateTime testSubmissionDate = DateTime.UtcNow;
            DateTime testResolutionDate = DateTime.UtcNow.AddDays(2);

            // Формируем DTO запроса на редактирование (без ID, так как ID передается в URL)
            EditComplaintRequest request = new EditComplaintRequest(
                Title: updatedComplaintTitle,
                Description: "Обновленное описание проблемы",
                UtilityId: utility.Id,
                SubmissionDate: testSubmissionDate,
                IssueResolutionDate: testResolutionDate,
                Status: ComplaintStatus.InProgress
            );

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            HttpResponseMessage response = await Client.PutAsJsonAsync($"api/complaint-board/{complaint.Id}", request);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            EditComplaintResponse? responseContent = await response.Content.ReadFromJsonAsync<EditComplaintResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().Be(complaint.Id);
            responseContent.Title.Should().Be(updatedComplaintTitle);

            // Проверяем физическое изменение данных прямо в базе PostgreSQL
            DbContext.ChangeTracker.Clear();
            Complaint? complaintInDb = await DbContext.Complaints
                .IgnoreQueryFilters() // Используем страховку поиска
                .FirstOrDefaultAsync(c => c.Id == complaint.Id);

            complaintInDb.Should().NotBeNull();
            complaintInDb!.Title.Should().Be(updatedComplaintTitle); // Доказываем, что заголовок перезаписался
            complaintInDb.Description.Should().Be(request.Description); // Доказываем, что описание изменилось
            complaintInDb.Status.Should().Be(ComplaintStatus.InProgress); // Доказываем, что статус обновился
            complaintInDb.SubmissionDate.Should().BeCloseTo(testSubmissionDate, TimeSpan.FromSeconds(1)); // Проверяем даты
            complaintInDb.IssueResolutionDate.Should().BeCloseTo(testResolutionDate, TimeSpan.FromSeconds(1));
            complaintInDb.UserId.Should().Be(testUserId); // Проверяем, что владелец не изменился
        }

        /// <summary>
        /// Проверяет, что метод удаления успешно удаляет запись жалобы из базы данных 
        /// и возвращает каноничный REST-статус 204 No Content без тела ответа.
        /// </summary>
        [Fact]
        public async Task Delete_Should_RemoveComplaintFromDatabase_And_ReturnNoContentStatus()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            const string testUserId = "test-admin-id-123";
            string complaintTitleToDelete = $"Жалоба для удаления {Guid.NewGuid().ToString("N")[..6]}";

            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = testUserId, UserName = "testadmin", FirstName = "Тест", LastName = "Админ" });
            }

            // Гарантируем наличие коммунальной услуги (Utility) в базе, так как UtilityId обязателен!
            Utility utility = await DbContext.Utilities.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (utility == null)
            {
                utility = new Utility { Name = "Тестовая коммунальная услуга" };
                await DbContext.Utilities.AddAsync(utility);
            }
            await DbContext.SaveChangesAsync();

            // Создаем запись жалобы, которую будем удалять
            Complaint complaint = new Complaint
            {
                Title = complaintTitleToDelete,
                Description = "Эта жалоба должна быть полностью удалена в ходе теста",
                UtilityId = utility.Id,
                UserId = testUserId,
                Status = ComplaintStatus.New,
                CreatedAt = DateTime.UtcNow
            };
            await DbContext.Complaints.AddAsync(complaint);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            HttpResponseMessage response = await Client.DeleteAsync($"api/complaint-board/{complaint.Id}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.NoContent); // Каноничный статус для DELETE без тела

            // Проверяем отсутствие записи в базе данных PostgreSQL
            DbContext.ChangeTracker.Clear();
            Complaint? complaintInDb = await DbContext.Complaints
                .IgnoreQueryFilters() // Отключаем фильтры, чтобы убедиться, что записи нет совсем (а не она просто скрыта)
                .FirstOrDefaultAsync(c => c.Id == complaint.Id);

            complaintInDb.Should().BeNull(); // Запись должна полностью исчезнуть
        }

        /// <summary>
        /// Проверяет, что при отправке валидного запроса на изменение статуса, метод PATCH успешно 
        /// обновляет поле Status в PostgreSQL и возвращает 200 OK с полными деталями карточки жалобы.
        /// </summary>
        [Fact]
        public async Task ChangeStatus_Should_UpdateStatusInDatabase_And_ReturnOkWithFullDetails()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            const string testUserId = "test-admin-id-123";
            string uniqueTitle = $"Жалоба для смены статуса {Guid.NewGuid().ToString("N")[..6]}";

            if (!await DbContext.Users.IgnoreQueryFilters().AnyAsync(u => u.Id == testUserId))
            {
                await DbContext.Users.AddAsync(new User { Id = testUserId, UserName = "testadmin", FirstName = "Тест", LastName = "Админ" });
            }

            // Гарантируем наличие коммунальной услуги (Utility) в базе, так как UtilityId обязателен!
            Utility? utility = await DbContext.Utilities.IgnoreQueryFilters().FirstOrDefaultAsync();
            if (utility == null)
            {
                utility = new Utility { Name = "Тестовая коммунальная услуга" };
                await DbContext.Utilities.AddAsync(utility);
            }
            await DbContext.SaveChangesAsync();

            // Создаем запись жалобы со стартовым статусом New
            Complaint complaint = new Complaint
            {
                Title = uniqueTitle,
                Description = "Тестовое описание проблемы для смены статуса",
                UtilityId = utility.Id,
                UserId = testUserId,
                Status = ComplaintStatus.New,
                SubmissionDate = DateTime.UtcNow,
                IssueResolutionDate = null,
                CreatedAt = DateTime.UtcNow
            };
            await DbContext.Complaints.AddAsync(complaint);
            await DbContext.SaveChangesAsync();
            DbContext.ChangeTracker.Clear();

            // Формируем DTO запроса на изменение статуса
            ChangeComplaintStatusRequest request = new ChangeComplaintStatusRequest(ComplaintStatus.InProgress);

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            HttpResponseMessage response = await Client.PatchAsJsonAsync($"api/complaint-board/{complaint.Id}/change-status", request);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем и проверяем полную структуру ответа ChangeComplaintStatusResponse
            ChangeComplaintStatusResponse? responseContent = await response.Content.ReadFromJsonAsync<ChangeComplaintStatusResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().Be(complaint.Id);
            responseContent.Title.Should().Be(uniqueTitle);
            responseContent.Description.Should().Be(complaint.Description);
            responseContent.UtilityId.Should().Be(utility.Id);
            responseContent.Status.Should().Be(ComplaintStatus.InProgress); // Статус должен обновиться

            // Проверяем физическое изменение статуса напрямую в базе PostgreSQL
            DbContext.ChangeTracker.Clear();
            Complaint? complaintInDb = await DbContext.Complaints
                .IgnoreQueryFilters() // Используем страховку поиска
                .FirstOrDefaultAsync(c => c.Id == complaint.Id);

            complaintInDb.Should().NotBeNull();
            complaintInDb!.Status.Should().Be(ComplaintStatus.InProgress); // Доказываем фиксацию в СУБД
            complaintInDb.Title.Should().Be(uniqueTitle); // Убеждаемся, что остальные критические поля не затерлись
            complaintInDb.UserId.Should().Be(testUserId);
        }

        /// <summary>
        /// Проверяет, что при попытке изменить статус у карточки жалобы, которой гарантированно 
        /// нет в базе данных, контроллер корректно обрабатывает ситуацию и возвращает статус 404 Not Found.
        /// </summary>
        [Fact]
        public async Task ChangeStatus_Should_ReturnNotFound_When_ComplaintDoesNotExist()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================

            // Благодаря Respawn идентификатор '1' гарантированно свободен
            const long nonExistentComplaintId = 1;
            ChangeComplaintStatusRequest request = new ChangeComplaintStatusRequest(ComplaintStatus.Resolved);

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            HttpResponseMessage response = await Client.PatchAsJsonAsync($"api/complaint-board/{nonExistentComplaintId}/change-status", request);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
