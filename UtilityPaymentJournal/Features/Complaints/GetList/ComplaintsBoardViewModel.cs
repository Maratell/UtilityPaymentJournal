using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Features.Complaints.Create;

namespace UtilityPaymentJournal.Features.Complaints.GetList
{
    /// <summary>
    /// Модель представления для Kanban-доски жалоб.
    /// </summary>
    public class ComplaintsBoardViewModel
    {
        /// <summary>
        /// Все жалобы, сгруппированные по их статусу. 
        /// Позволяет рендерить колонки динамически через простой цикл foreach.
        /// </summary>
        public Dictionary<ComplaintStatus, List<GetComplaintsListResponse.Item>> ComplaintsByStatus { get; set; } = new();

        /// <summary>
        /// Изолированная модель данных формы. 
        /// На доске она НЕ нужна, так как форма должна рендериться через отдельный ViewComponent.
        /// </summary>
        public CreateComplaintViewModel EmptyForm { get; set; } = new();
    }
}
