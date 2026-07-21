using UtilityPaymentJournal.EF.Entity.Residences;
using UtilityPaymentJournal.Features.Residences.Commands;
using UtilityPaymentJournal.Features.Residences.Models;
using UtilityPaymentJournal.Features.Residences.Queries;

namespace UtilityPaymentJournal.Features.Residences
{
    /// <summary>
    /// Интерфейс маппера для преобразования моделей данных объекта недвижимости между слоями.
    /// </summary>
    public interface IResidenceMapper
    {
        /// <summary>
        /// Преобразовать входящую модель создания во входной ДТО бизнес-логики.
        /// </summary>
        CreateResidenceDto ToDto(CreateResidenceViewModel createViewModel);
        /// <summary>
        /// Преобразовать входящую модель редактирования во входной ДТО бизнес-логики.
        /// </summary>
        EditResidenceDto ToDto(EditResidenceViewModel editViewModel);
        /// <summary>
        /// Преобразовать входной ДТО создания в доменную сущность для базы данных.
        /// </summary>
        Residence ToEntity(CreateResidenceDto createDto);
        /// <summary>
        /// Обновить существующую доменную сущность на основе ДТО редактирования.
        /// </summary>
        void UpdateEntity(EditResidenceDto editDto, Residence entity);
        /// <summary>
        /// Преобразовать сущность после сохранения в плоский ДТО результата команды записи.
        /// </summary>
        ResidenceCommandResultDto ToCommandResultDto(Residence entity);
        /// <summary>
        /// Преобразовать сущность в ДТО результата запроса чтения.
        /// </summary>
        ResidenceQueryResultDto ToQueryResultDto(Residence entity);
        /// <summary>
        /// Преобразовать плоский ДТО записи в модель ответа API создания (для POST).
        /// </summary>
        ResidenceCreatedViewModel ToCreatedViewModel(ResidenceCommandResultDto dto);
        /// <summary>
        /// Преобразовать плоский ДТО записи в модель ответа API обновления (для PUT).
        /// </summary>
        ResidenceUpdatedViewModel ToUpdatedViewModel(ResidenceCommandResultDto dto);
        /// <summary>
        /// Преобразовать ДТО чтения в детальную модель представления для UI (для GET).
        /// </summary>
        ResidenceDetailsViewModel ToViewModel(ResidenceQueryResultDto dto);
    }
}
