using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.DTO;
using UtilityPaymentJournal.EF.Context;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Models.ViewModels;

namespace UtilityPaymentJournal.Controllers
{
    /// <summary>
    /// Контроллер для создания пользователей и ролей
    /// </summary>
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        //private readonly ApplicationDbContext _dbContext;
        //private readonly IMapper _mapper;

        public AdminController(UserManager<User> userManager
            , RoleManager<Role> roleManager
            //, ApplicationDbContext dbContext
            /*, IMapper mapper*/)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            //_dbContext = dbContext;
            //_mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserWithRole(CreateUserViewModel createUserVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1. Создаем пользователя (без RoleId)
            var user = new User
            {
                UserName = createUserVM.UserName,
                FirstName = createUserVM.FirstName,
                LastName = createUserVM.LastName
            };

            var result = await _userManager.CreateAsync(user, createUserVM.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // 2. Создаем роль, если её нет
            string roleName = "User";
            try
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await _roleManager.CreateAsync(new Role(roleName));
                    if (!roleResult.Succeeded)
                        return BadRequest("Ошибка при создании роли.");
                }
            }
            catch (Exception ex)
            {
                // InnerException часто содержит реальную ошибку от базы данных (например, NpgsqlException)
                var innerMessage = ex.InnerException?.Message ?? "Нет вложенного исключения";
                return StatusCode(500, new { error = ex.Message, details = innerMessage });
            }

            // 3. Привязываем роль через стандартную таблицу связи
            var addRoleResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!addRoleResult.Succeeded)
                return BadRequest("Ошибка при добавлении пользователя в роль.");

            return Ok(new { message = "Пользователь успешно создан и связан с ролью!" });
        }

        //[HttpPost("create_user")]
        //[HttpPost]
        //public async Task<IActionResult> CreateUserWithRole(CreateUserViewModel createUserVM)
        //{
        //    // Автоматическая конвертация ViewModel в DTO
        //    //CreateUserDTO createUserDTO = _mapper.Map<CreateUserDTO>(createUserVM);
        //    CreateUserDTO createUserDTO = new CreateUserDTO()
        //    {
        //        UserName = createUserVM.UserName,
        //        FullName = createUserVM.FullName,
        //        Password = createUserVM.Password
        //    };

        //    string roleName = "User";

        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    // 1. Создаем пользователя
        //    User user = new User { UserName = createUserDTO.UserName/*, Email = model.Email*/ };
        //    var result = await _userManager.CreateAsync(user, createUserDTO.Password);

        //    if (!result.Succeeded)
        //        return BadRequest(result.Errors);

        //    // 2. Проверяем, существует ли роль, и создаем её, если нет
        //    if (!await _roleManager.RoleExistsAsync(roleName))
        //    {
        //        var roleResult = await _roleManager.CreateAsync(new Role(roleName));
        //        if (!roleResult.Succeeded)
        //            return BadRequest("Ошибка при создании роли.");
        //    }

        //    // 3. Добавляем пользователя в роль
        //    var addRoleResult = await _userManager.AddToRoleAsync(user, roleName);

        //    if (!addRoleResult.Succeeded)
        //        return BadRequest("Ошибка при добавлении пользователя в роль.");

        //    return Ok(new { message = "Пользователь и роль успешно созданы и связаны!" });
        //}
    }
}
