using UtilityPaymentJournal.Infrastructure.EF.Entity.ComplaintBoard;

namespace UtilityPaymentJournal.Features.ComplaintBoard.GetList
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения списка карточек жалоб.
    /// </summary>
    public static class GetComplaintsListMappingExtensions
    {
        /// <summary>
        /// Преобразует коллекцию доменных сущностей в единый объект ответа API со списком элементов.
        /// </summary>
        /// <param name="entities">Коллекция доменных сущностей <see cref="Complaint"/>, загруженных из БД.</param>
        /// <returns>Заполненный объект ответа <see cref="GetComplaintsListResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если коллекция доменных сущностей равна null.</exception>
        public static GetComplaintsListResponse ToResponse(this IEnumerable<Complaint> entities)
        {
            ArgumentNullException.ThrowIfNull(entities);

            // Трансформируем сущности во вложенные рекорды Item
            GetComplaintsListResponse.Item[] items = entities
                .Select(e => new GetComplaintsListResponse.Item(
                    Id: e.Id,
                    Title: e.Title,
                    Description: e.Description,
                    UtilityId: e.UtilityId,
                    UtilityName: e.Utility?.Name,         
                    UtilityIcon: e.Utility?.IconClass,         
                    CreatedAt: e.CreatedAt,
                    SubmissionDate: e.SubmissionDate,
                    IssueResolutionDate: e.IssueResolutionDate,
                    Status: e.Status
                ))
                .ToArray();

            // Возвращаем готовый единый объект ответа
            return new GetComplaintsListResponse(items);
        }
    }
}
