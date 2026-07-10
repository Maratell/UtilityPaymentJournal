using UtilityPaymentJournal.DTOs.UtilityProviders;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Models.UtilityProviders;
using UtilityProviderPaymentJournal.Interface.Mapping;


namespace UtilityPaymentJournal.Mapping
{
    public class UtilityProviderMapper : IUtilityProviderMapper
    {
        public CreateUtilityProviderDTO ToDto(CreateUtilityProviderViewModel createUtilityProviderViewModel)
        {
            if (createUtilityProviderViewModel == null)
                return null!;

            return new CreateUtilityProviderDTO
            {
                Name = createUtilityProviderViewModel.Name
            };
        }

        public UtilityProviderDTO ToDto(UtilityProvider utilityProvider)
        {
            if (utilityProvider == null)
                return null!;

            return new UtilityProviderDTO
            {
                Id = utilityProvider.Id,
                Name = utilityProvider.Name
            };
        }

        public EditUtilityProviderDTO ToDto(EditUtilityProviderViewModel editUtilityProviderViewModel)
        {
            if (editUtilityProviderViewModel == null)
                return null!;

            return new EditUtilityProviderDTO
            {
                Id = editUtilityProviderViewModel.Id,
                Name = editUtilityProviderViewModel.Name
            };
        }

        public UtilityProvider ToEntity(CreateUtilityProviderDTO createUtilityProviderDto)
        {
            if (createUtilityProviderDto == null)
                return null!;

            return new UtilityProvider
            {
                Name = createUtilityProviderDto.Name
            };
        }

        public UtilityProviderViewModel ToViewModel(UtilityProviderDTO utilityProviderDto)
        {
            if (utilityProviderDto == null)
                return null!;

            return new UtilityProviderViewModel
            {
                Id = utilityProviderDto.Id,
                Name = utilityProviderDto.Name
            };
        }

        public void UpdateEntity(EditUtilityProviderDTO dto, UtilityProvider entity)
        {
            if (dto == null || entity == null)
                return;

            entity.Name = dto.Name;
            //entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
