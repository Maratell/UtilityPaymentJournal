using UtilityPaymentJournal.EF.Entity.ElectricityReadings;
using UtilityPaymentJournal.Features.ElectricityReadings.Commands;
using UtilityPaymentJournal.Features.ElectricityReadings.Models;
using UtilityPaymentJournal.Features.ElectricityReadings.Queries;

namespace UtilityPaymentJournal.Features.ElectricityReadings
{
    public interface IElectricityReadingMapper
    {
        /// <summary>
        /// Преобразовать входящую модель создания во входной ДТО бизнес-логики.
        /// </summary>
        CreateElectricityReadingDto ToDto(CreateElectricityReadingViewModel createViewModel);
        /// <summary>
        /// Преобразовать входящую модель редактирования во входной ДТО бизнес-логики.
        /// </summary>
        EditElectricityReadingDto ToDto(EditElectricityReadingViewModel editViewModel);
        /// <summary>
        /// Преобразовать входной ДТО создания в доменную сущность для базы данных.
        /// </summary>
        ElectricityReading ToEntity(CreateElectricityReadingDto createDto);
        /// <summary>
        /// Обновить существующую доменную сущность на основе ДТО редактирования.
        /// </summary>
        void UpdateEntity(EditElectricityReadingDto editDto, ElectricityReading entity);
        /// <summary>
        /// Преобразовать сущность после сохранения в плоский ДТО результата команды записи.
        /// </summary>
        ElectricityReadingCommandResultDto ToCommandResultDto(ElectricityReading entity);
        /// <summary>
        /// Преобразовать сущность со всеми Include в ДТО результата запроса чтения.
        /// </summary>
        ElectricityReadingQueryResultDto ToQueryResultDto(ElectricityReading entity);
        /// <summary>
        /// Преобразовать плоский ДТО записи в строго плоскую модель ответа API создания (для POST).
        /// </summary>
        ElectricityReadingCreatedViewModel ToCreatedViewModel(ElectricityReadingCommandResultDto dto);
        /// <summary>
        /// Преобразовать плоский ДТО записи в строго плоскую модель ответа API обновления (для PUT).
        /// </summary>
        ElectricityReadingUpdatedViewModel ToUpdatedViewModel(ElectricityReadingCommandResultDto dto);
        /// <summary>
        /// Преобразовать развернутый ДТО чтения в детальную модель представления для UI (для GET).
        /// </summary>
        ElectricityReadingDetailsViewModel ToViewModel(ElectricityReadingQueryResultDto dto);
    }
}
