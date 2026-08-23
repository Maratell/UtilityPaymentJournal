using MediatR;

namespace UtilityPaymentJournal.Features.Utilities.Create
{
    /// <summary>
    /// Команда на создание новой услуги
    /// </summary>
    /// <param name="Name">Наименование услуги</param>
    /// <param name="IconClass">Класс иконки Bootstrap Icons для отображения</param>
    /// <param name="IsActive">Статус активности услуги</param>
    public record CreateUtilityCommand(
        string Name, 
        string IconClass,
        bool IsActive = true
    ) : IRequest<CreateUtilityResponse>;
}
