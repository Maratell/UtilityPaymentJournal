
namespace UtilityPaymentJournal.Features.Utilities.Edit
{
    /// <summary>
    /// Запрос на редактировании услуги.
    /// </summary>
    /// <param name="Name">Новое наименование услуги.</param>
    /// <param name="IconClass">Новый класс иконки Bootstrap Icons</param>
    /// <param name="IsActive">Новый статус активности услуги</param>
    public record EditUtilityRequest(
        string Name,
        string IconClass,
        bool IsActive
    )
    {
        public EditUtilityCommand ToCommand(long id) =>
            new EditUtilityCommand(
                id, 
                Name,
                IconClass,
                IsActive
            );
    }
}
