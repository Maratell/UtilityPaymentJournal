using UtilityPaymentJournal.DTO.UtilityProviders;
using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Models.UtilityProviders;

namespace UtilityProviderPaymentJournal.Interface.Mapping
{
    public interface IUtilityProviderMapper
    {
        CreateUtilityProviderDTO ToDto(CreateUtilityProviderViewModel createUtilityProviderViewModel);

        UtilityProviderDTO ToDto(UtilityProvider utility);

        EditUtilityProviderDTO ToDto(EditUtilityProviderViewModel editUtilityProviderViewModel);

        UtilityProvider ToEntity(CreateUtilityProviderDTO createUtilityProviderDto);

        UtilityProviderViewModel ToViewModel(UtilityProviderDTO utilityDto);
    }
}
