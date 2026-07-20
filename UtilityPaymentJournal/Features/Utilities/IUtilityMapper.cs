using UtilityPaymentJournal.EF.Entity.Utilities;
using UtilityPaymentJournal.Features.Utilities.Commands;
using UtilityPaymentJournal.Features.Utilities.Models;
using UtilityPaymentJournal.Features.Utilities.Queries;

namespace UtilityPaymentJournal.Features.Utilities
{
    public interface IUtilityMapper
    {
        /// <summary>
        /// Преобразовать входящую модель создания во входной ДТО бизнес-логики.
        /// </summary>
        CreateUtilityDto ToDto(CreateUtilityViewModel createViewModel);
        /// <summary>
        /// Преобразовать входящую модель редактирования во входной ДТО бизнес-логики.
        /// </summary>
        EditUtilityDto ToDto(EditUtilityViewModel editViewModel);
        /// <summary>
        /// Преобразовать входной ДТО создания в доменную сущность для базы данных.
        /// </summary>
        Utility ToEntity(CreateUtilityDto createDto);
        /// <summary>
        /// Обновить существующую доменную сущность на основе ДТО редактирования.
        /// </summary>
        void UpdateEntity(EditUtilityDto editDto, Utility entity);
        /// <summary>
        /// Преобразовать сущность после сохранения в плоский ДТО результата команды записи.
        /// </summary>
        UtilityCommandResultDto ToCommandResultDto(Utility entity);
        /// <summary>
        /// Преобразовать сущность в ДТО результата запроса чтения.
        /// </summary>
        UtilityQueryResultDto ToQueryResultDto(Utility entity);
        /// <summary>
        /// Преобразовать плоский ДТО записи в строго плоскую модель ответа API создания (для POST).
        /// </summary>
        UtilityCreatedViewModel ToCreatedViewModel(UtilityCommandResultDto dto);
        /// <summary>
        /// Преобразовать плоский ДТО записи в строго плоскую модель ответа API обновления (для PUT).
        /// </summary>
        UtilityUpdatedViewModel ToUpdatedViewModel(UtilityCommandResultDto dto);
        /// <summary>
        /// Преобразовать развернутый ДТО чтения в детальную модель представления для UI (для GET).
        /// </summary>
        UtilityDetailsViewModel ToViewModel(UtilityQueryResultDto dto);
    }
}
