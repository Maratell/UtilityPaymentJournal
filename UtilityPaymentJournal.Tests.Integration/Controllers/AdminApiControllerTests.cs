using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Features.Users.Create;
using UtilityPaymentJournal.Features.Users.GetById;
using UtilityPaymentJournal.Features.Users.GetList;
using UtilityPaymentJournal.Tests.Integration.Infrastructure;

namespace UtilityPaymentJournal.Tests.Integration.Controllers
{
    [Collection(nameof(IntegrationTestCollection))]
    
    public class AdminApiControllerTests : BaseIntegrationTest
    {
        public AdminApiControllerTests(IntegrationTestWebAppFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Проверяет, что создание пользователя через POST api/admin/users возвращает 201 Created,
        /// заполненный заголовок Location и корректную DTO-модель CreateUserResponse.
        /// </summary>
        [Fact]
        public async Task CreateUserWithRole_Should_ReturnCreated_And_CorrectLocationHeader()
        {
            // ==========================================
            // ARRANGE (Подготовка данных)
            // ==========================================
            string testUserName = $"user_{Guid.NewGuid().ToString("N")[..6]}";
            string testPassword = "AdminTestPassword123!";

            var createUserRequest = new CreateUserRequest(
                testUserName,
                "AdminTestFirst",
                "AdminTestLast",
                testPassword,
                UserRole.User
            );

            // ==========================================
            // ACT (Выполнение запроса)
            // ==========================================
            HttpResponseMessage response = await Client.PostAsJsonAsync("api/admin/users", createUserRequest);

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Проверяем заголовок Location (должен вести на api/admin/users/{id})
            response.Headers.Location.Should().NotBeNull();
            response.Headers.Location!.ToString().Should().Contain($"api/admin/users/");

            // Десериализуем согласно точной структуре CreateUserResponse
            CreateUserResponse? responseContent = await response.Content.ReadFromJsonAsync<CreateUserResponse>();
            responseContent.Should().NotBeNull();
            responseContent!.Id.Should().NotBeNullOrEmpty();
            responseContent.UserName.Should().Be(testUserName);
            responseContent.FirstName.Should().Be("AdminTestFirst");
            responseContent.LastName.Should().Be("AdminTestLast");

            // Проверяем физическое наличие записи в базе данных
            DbContext.ChangeTracker.Clear();
            var userInDb = await DbContext.Users.FindAsync(responseContent.Id);
            userInDb.Should().NotBeNull();
            userInDb!.UserName.Should().Be(testUserName);
        }

        /// <summary>
        /// Проверяет, что метод GET api/admin/users возвращает статус 200 OK 
        /// и обертку GetUsersListResponse, где в коллекции Items присутствует созданный пользователь.
        /// </summary>
        [Fact]
        public async Task GetAll_Should_ReturnOk_With_UsersList()
        {
            // ==========================================
            // ARRANGE (Создаем тестового пользователя через API)
            // ==========================================
            string testUserName = $"user_{Guid.NewGuid().ToString("N")[..6]}";
            var createUserRequest = new CreateUserRequest(testUserName, "Ivan", "Ivanov", "Pass123!", UserRole.User);

            HttpResponseMessage createResponse = await Client.PostAsJsonAsync("api/admin/users", createUserRequest);
            createResponse.IsSuccessStatusCode.Should().BeTrue();

            CreateUserResponse? createdUser = await createResponse.Content.ReadFromJsonAsync<CreateUserResponse>();
            createdUser.Should().NotBeNull();

            // ==========================================
            // ACT (Выполнение запроса на получение списка)
            // ==========================================
            HttpResponseMessage response = await Client.GetAsync("api/admin/users");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем согласно точной структуре GetUsersListResponse
            GetUsersListResponse? listResponse = await response.Content.ReadFromJsonAsync<GetUsersListResponse>();
            listResponse.Should().NotBeNull();

            // Проверяем обращение к свойству Items
            listResponse!.Items.Should().NotBeNullOrEmpty();
            listResponse.Items.Should().Contain(u => u.Id == createdUser!.Id && u.UserName == testUserName);
        }

        /// <summary>
        /// Проверяет, что метод GET api/admin/users/{id} возвращает статус 200 OK 
        /// и детальную модель GetUserByIdResponse для конкретного пользователя.
        /// </summary>
        [Fact]
        public async Task GetById_Should_ReturnOk_With_DetailedUserInfo()
        {
            // ==========================================
            // ARRANGE (Создаем тестового пользователя)
            // ==========================================
            string testUserName = $"user_{Guid.NewGuid().ToString("N")[..6]}";
            var createUserRequest = new CreateUserRequest(testUserName, "Petr", "Petrov", "Pass123!", UserRole.User);

            HttpResponseMessage createResponse = await Client.PostAsJsonAsync("api/admin/users", createUserRequest);
            CreateUserResponse? createdUser = await createResponse.Content.ReadFromJsonAsync<CreateUserResponse>();
            createdUser.Should().NotBeNull();

            // ==========================================
            // ACT (Выполнение запроса по конкретному ID)
            // ==========================================
            HttpResponseMessage response = await Client.GetAsync($"api/admin/users/{createdUser!.Id}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Десериализуем согласно точной структуре GetUserByIdResponse
            GetUserByIdResponse? userDetails = await response.Content.ReadFromJsonAsync<GetUserByIdResponse>();
            userDetails.Should().NotBeNull();
            userDetails!.Id.Should().Be(createdUser.Id);
            userDetails.UserName.Should().Be(testUserName);
            userDetails.FirstName.Should().Be("Petr");
            userDetails.LastName.Should().Be("Petrov");
        }

        /// <summary>
        /// Проверяет, что DELETE api/admin/users/{id} возвращает 204 NoContent,
        /// а запись о пользователе полностью удаляется из базы данных.
        /// </summary>
        [Fact]
        public async Task Delete_Should_ReturnNoContent_And_RemoveUserFromDb()
        {
            // ==========================================
            // ARRANGE (Создаем пользователя)
            // ==========================================
            string testUserName = $"user_{Guid.NewGuid().ToString("N")[..6]}";
            var createUserRequest = new CreateUserRequest(testUserName, "Deleted", "User", "Pass123!", UserRole.User);

            HttpResponseMessage createResponse = await Client.PostAsJsonAsync("api/admin/users", createUserRequest);
            CreateUserResponse? createdUser = await createResponse.Content.ReadFromJsonAsync<CreateUserResponse>();
            createdUser.Should().NotBeNull();

            // Убеждаемся, что перед удалением сущность физически существует в БД
            DbContext.ChangeTracker.Clear();
            var userBeforeDelete = await DbContext.Users.FindAsync(createdUser!.Id);
            userBeforeDelete.Should().NotBeNull();

            // ==========================================
            // ACT (Выполнение запроса на удаление)
            // ==========================================
            HttpResponseMessage response = await Client.DeleteAsync($"api/admin/users/{createdUser.Id}");

            // ==========================================
            // ASSERT (Проверка результатов)
            // ==========================================
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // Тело ответа при 204 должно быть пустым
            string content = await response.Content.ReadAsStringAsync();
            content.Should().BeEmpty();

            // Сбрасываем трекер и проверяем, что пользователя действительно больше нет в базе данных
            DbContext.ChangeTracker.Clear();
            var userAfterDelete = await DbContext.Users.FindAsync(createdUser.Id);
            userAfterDelete.Should().BeNull();
        }
    }
}
