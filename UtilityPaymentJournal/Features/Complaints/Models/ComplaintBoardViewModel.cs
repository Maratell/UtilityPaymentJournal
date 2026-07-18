using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Models
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
        public Dictionary<ComplaintStatus, List<ComplaintViewModel>> ComplaintsByStatus { get; set; } = new();

        /// <summary>
        /// Изолированная модель данных формы. 
        /// На доске она НЕ нужна, так как форма должна рендериться через отдельный ViewComponent.
        /// </summary>
        public CreateComplaintViewModel EmptyForm { get; set; } = new();
    }
}
