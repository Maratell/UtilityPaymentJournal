using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using UtilityPaymentJournal.Common.Exceptions;
using UtilityPaymentJournal.Common.Extensions;
using UtilityPaymentJournal.Infrastructure.EF.Context;
using UtilityPaymentJournal.Infrastructure.EF.Entity.Authentication;

namespace UtilityPaymentJournal.Features.Users.Commands
{
    public partial class UserCommandService(
    UserManager<User> userManager,
    RoleManager<Role> roleManager,
    ApplicationDbContext dbContext,
    IUserMapper userMapper,
    ILogger<UserCommandService> logger) : IUserCommandService
    {
        private readonly UserManager<User> _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        private readonly RoleManager<Role> _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        private readonly ApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        private readonly IUserMapper _userMapper = userMapper ?? throw new ArgumentNullException(nameof(userMapper));
        private readonly ILogger<UserCommandService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));


        public async Task<UserCommandResultDto> CreateAsync(CreateUserDto createDto, CancellationToken cancellationToken = default)
        {
            LogUserCreationRequested(_logger, createDto.UserName);
            // Менеджеры Identity сразу сохраняют каждое действие в БД через SaveChanges.
            // Чтобы при ошибке привязки роли в базе не оставался "битый" пользователь без роли,
            // мы объединяем все три шага в одну неделимую транзакцию: или запишется всё, или ничего.
            using (IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                // try нужен для того, чтобы перехватить любой сбой прямо в процессе выполнения шагов.
                // Без этого блока мы не смогли бы вызвать асинхронный откат (RollbackAsync), и серверу 
                // пришлось бы блокировать свои потоки, выполняя отмену синхронно внутри механизма using.
                try
                {
                    // Шаг 1: Создаем пользователя в таблице AspNetUsers
                    User user = _userMapper.ToEntity(createDto);

                    IdentityResult createdUser = await _userManager.CreateAsync(user, createDto.Password);
                    if (!createdUser.Succeeded)
                    {
                        // Вместо InvalidOperationException и склейки строк выбрасываем кастомное исключение валидации.
                        // IdentityValidationExceptionHandler перехватит его и вернет красивый 400 BadRequest.
                        throw new IdentityValidationException(createdUser.Errors.Select(e => e.Description));
                    }

                    // Шаг 2: Гарантируем наличие роли в таблице AspNetRoles
                    string roleName = createDto.Role.GetDisplayName();
                    LogCheckingRoleExistence(_logger, roleName);
                    if (!await _roleManager.RoleExistsAsync(roleName))
                    {
                        var roleResult = await _roleManager.CreateAsync(new Role(roleName));
                        if (!roleResult.Succeeded)
                        {
                            LogIdentityRoleCreateFailed(_logger, roleName);
                            throw new IdentityValidationException(roleResult.Errors.Select(e => e.Description));
                        }
                    }

                    // Шаг 3: Привязываем пользователя к роли в связующей таблице AspNetUserRoles
                    LogAssigningRoleToUser(_logger, createDto.UserName, roleName);
                    var addRoleResult = await _userManager.AddToRoleAsync(user, roleName);
                    if (!addRoleResult.Succeeded)
                    {
                        LogRoleAssignmentFailed(_logger, createDto.UserName, roleName);
                        throw new IdentityValidationException(addRoleResult.Errors.Select(e => e.Description));
                    }
                    
                    // Если все шаги успешны — подтверждаем изменения и сохраняем их на диск
                    await transaction.CommitAsync(cancellationToken);
                    LogUserCreationTransactionCommitted(_logger, createDto.UserName);

                    return _userMapper.ToCommandResultDto(user, roleName);
                }
                catch
                {
                    // При любой ошибке (включая наше исключение валидации) полностью стираем временные изменения из БД
                    if (_dbContext.Database.CurrentTransaction != null)
                    {
                        LogRollingBackUserCreationTransaction(_logger, createDto.UserName);
                        await transaction.RollbackAsync(cancellationToken);
                    }
                    throw; // Передаем исключение дальше. Исключение валидации уйдет в свой Handler, остальные — в системные
                }
            }
        }

        public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            LogUserDeletionRequested(_logger, id);

            User? user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                LogUserToDeleteNotFound(_logger, id);
                throw new KeyNotFoundException($"Не удалось найти пользователя с ID {id}.");
            }

            IdentityResult result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                LogUserDeletionFailed(_logger, id);
                throw new IdentityValidationException(result.Errors.Select(e => e.Description));
            }

            LogUserSuccessfullyDeleted(_logger, id, user.UserName ?? string.Empty);
            return true;
        }
    }
}
