using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using UtilityPaymentJournal.Models.Utilities;

namespace UtilityPaymentJournal.Models.ComplaintBoard
{
    public class ComplaintBoardViewModel
    {
        /// <summary>
        /// Список новых (созданных) жалоб
        /// </summary>
        [ValidateNever]
        public List<ComplaintViewModel> NewComplaints { get; set; } = new();
        /// <summary>
        /// Список жалоб со статусом "В работе"
        /// </summary>
        [ValidateNever]
        public List<ComplaintViewModel> InProgressComplaints { get; set; } = new();
        /// <summary>
        /// Список жалоб со статусом "Решено"
        /// </summary>
        [ValidateNever]
        public List<ComplaintViewModel> ResolvedComplaints { get; set; } = new();
        /// <summary>
        /// Список услуг
        /// </summary>
        [ValidateNever]
        public List<UtilityViewModel> AvailableUtilities { get; set; } = new();

        // Поле для формы быстрого добавления (форма биндится прямо сюда)
        public ComplaintViewModel EmptyForm { get; set; } = new();
    }
}
