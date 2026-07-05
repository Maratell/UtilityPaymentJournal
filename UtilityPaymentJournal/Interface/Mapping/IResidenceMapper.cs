using UtilityPaymentJournal.DTO.Residences;
using UtilityPaymentJournal.DTO.UtilityProviders;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Models.Residences;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IResidenceMapper
    {
        CreateResidenceDTO ToDto(CreateResidenceViewModel createViewModel);
        ResidenceDTO ToDto(Residence entity);
        EditResidenceDTO ToDto(EditResidenceViewModel editViewModel);
        Residence ToEntity(CreateResidenceDTO dto);
        ResidenceViewModel ToViewModel(ResidenceDTO dto);
        void UpdateEntity(EditResidenceDTO dto, Residence entity);
    }
}
