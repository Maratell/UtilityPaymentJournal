using UtilityPaymentJournal.DTO.Residences;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Models.Residences;

namespace UtilityPaymentJournal.Mapping
{
    public class ResidenceMapper : IResidenceMapper
    {
        public CreateResidenceDTO ToDto(CreateResidenceViewModel residenceCreateViewModel)
        {
            if (residenceCreateViewModel == null)
                return null!;

            return new CreateResidenceDTO
            {
                Address = residenceCreateViewModel.Address
            };
        }

        public ResidenceDTO ToDto(Residence residence)
        {
            if (residence == null)
                return null!;

            return new ResidenceDTO
            {
                Id = residence.Id,
                Address = residence.Address
            };
        }

        public EditResidenceDTO ToDto(EditResidenceViewModel editResidenceVM)
        {
            if (editResidenceVM == null)
                return null!;

            return new EditResidenceDTO
            {
                Id = editResidenceVM.Id,
                Address = editResidenceVM.Address
            };
        }

        public Residence ToEntity(CreateResidenceDTO residenceDto)
        {
            if (residenceDto == null)
                return null!;

            return new Residence
            {
                Address = residenceDto.Address
            };
        }

        public ResidenceViewModel ToViewModel(ResidenceDTO residenceDto)
        {
            if (residenceDto == null)
                return null!;

            return new ResidenceViewModel
            {
                Id = residenceDto.Id,
                Address = residenceDto.Address
            };
        }
    }
}
