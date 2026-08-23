using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UtilityPaymentJournal.Features.Utilities.GetList
{
    /// <summary>
    /// Запрос на получение списка коммунальных услуг с возможностью фильтрации.
    /// </summary>
    public record GetUtilitiesListQuery : IRequest<GetUtilitiesListResponse>
    {
        /// <summary>
        /// Фильтр по статусу активности услуги:
        /// <list type="bullet">
        /// <item><description><c>null</c> — отобразить все услуги (без фильтрации);</description></item>
        /// <item><description><c>true</c> — отобразить только активные услуги;</description></item>
        /// <item><description><c>false</c> — отобразить только неактивные (архивные) услуги.</description></item>
        /// </list>
        /// </summary>
        [FromQuery(Name = "isActive")]
        public bool? IsActive { get; init; } = null;
    }
}
