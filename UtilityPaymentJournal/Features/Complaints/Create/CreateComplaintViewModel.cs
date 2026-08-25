using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.Features.Complaints.Create
{
    /// <summary>
    /// Модель представления для создания новой жалобы (ввод данных из формы).
    /// </summary>
    public class CreateComplaintViewModel
    {
        [Display(Name = "Заголовок жалобы")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Описание проблемы")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Наименование услуги")]
        public long UtilityId { get; set; }

        [Display(Name = "Дата подачи жалобы")]
        [DataType(DataType.Date)]
        public DateTime? SubmissionDate { get; set; }

        [Display(Name = "Дата решения проблемы")]
        [DataType(DataType.Date)]
        public DateTime? IssueResolutionDate { get; set; }

        [Display(Name = "Статус жалобы")]
        public ComplaintStatus Status { get; set; } = ComplaintStatus.New;
    }
}
