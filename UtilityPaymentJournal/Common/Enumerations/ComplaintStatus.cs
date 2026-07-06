using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Common.Enumerations
{
    public enum ComplaintStatus
    {
        [Display(Name = "Новые")] 
        New = 0,
        [Display(Name = "В работе")] 
        InProgress = 1,
        [Display(Name = "Решенные")] 
        Resolved = 2
    }
}
