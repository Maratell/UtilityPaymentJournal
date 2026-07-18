using UtilityPaymentJournal.EF.Entity.ComplaintBoard;
using UtilityPaymentJournal.Features.Complaints.Commands;
using UtilityPaymentJournal.Features.Complaints.Models;
using UtilityPaymentJournal.Features.Complaints.Queries;

namespace UtilityPaymentJournal.Features.Complaints
{
    public interface IComplaintMapper
    {
        /// <summary>
        /// Преобразовать входящую модель создания во входной ДТО бизнес-логики.
        /// </summary>
        CreateComplaintDto ToDto(CreateComplaintViewModel createViewModel);
        /// <summary>
        /// Преобразовать входящую модель редактирования во входной ДТО бизнес-логики.
        /// </summary>
        EditComplaintDto ToDto(EditComplaintViewModel editViewModel);
        /// <summary>
        /// Преобразовать входящую модель смены статуса в ДТО бизнес-логики.
        /// </summary>
        ChangeComplaintStatusDto ToDto(ChangeComplaintStatusViewModel changeStatusViewModel);
        /// <summary>
        /// Преобразовать входной ДТО создания в доменную сущность для базы данных.
        /// </summary>
        Complaint ToEntity(CreateComplaintDto createDto);
        /// <summary>
        /// Обновить существующую доменную сущность на основе ДТО редактирования.
        /// </summary>
        void UpdateEntity(EditComplaintDto editDto, Complaint entity);
        /// <summary>
        /// Преобразовать сущность после сохранения в плоский ДТО результата команды записи.
        /// </summary>
        ComplaintCommandResultDto ToCommandResultDto(Complaint entity);
        /// <summary>
        /// Преобразовать сущность со всеми Include в ДТО результата запроса чтения.
        /// </summary>
        ComplaintQueryResultDto ToQueryResultDto(Complaint entity);
        /// <summary>
        /// Преобразовать плоский ДТО записи в строго плоскую модель ответа API создания (для POST).
        /// </summary>
        ComplaintCreatedViewModel ToCreatedViewModel(ComplaintCommandResultDto dto);
        /// <summary>
        /// Преобразовать плоский ДТО записи в строго плоскую модель ответа API обновления (для PUT).
        /// </summary>
        ComplaintUpdatedViewModel ToUpdatedViewModel(ComplaintCommandResultDto dto);
        /// <summary>
        /// Преобразовать развернутый ДТО чтения в модель представления для UI (для списков / доски).
        /// </summary>
        ComplaintViewModel ToViewModel(ComplaintQueryResultDto dto);
        /// <summary>
        /// Преобразовать развернутый ДТО чтения в детальную модель представления для UI (для GET деталей).
        /// </summary>
        ComplaintDetailsViewModel ToDetailsViewModel(ComplaintQueryResultDto dto);
    }
}
