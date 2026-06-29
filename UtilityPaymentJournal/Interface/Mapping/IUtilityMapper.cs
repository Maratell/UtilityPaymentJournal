using UtilityPaymentJournal.DTO.Utilities;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Models.Utilities;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IUtilityMapper
    {
        CreateUtilityDTO ToDto(CreateUtilityViewModel vm);

        UtilityDTO ToDto(Utility entity);

        EditUtilityDTO ToDto(EditUtilityViewModel vm);

        Utility ToEntity(CreateUtilityDTO dto);

        UtilityViewModel ToViewModel(UtilityDTO dto);
    }
}
