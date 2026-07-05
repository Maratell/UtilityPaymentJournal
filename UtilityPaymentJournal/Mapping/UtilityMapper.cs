using UtilityPaymentJournal.DTO.Utilities;
using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Interface.Mapping;
using UtilityPaymentJournal.Models.Utilities;

namespace UtilityPaymentJournal.Mapping
{
    public class UtilityMapper : IUtilityMapper
    {
        public CreateUtilityDTO ToDto(CreateUtilityViewModel vm)
        {
            if (vm == null)
                return null!;

            return new CreateUtilityDTO
            {
                Name = vm.Name
            };
        }

        public UtilityDTO ToDto(Utility entity)
        {
            if (entity == null)
                return null!;

            return new UtilityDTO
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public EditUtilityDTO ToDto(EditUtilityViewModel vm)
        {
            if (vm == null)
                return null!;

            return new EditUtilityDTO
            {
                Id = vm.Id,
                Name = vm.Name
            };
        }

        public Utility ToEntity(CreateUtilityDTO dto)
        {
            if (dto == null)
                return null!;

            return new Utility
            {
                Name = dto.Name
            };
        }

        public UtilityViewModel ToViewModel(UtilityDTO dto)
        {
            if (dto == null)
                return null!;

            return new UtilityViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public void UpdateEntity(EditUtilityDTO dto, Utility entity)
        {
            if (dto == null || entity == null)
                return;

            entity.Name = dto.Name;
        }
    }
}
