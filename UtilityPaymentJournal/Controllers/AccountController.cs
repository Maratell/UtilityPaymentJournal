using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using UtilityPaymentJournal.EF.Entity.Authentication;
using UtilityPaymentJournal.Models.ViewModels;

namespace UtilityPaymentJournal.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, 
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Login - отображает форму
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //// POST: /Account/Login - обрабатывает данные формы
        //[HttpPost]
        ////[ValidateAntiForgeryToken] // Защита от CSRF атак
        //public async Task<IActionResult> Login(LoginViewModel login)
        //{
        //    // 1. Простая проверка данных (в реальности - сравнение с БД)
        //    if (login.UserName == "admin" && login.Password == "password123")
        //    {
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, login.UserName),
        //            //new Claim(ClaimTypes.NameIdentifier, password) // Рекомендуется добавить ID пользователя
        //        };

        //        //var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //        //var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        //        //await HttpContext.SignInAsync(
        //        //    CookieAuthenticationDefaults.AuthenticationScheme,
        //        //    claimsPrincipal);

        //        // Используем SignInManager вместо ручного HttpContext.SignInAsync
        //        var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, isPersistent: false, lockoutOnFailure: false);

        //        // 2. Вручную обновляем User для текущего запроса
        //        //HttpContext.User = claimsPrincipal;

        //        return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });

        //        // Успешный вход: редирект на главную
        //        //return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
        //    }

        //    //// 2. Если вход не удался
        //    //ModelState.AddModelError("", "Неверный логин или пароль");

        //    // ВЕРНЫЙ ВАРИАНТ ДЛЯ AJAX: возвращаем JSON с ошибкой
        //    return Json(new
        //    {
        //        success = false,
        //        message = "Неверный логин или пароль"
        //    });
        //    //return View();
        //}

        [HttpPost]
        //[ValidateAntiForgeryToken] // Верните защиту обратно
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            // 1. Ищем пользователя по имени (или Email в зависимости от ваших настроек)
            var user = await _userManager.FindByNameAsync(login.UserName);

            if (user != null)
            {
                // 2. Проверяем пароль перед входом
                var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var claims = new List<Claim> 
                    {
                        new Claim("FirstName", user.FirstName),
                        new Claim("LastName", user.LastName)
                    };

                    await _signInManager.SignInWithClaimsAsync(user, isPersistent: false, claims);

                    // Если пароль верный, выполняем сам вход (установка куки)
                    //await _signInManager.SignInAsync(user, isPersistent: false);

                    return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
                }

                // Разбираем специфические ошибки
                if (result.IsLockedOut)
                {
                    return Json(new { success = false, message = "Аккаунт временно заблокирован из-за множества неверных попыток." });
                }
                if (result.IsNotAllowed)
                {
                    return Json(new { success = false, message = "Вход не разрешен. Возможно, требуется подтверждение почты." });
                }
            }

            // Если пользователя нет или пароль не подошел
            return Json(new { success = false, message = "Неверный логин или пароль" });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Json(new { success = true, redirectUrl = Url.Action("Index", "Account") });
        }
    }
}
