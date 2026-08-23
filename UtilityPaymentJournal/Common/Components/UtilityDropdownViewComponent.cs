using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Features.Utilities.GetList;

namespace UtilityPaymentJournal.Common.Components
{
    public class UtilityDropdownViewComponent(ISender mediator) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken)
        {
            // отображаем для выбора только активные коммунальные услуги
            GetUtilitiesListQuery query = new GetUtilitiesListQuery { IsActive = true };
            GetUtilitiesListResponse response = await mediator.Send(query, cancellationToken);

            return View(response.Items);
        }
    }
}
