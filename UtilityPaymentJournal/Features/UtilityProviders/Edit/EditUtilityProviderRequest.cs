
namespace UtilityPaymentJournal.Features.UtilityProviders.Edit
{
    /// <summary>
    /// Запрос на редактировании постащика услуг.
    /// </summary>
    /// <param name="Name">Новое наименование постащика услуг.</param>
    public record EditUtilityProviderRequest(string Name)
    {
        public EditUtilityProviderCommand ToCommand(long id) =>
            new EditUtilityProviderCommand(id, Name);
    }
}
