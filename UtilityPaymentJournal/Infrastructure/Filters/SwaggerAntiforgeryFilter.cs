using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using UtilityPaymentJournal.Common.Constants;

namespace UtilityPaymentJournal.Infrastructure.Filters
{
    /// <summary>
    /// Фильтр для Swagger, который автоматически находит эндпоинты, защищенные от CSRF-атак,
    /// и динамически добавляет для них поле ввода заголовка, имя которого задано в константе AntiforgerySecurityConstants.AntiforgeryHeaderName.
    /// </summary>
    public class SwaggerAntiforgeryFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // ПРОВЕРКА ТОЧЕЧНОЙ ЗАЩИТЫ [ValidateAntiForgeryToken]
            // Проверяем, повесил ли разработчик атрибут защиты вручную:
            // - либо прямо на сам метод
            // - либо целиком на класс контроллера (если метод не null)
            bool hasExplicitAntiforgery = context.MethodInfo.GetCustomAttributes(true)
                .Any(attr => attr.GetType() == typeof(ValidateAntiForgeryTokenAttribute)) ||
                (context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
                .Any(attr => attr.GetType() == typeof(ValidateAntiForgeryTokenAttribute)) ?? false);

            // ОПРЕДЕЛЕНИЕ ТИПА HTTP-МЕТОДА
            // Узнаем, какой HTTP-метод используется (GET, POST и т.д.) и переводим в верхний регистр.
            // Глобальная защита AutoValidate защищает только "небезопасные" методы POST, PUT, PATCH, DELETE,
            // которые меняют данные на сервере. GET-запросы она не трогает.
            string? httpMethod = context.ApiDescription.HttpMethod?.ToUpperInvariant();
            bool isUnsafeMethod = httpMethod == "POST" || httpMethod == "PUT" || httpMethod == "PATCH" || httpMethod == "DELETE";

            // ПРОВЕРКА ИСКЛЮЧЕНИЙ [IgnoreAntiforgeryToken]
            // Проверяем, нет ли на методе атрибута-исключения, который отключает защиту от CSRF для этого конкретного действия.
            bool hasIgnoreAttribute = context.MethodInfo.GetCustomAttributes(true)
                .Any(attr => attr.GetType() == typeof(IgnoreAntiforgeryTokenAttribute));

            // ФИНАЛЬНОЕ РЕШЕНИЕ: НУЖЕН ЛИ ТОКЕН ДЛЯ ЭТОГО ЭНДПОИНТА?
            // Токен нужен, если:
            // - разработчик явно написал [ValidateAntiForgeryToken] над методом/контроллером
            // - ИЛИ это изменяющий метод (POST/PUT/...) и при этом на нем НЕТ атрибута [IgnoreAntiforgeryToken]
            bool needsAntiforgery = hasExplicitAntiforgery || (isUnsafeMethod && !hasIgnoreAttribute);

            // МОДИФИКАЦИЯ СТРАНИЦЫ SWAGGER
            // Если эндпоинту требуется защита, мы добавляем поле для ввода токена
            if (needsAntiforgery)
            {
                // Если у этого метода в Swagger еще вообще нет списка параметров, создаем пустой список
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IOpenApiParameter>();
                }

                // Проверяем, чтобы случайно не добавить заголовок X-XSRF-TOKEN дважды
                if (!operation.Parameters.Any(p => p.Name == AntiforgerySecurityConstants.AntiforgeryHeaderName))
                {
                    // Добавляем новое поле ввода в интерфейс Swagger для этого метода
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = AntiforgerySecurityConstants.AntiforgeryHeaderName, // Имя заголовка, которое ожидает ASP.NET Core Antiforgery
                        In = ParameterLocation.Header, // Указываем Swagger, что этот параметр передается в Заголовках (Header)
                        Required = false, // Делаем поле необязательным для заполнения (чтобы не блокировать UI, если токен не нужен)
                        Description = "Токен защиты от CSRF (ValidateAntiForgeryToken / AutoValidate)", // Подсказка для разработчика
                        Schema = new OpenApiSchema
                        {
                            Type = JsonSchemaType.String // Генерируем схему данных "string",
                                                         // чтобы в поле можно было вводить обычный текст токена
                        }
                    });
                }
            }
        }
    }



    //public class SwaggerAntiforgeryFilter : IOperationFilter
    //{
    //    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    //    {
    //        // 1. Ищем точечный атрибут ValidateAntiForgeryToken (для ручной подстраховки)
    //        var hasExplicitAntiforgery = context.MethodInfo.GetCustomAttributes(true)
    //            .Any(attr => attr.GetType().Name == "ValidateAntiForgeryTokenAttribute") ||
    //            (context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
    //            .Any(attr => attr.GetType().Name == "ValidateAntiForgeryTokenAttribute") ?? false);

    //        // 2. Учитываем глобальный AutoValidateAntiforgeryTokenAttribute (он защищает POST, PUT, PATCH, DELETE)
    //        var httpMethod = context.ApiDescription.HttpMethod?.ToUpperInvariant();
    //        var isUnsafeMethod = httpMethod == "POST" || httpMethod == "PUT" || httpMethod == "PATCH" || httpMethod == "DELETE";

    //        var hasIgnoreAttribute = context.MethodInfo.GetCustomAttributes(true)
    //            .Any(attr => attr.GetType().Name == "IgnoreAntiforgeryTokenAttribute");

    //        // Токен нужен, если есть точечный атрибут ИЛИ если метод изменяющий (и работает глобальная защита из Program.cs)
    //        var needsAntiforgery = hasExplicitAntiforgery || (isUnsafeMethod && !hasIgnoreAttribute);

    //        if (needsAntiforgery)
    //        {
    //            if (operation.Parameters == null)
    //            {
    //                operation.Parameters = new List<IOpenApiParameter>();
    //            }

    //            if (!operation.Parameters.Any(p => p.Name == "X-XSRF-TOKEN"))
    //            {
    //                operation.Parameters.Add(new OpenApiParameter
    //                {
    //                    Name = "X-XSRF-TOKEN",
    //                    In = ParameterLocation.Header,
    //                    Required = false,
    //                    Description = "Токен защиты от CSRF (ValidateAntiForgeryToken / AutoValidate)",
    //                    Schema = new OpenApiSchema
    //                    {
    //                        Type = JsonSchemaType.String // Чистая строка, которая везде скомпилируется
    //                    }
    //                });
    //            }
    //        }
    //    }
    //}


}
