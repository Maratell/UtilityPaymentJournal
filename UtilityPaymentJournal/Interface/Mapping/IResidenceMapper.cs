using UtilityPaymentJournal.DTOs.Residences;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.Models.Residences;

namespace UtilityPaymentJournal.Interface.Mapping
{
    public interface IResidenceMapper
    {
        CreateResidenceDto ToDto(CreateResidenceViewModel createViewModel);
        ResidenceDto ToDto(Residence entity);
        EditResidenceDto ToDto(EditResidenceViewModel editViewModel);
        Residence ToEntity(CreateResidenceDto createDto);
        ResidenceViewModel ToViewModel(ResidenceDto dto);
        void UpdateEntity(EditResidenceDto editDto, Residence entity);
    }
}
