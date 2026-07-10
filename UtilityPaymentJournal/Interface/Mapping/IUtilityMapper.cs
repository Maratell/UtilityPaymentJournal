using UtilityPaymentJournal.DTOs.Utilities;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Models.Utilities;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IUtilityMapper
    {
        CreateUtilityDTO ToDto(CreateUtilityViewModel createViewModel);
        UtilityDTO ToDto(Utility entity);
        EditUtilityDTO ToDto(EditUtilityViewModel editViewModel);
        Utility ToEntity(CreateUtilityDTO dto);
        UtilityViewModel ToViewModel(UtilityDTO dto);
        void UpdateEntity(EditUtilityDTO dto, Utility entity);
    }
}
