using UtilityPaymentJournal.DTOs.UtilityProviders;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Models.UtilityProviders;

namespace UtilityProviderPaymentJournal.Interface.Mapping
{
    public interface IUtilityProviderMapper
    {
        CreateUtilityProviderDto ToDto(CreateUtilityProviderViewModel createViewModel);
        UtilityProviderDto ToDto(UtilityProvider entity);
        EditUtilityProviderDto ToDto(EditUtilityProviderViewModel editViewModel);
        UtilityProvider ToEntity(CreateUtilityProviderDto createDto);
        UtilityProviderViewModel ToViewModel(UtilityProviderDto dto);
        void UpdateEntity(EditUtilityProviderDto editDto, UtilityProvider entity);
    }
}
