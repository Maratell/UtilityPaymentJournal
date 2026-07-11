using System.ComponentModel.DataAnnotations;
using UtilityPaymentJournal.Common.Enumerations;

namespace UtilityPaymentJournal.DTOs.ComplaintBoard
{
    /// <summary>
    /// ДТО для возврата данных о жалобе (ответ API).
    /// </summary>
    /// <param name="Id">Уникальный идентификатор жалобы</param>
    /// <param name="Title">Заголовок или краткая суть жалобы</param>
    /// <param name="Description">Подробное описание проблемы</param>
    /// <param name="UtilityId">Идентификатор связанной коммунальной услуги</param>
    /// <param name="UtilityName">Наименование связанной коммунальной услуги (опционально)</param>
    /// <param name="UtilityIcon">Ссылка на иконку коммунальной услуги (опционально)</param>
    /// <param name="CreatedAt">Дата и время создания записи в системе</param>
    /// <param name="SubmissionDate">Дата и время официальной подачи жалобы (опционально)</param>
    /// <param name="IssueResolutionDate">Дата и время фактического решения проблемы (опционально)</param>
    /// <param name="Status">Текущий статус рассмотрения жалобы</param>
    public record ComplaintDto(
        long Id,
        string Title,
        string Description,
        long UtilityId,
        string? UtilityName,
        string? UtilityIcon,
        DateTime CreatedAt,
        DateTime? SubmissionDate,
        DateTime? IssueResolutionDate,
        ComplaintStatus Status
    );
}
