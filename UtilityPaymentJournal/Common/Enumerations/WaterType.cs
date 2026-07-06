using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Common.Enumerations
{
    public enum WaterType
    {
        [Display(Name = "холодная")]
        Cold = 0,
        [Display(Name = "горячая")]
        Hot = 1,
        [Display(Name = "водоотведение")]
        Drainage = 2   
    }
}
