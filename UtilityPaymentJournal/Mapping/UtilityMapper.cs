using UtilityPaymentJournal.DTOs.Utilities;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Models.Utilities;

namespace UtilityPaymentJournal.Mapping
{
    public class UtilityMapper : IUtilityMapper
    {
        public CreateUtilityDto ToDto(CreateUtilityViewModel createViewModel)
        {
            ArgumentNullException.ThrowIfNull(createViewModel);
            return new CreateUtilityDto(createViewModel.Name);
        }

        public UtilityDto ToDto(Utility entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            return new UtilityDto(entity.Id, entity.Name);
        }

        public EditUtilityDto ToDto(EditUtilityViewModel editViewModel)
        {
            ArgumentNullException.ThrowIfNull(editViewModel);
            return new EditUtilityDto(editViewModel.Id, editViewModel.Name);
        }

        public Utility ToEntity(CreateUtilityDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);
            return new Utility
            {
                Name = createDto.Name
            };
        }

        public UtilityViewModel ToViewModel(UtilityDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            return new UtilityViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public void UpdateEntity(EditUtilityDto editDto, Utility entity)
        {
            ArgumentNullException.ThrowIfNull(editDto);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Name = editDto.Name;
        }
    }
}
