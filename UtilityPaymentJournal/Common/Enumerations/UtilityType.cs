using System.ComponentModel.DataAnnotations;

namespace UtilityPaymentJournal.Common.Enumerations
{
    public enum UtilityType
    {
        [Display(Name = "водоснабжение")]
        WaterSupply = 0,
        [Display(Name = "электроэнергия")]
        Electricity = 1
    }
}
