using MediatR;
using Microsoft.AspNetCore.Mvc;
using UtilityPaymentJournal.Common.Enumerations;
using UtilityPaymentJournal.Features.Complaints.Create;
using UtilityPaymentJournal.Features.Complaints.GetList;

namespace UtilityPaymentJournal.Features.Complaints
{
    [Route("complaints")]
    public class ComplaintsController(ISender mediator) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            // Отправляем запрос в MediatR и получаем плоский список
            GetComplaintsListResponse response = await mediator.Send(new GetComplaintsListQuery(), cancellationToken);

            // Группируем карточки по статусу
            Dictionary<ComplaintStatus, List<GetComplaintsListResponse.Item>> complaintsByStatus = Enum.GetValues<ComplaintStatus>()
                .ToDictionary(
                    status => status,
                    status => response.Items.Where(item => item.Status == status).ToList()
                );

            // Формируем модель представления доски и отпправляем во View
            return View(new ComplaintsBoardViewModel
            {
                ComplaintsByStatus = complaintsByStatus,
                // Инициализируем пустую модель формы. 
                // Она необходима движку Razor для автоматической генерации HTML-атрибутов 
                // и правил валидации внутри модального окна добавления новой жалобы.
                EmptyForm = new CreateComplaintViewModel()
            });
        }
    }
}
