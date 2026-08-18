using FluentValidation;
using MediatR;

namespace UtilityPaymentJournal.Common.Behaviours
{
    /// <summary>
    /// Сквозной обработчик (Middleware) для пайплайна MediatR, отвечающий за автоматическую валидацию запросов.
    /// Перехватывает любую команду (Command) или запрос (Query) до того, как они попадут в свой Handler.
    /// </summary>
    /// <typeparam name="TRequest">Тип входящего запроса MediatR, реализующего <see cref="IRequest{TResponse}"/>.</typeparam>
    /// <typeparam name="TResponse">Тип возвращаемого ответа.</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="ValidationBehavior{TRequest, TResponse}"/>.
        /// </summary>
        /// <param name="validators">Коллекция всех зарегистрированных валидаторов FluentValidation для данного типа запроса.</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// Метод перехвата и обработки запроса MediatR.
        /// </summary>
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Если для данного запроса нет ни одного правила валидации, сразу передаем управление дальше
            if (!_validators.Any())
            {
                return await next();
            }

            // Создаем контекст валидации FluentValidation для текущего запроса
            var context = new ValidationContext<TRequest>(request);

            // Запускаем все найденные валидаторы параллельно для экономии времени
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            // Собираем все ошибки валидации в один общий плоский список, отсекая пустые результаты
            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            // Если обнаружена хотя бы одна ошибка — прерываем выполнение и выбрасываем исключение.
            // До выполнения Handler-а дело гарантированно не дойдет.
            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            // Если все проверки пройдены успешно — передаем управление следующему звену пайплайна или самому Handler-у
            return await next();
        }
    }
}
