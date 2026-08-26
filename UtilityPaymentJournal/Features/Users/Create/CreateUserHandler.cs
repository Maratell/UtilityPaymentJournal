using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using UtilityPaymentJournal.Common.Exceptions;
using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Features.Users.Create
{
    public partial class CreateUserHandler(
        ApplicationDbContext context,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        ILogger<CreateUserHandler> logger) : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        public async Task<CreateUserResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            LogUserCreationRequested(logger, command.UserName);
            // Менеджеры Identity сразу сохраняют каждое действие в БД через SaveChanges.
            // Чтобы при ошибке привязки роли в базе не оставался "битый" пользователь без роли,
            // мы объединяем все три шага в одну неделимую транзакцию: или запишется всё, или ничего.
            using (IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(cancellationToken))
            {
                // try нужен для того, чтобы перехватить любой сбой прямо в процессе выполнения шагов.
                // Без этого блока мы не смогли бы вызвать асинхронный откат (RollbackAsync), и серверу 
                // пришлось бы блокировать свои потоки, выполняя отмену синхронно внутри механизма using.
                try
                {
                    // Шаг 1: Создаем пользователя в таблице AspNetUsers
                    User user = command.ToEntity();

                    IdentityResult createdUser = await userManager.CreateAsync(user, command.Password);
                    if (!createdUser.Succeeded)
                    {
                        // Вместо InvalidOperationException и склейки строк выбрасываем кастомное исключение валидации.
                        // IdentityValidationExceptionHandler перехватит его и вернет красивый 400 BadRequest.
                        throw new IdentityValidationException(createdUser.Errors.Select(e => e.Description));
                    }

                    // Шаг 2: Гарантируем наличие роли в таблице AspNetRoles
                    string roleName = command.Role.GetDisplayName();
                    LogCheckingRoleExistence(logger, roleName);
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        IdentityResult roleResult = await roleManager.CreateAsync(new Role(roleName));
                        if (!roleResult.Succeeded)
                        {
                            LogIdentityRoleCreateFailed(logger, roleName);
                            throw new IdentityValidationException(roleResult.Errors.Select(e => e.Description));
                        }
                    }

                    // Шаг 3: Привязываем пользователя к роли в связующей таблице AspNetUserRoles
                    LogAssigningRoleToUser(logger, command.UserName, roleName);
                    IdentityResult addRoleResult = await userManager.AddToRoleAsync(user, roleName);
                    if (!addRoleResult.Succeeded)
                    {
                        LogRoleAssignmentFailed(logger, command.UserName, roleName);
                        throw new IdentityValidationException(addRoleResult.Errors.Select(e => e.Description));
                    }

                    // Если все шаги успешны — подтверждаем изменения и сохраняем их на диск
                    await transaction.CommitAsync(cancellationToken);
                    LogUserCreationTransactionCommitted(logger, command.UserName);

                    return user.ToResponse(roleName);
                }
                catch
                {
                    // При любой ошибке (включая наше исключение валидации) полностью стираем временные изменения из БД
                    if (context.Database.CurrentTransaction != null)
                    {
                        LogRollingBackUserCreationTransaction(logger, command.UserName);
                        await transaction.RollbackAsync(cancellationToken);
                    }
                    throw; // Передаем исключение дальше. Исключение валидации уйдет в свой Handler, остальные — в системные
                }
            }
        }
    }
}
