using UtilityPaymentJournal.DTOs.UtilityProviders;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Models.UtilityProviders;

namespace UtilityProviderPaymentJournal.Interface.Mapping
{
    public interface IUtilityProviderMapper
    {
        CreateUtilityProviderDTO ToDto(CreateUtilityProviderViewModel createViewModel);

        UtilityProviderDTO ToDto(UtilityProvider entity);

        EditUtilityProviderDTO ToDto(EditUtilityProviderViewModel editViewModel);

        UtilityProvider ToEntity(CreateUtilityProviderDTO dto);

        UtilityProviderViewModel ToViewModel(UtilityProviderDTO dto);

        void UpdateEntity(EditUtilityProviderDTO dto, UtilityProvider entity);
    }
}
