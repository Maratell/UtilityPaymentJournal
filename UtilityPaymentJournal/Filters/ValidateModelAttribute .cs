using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UtilityPaymentJournal.Filters
{
    /// <summary>
    /// Фильтр не нужен, поскольку используется встроенный ApiController для api-контроллеров
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorMessage = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault() ?? "Ошибка валидации";

                // Прерываем выполнение и возвращаем BadRequest
                context.Result = new BadRequestObjectResult(errorMessage);
            }
        }
    }
}
