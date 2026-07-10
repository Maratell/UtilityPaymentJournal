using UtilityPaymentJournal.DTOs.Residences;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Models.Residences;

namespace UtilityPaymentJournal.Mapping
{
    public class ResidenceMapper : IResidenceMapper
    {
        public CreateResidenceDto ToDto(CreateResidenceViewModel createViewModel)
        {
            ArgumentNullException.ThrowIfNull(createViewModel);
            return new CreateResidenceDto(createViewModel.Address);
        }

        public ResidenceDto ToDto(Residence entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            return new ResidenceDto(entity.Id, entity.Address);
        }

        public EditResidenceDto ToDto(EditResidenceViewModel editViewModel)
        {
            ArgumentNullException.ThrowIfNull(editViewModel);
            return new EditResidenceDto(editViewModel.Id, editViewModel.Address);
        }

        public Residence ToEntity(CreateResidenceDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);
            return new Residence() 
            { 
                Address = createDto.Address 
            };
        }

        public ResidenceViewModel ToViewModel(ResidenceDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            return new ResidenceViewModel
            {
                Id = dto.Id,
                Address = dto.Address
            };
        }

        public void UpdateEntity(EditResidenceDto editDto, Residence entity)
        {
            ArgumentNullException.ThrowIfNull(editDto);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Address = editDto.Address;
            //entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
