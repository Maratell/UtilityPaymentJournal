using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Features.Complaints.Models;

namespace UtilityPaymentJournal.Features.Complaints.Queries
{
    /// <summary>
    /// Интерфейс сервиса запросов (чтения) для получения данных о жалобах.
    /// Отвечает исключительно за выборку и предоставление данных (R) без изменения состояния БД в рамках паттерна CQRS.
    /// </summary>
    public interface IComplaintQueryService
    {
        /// <summary>
        /// Получить подробную информацию о жалобе по её идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор записи жалобы</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>ДТО с развернутыми данными жалобы и текстовыми деталями связей</returns>
        /// <exception cref="KeyNotFoundException">Выбрасывается, если запись с указанным ID отсутствует в БД</exception>
        Task<ComplaintQueryResultDto> GetByIdAsync(long id, CancellationToken cancellationToken = default);
        /// <summary>
        /// Получить список всех зарегистрированных жалоб в системе.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Коллекция ДТО жалоб, оптимизированная для вывода на UI</returns>
        Task<IReadOnlyCollection<ComplaintQueryResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
        /// <summary>
        /// Получить словарь всех жалоб, сгруппированных по их текущему статусу для Kanban-доски.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Словарь, где ключом является статус жалобы, а значением — список соответствующих моделей представления</returns>
        Task<Dictionary<ComplaintStatus, List<ComplaintViewModel>>> GetComplaintsGroupedByStatusAsync(CancellationToken cancellationToken = default);
    }
}
