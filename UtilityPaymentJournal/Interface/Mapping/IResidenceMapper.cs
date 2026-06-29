using UtilityPaymentJournal.DTO.Residences;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.Models.Residences;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IResidenceMapper
    {
        CreateResidenceDTO ToDto(CreateResidenceViewModel residenceCreateViewModel);

        ResidenceDTO ToDto(Residence residence);

        EditResidenceDTO ToDto(EditResidenceViewModel editResidenceViewModel);

        Residence ToEntity(CreateResidenceDTO createResidenceDto);

        ResidenceViewModel ToViewModel(ResidenceDTO residenceDto);
    }
}
