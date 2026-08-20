using UtilityPaymentJournal.Infrastructure.EF.Entity.Residences;

namespace UtilityPaymentJournal.Features.Residences.GetById
{
    /// <summary>
    /// Методы расширения для маппинга данных фичи получения деталей объекта недвижимости.
    /// </summary>
    public static class GetResidenceByIdMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность недвижимости в объект ответа API в памяти приложения.
        /// </summary>
        /// <param name="entity">Доменная сущеость <see cref="Residence"/>.</param>
        /// <returns>Заполненный ДТО ответа <see cref="GetResidenceByIdResponse"/>.</returns>
        /// <exception cref="ArgumentNullException">Выбрасывается, если доменная сущность равна null.</exception>
        public static GetResidenceByIdResponse ToResponse(this Residence entity)
        {
            ArgumentNullException.ThrowIfNull(entity);

            return new GetResidenceByIdResponse(
                Id: entity.Id,        
                Address: entity.Address
            );
        }
    }
}
