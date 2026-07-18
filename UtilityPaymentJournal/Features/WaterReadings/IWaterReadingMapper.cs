using UtilityPaymentJournal.EF.Entity.WaterReadings;
using UtilityPaymentJournal.Features.WaterReadings.Commands;
using UtilityPaymentJournal.Features.WaterReadings.Models;
using UtilityPaymentJournal.Features.WaterReadings.Queries;

namespace UtilityPaymentJournal.Features.WaterReadings
{
    public interface IWaterReadingMapper
    {
        /// <summary>
        /// Преобразовать входящую модель создания во входной ДТО бизнес-логики.
        /// </summary>
        CreateWaterReadingDto ToDto(CreateWaterReadingViewModel createViewModel);
        /// <summary>
        /// Преобразовать входящую модель редактирования во входной ДТО бизнес-логики.
        /// </summary>
        EditWaterReadingDto ToDto(EditWaterReadingViewModel editViewModel);
        /// <summary>
        /// Преобразовать входной ДТО создания в доменную сущность для базы данных.
        /// </summary>
        WaterReading ToEntity(CreateWaterReadingDto createDto);
        /// <summary>
        /// Обновить существующую доменную сущность на основе ДТО редактирования.
        /// </summary>
        void UpdateEntity(EditWaterReadingDto editDto, WaterReading entity);
        /// <summary>
        /// Преобразовать сущность после сохранения в плоский ДТО результата команды записи.
        /// </summary>
        WaterReadingCommandResultDto ToCommandResultDto(WaterReading entity);
        /// <summary>
        /// Преобразовать сущность со всеми Include в ДТО результата запроса чтения.
        /// </summary>
        WaterReadingQueryResultDto ToQueryResultDto(WaterReading entity);
        /// <summary>
        /// Преобразовать плоский ДТО записи в строго плоскую модель ответа API создания (для POST).
        /// </summary>
        WaterReadingCreatedViewModel ToCreatedViewModel(WaterReadingCommandResultDto dto);
        /// <summary>
        /// Преобразовать плоский ДТО записи в строго плоскую модель ответа API обновления (для PUT).
        /// </summary>
        WaterReadingUpdatedViewModel ToUpdatedViewModel(WaterReadingCommandResultDto dto);
        /// <summary>
        /// Преобразовать развернутый ДТО чтения в детальную модель представления для UI (для GET).
        /// </summary>
        WaterReadingDetailsViewModel ToViewModel(WaterReadingQueryResultDto dto);
    }
}
