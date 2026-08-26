using FluentValidation;

namespace UtilityPaymentJournal.Features.ComplaintBoard.Create
{
    public class CreateComplaintCommandValidator : AbstractValidator<CreateComplaintCommand>
    {
        public CreateComplaintCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Заголовок обязателен для заполнения")
                .MaximumLength(100)
                .WithMessage("Заголовок не должен превышать 100 символов");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Пожалуйста, опишите суть проблемы");

            RuleFor(x => x.UtilityId)
                .NotNull()
                .WithMessage("Выберите услугу");

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("Выбран некорректный статус жалобы.");
        }
    }
}
