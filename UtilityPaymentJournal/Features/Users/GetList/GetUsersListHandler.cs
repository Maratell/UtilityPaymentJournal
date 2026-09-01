using MediatR;

namespace UtilityPaymentJournal.Features.Users.GetList
{
    public partial class GetUsersListHandler(
            IUserQueryService userQueryService,
            ILogger<GetUsersListHandler> logger) : IRequestHandler<GetUsersListQuery, GetUsersListResponse>
    {
        public async Task<GetUsersListResponse> Handle(GetUsersListQuery query, CancellationToken cancellationToken)
        {
            LogFetchingAllUsers(logger);

            // Пользуемся сервисом, чтобы получить список пользователей
            IReadOnlyCollection<GetUsersListResponse.Item> usersQuery = await userQueryService.GetUsersListWithRolesAsync(cancellationToken);

            LogAllUsersSuccessfullyFetchedFromDb(logger, usersQuery.Count);

            // Возвращаем единый объект ответа с вложенным списком пользователей
            return new GetUsersListResponse(usersQuery);
        }
    }
}
