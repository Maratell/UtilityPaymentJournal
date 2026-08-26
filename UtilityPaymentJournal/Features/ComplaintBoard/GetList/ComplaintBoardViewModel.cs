using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.ComplaintBoard.GetList
{
    /// <summary>
    /// Модель представления для Kanban-доски жалоб.
    /// </summary>
    public class ComplaintBoardViewModel
    {
        /// <summary>
        /// Все жалобы, сгруппированные по их статусу. 
        /// Позволяет рендерить колонки динамически через простой цикл foreach.
        /// </summary>
        public Dictionary<ComplaintStatus, List<GetComplaintsListResponse.Item>> ComplaintsByStatus { get; set; } = new();
    }
}
