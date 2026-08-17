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
