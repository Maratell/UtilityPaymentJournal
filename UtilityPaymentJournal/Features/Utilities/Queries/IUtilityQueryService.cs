using UtilityPaymentJournal.Common.Specifications;
using UtilityPaymentJournal.EF.Entity.Utilities;

namespace UtilityPaymentJournal.Features.Utilities.Queries
{
    /// <summary>
    /// Интерфейс сервиса запросов (чтения) для получения данных о коммунальных услугах.
    /// Отвечает исключительно за выборку и предоставление данных (R) без изменения состояния БД в рамках паттерна CQRS.
    /// </summary>
    public interface IUtilityQueryService
    {
        /// <summary>
        /// Получить подробную информацию о коммунальной услуге по её идентификатору.
        /// </summary>
        /// <param name="id">Уникальный идентификатор записи коммунальной услуги</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>ДТО с полными данными коммунальной услуги</returns>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если коммунальная услуга с указанным ID отсутствует в БД</exception>
        Task<UtilityQueryResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Получить список коммунальных услуг в системе на основе переданной спецификации критериев фильтрации.
        /// </summary>
        /// <param name="criteria">Спецификация, инкапсулирующая динамическое бизнес-правило отбора данных</param>
        /// <param name="cancellationToken">Токен отмены асинхронной операции</param>
        /// <returns>Коллекция ДТО коммунальных услуг, оптимизированная для вывода на UI</returns>
        Task<IReadOnlyCollection<UtilityQueryResultDto>> GetAllAsync(ICriteriaSpecification<Utility> criteria, CancellationToken cancellationToken = default);
    }
}
