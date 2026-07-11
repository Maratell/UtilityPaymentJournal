using Microsoft.AspNetCore.Http.HttpResults;
using UtilityPaymentJournal.DTOs.UtilityProviders;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Models.UtilityProviders;
using UtilityProviderPaymentJournal.Interface.Mapping;


namespace UtilityPaymentJournal.Mapping
{
    public class UtilityProviderMapper : IUtilityProviderMapper
    {
        public CreateUtilityProviderDto ToDto(CreateUtilityProviderViewModel createViewModel)
        {
            ArgumentNullException.ThrowIfNull(createViewModel);
            return new CreateUtilityProviderDto(createViewModel.Name);
        }

        public UtilityProviderDto ToDto(UtilityProvider entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            return new UtilityProviderDto(entity.Id, entity.Name);
        }

        public EditUtilityProviderDto ToDto(EditUtilityProviderViewModel editViewModel)
        {
            ArgumentNullException.ThrowIfNull(editViewModel);
            return new EditUtilityProviderDto(editViewModel.Id, editViewModel.Name);
        }

        public UtilityProvider ToEntity(CreateUtilityProviderDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);
            return new UtilityProvider
            {
                Name = createDto.Name
            };
        }

        public UtilityProviderViewModel ToViewModel(UtilityProviderDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);
            return new UtilityProviderViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public void UpdateEntity(EditUtilityProviderDto editDto, UtilityProvider entity)
        {
            ArgumentNullException.ThrowIfNull(editDto);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Name = editDto.Name;
            //entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}
