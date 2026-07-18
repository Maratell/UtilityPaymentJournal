using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Commands
{
    /// <summary>
    /// Модель представления для запроса на изменение статуса с UI.
    /// </summary>
    public class ChangeComplaintStatusViewModel
    {
        /// <summary>
        /// Идентификатор жалобы, у которой меняется статус.
        /// </summary>
        [Required]
        public long Id { get; set; }
        /// <summary>
        /// Новый целевой статус жалобы.
        /// </summary>
        [Required(ErrorMessage = "Необходимо указать новый статус")]
        public ComplaintStatus NewStatus { get; set; }
    }
}
