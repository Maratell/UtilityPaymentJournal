using UtilityPaymentJournal.DTOs.Utilities;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Models.Utilities;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IUtilityMapper
    {
        CreateUtilityDto ToDto(CreateUtilityViewModel createViewModel);
        UtilityDto ToDto(Utility entity);
        EditUtilityDto ToDto(EditUtilityViewModel editViewModel);
        Utility ToEntity(CreateUtilityDto createDto);
        UtilityViewModel ToViewModel(UtilityDto dto);
        void UpdateEntity(EditUtilityDto editDto, Utility entity);
    }
}
