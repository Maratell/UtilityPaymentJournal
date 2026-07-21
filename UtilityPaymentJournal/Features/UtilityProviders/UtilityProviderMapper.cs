using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Features.UtilityProviders.Commands;
using UtilityPaymentJournal.Features.UtilityProviders.Models;
using UtilityPaymentJournal.Features.UtilityProviders.Queries;

namespace UtilityPaymentJournal.Features.UtilityProviders
{
    /// <summary>
    /// Реализация маппера для преобразования моделей данных поставщика коммунальных услуг между слоями.
    /// </summary>
    public class UtilityProviderMapper : IUtilityProviderMapper
    {
        public CreateUtilityProviderDto ToDto(CreateUtilityProviderViewModel createViewModel)
        {
            ArgumentNullException.ThrowIfNull(createViewModel);

            return new CreateUtilityProviderDto(
                Name: createViewModel.Name
            );
        }

        public EditUtilityProviderDto ToDto(EditUtilityProviderViewModel editViewModel)
        {
            ArgumentNullException.ThrowIfNull(editViewModel);

            return new EditUtilityProviderDto(
                Name: editViewModel.Name
            );
        }

        public UtilityProvider ToEntity(CreateUtilityProviderDto createDto)
        {
            ArgumentNullException.ThrowIfNull(createDto);

            return new UtilityProvider
            {
                Name = createDto.Name
            };
        }

        public void UpdateEntity(EditUtilityProviderDto editDto, UtilityProvider entity)
        {
            ArgumentNullException.ThrowIfNull(editDto);
            ArgumentNullException.ThrowIfNull(entity);

            entity.Name = editDto.Name;
        }

        public UtilityProviderCommandResultDto ToCommandResultDto(UtilityProvider entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new UtilityProviderCommandResultDto(
                Id: entity.Id,
                Name: entity.Name
            );
        }

        public UtilityProviderQueryResultDto ToQueryResultDto(UtilityProvider entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new UtilityProviderQueryResultDto(
                Id: entity.Id,
                Name: entity.Name
            );
        }

        public UtilityProviderCreatedViewModel ToCreatedViewModel(UtilityProviderCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new UtilityProviderCreatedViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public UtilityProviderUpdatedViewModel ToUpdatedViewModel(UtilityProviderCommandResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new UtilityProviderUpdatedViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }

        public UtilityProviderDetailsViewModel ToViewModel(UtilityProviderQueryResultDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            return new UtilityProviderDetailsViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };
        }
    }
}
