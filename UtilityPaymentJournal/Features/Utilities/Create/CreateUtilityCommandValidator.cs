using FluentValidation;

namespace UtilityPaymentJournal.Features.Utilities.Create
{
    public class CreateUtilityCommandValidator : AbstractValidator<CreateUtilityCommand>
    {
        public CreateUtilityCommandValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop) // Остановит проверку дальше, если NotEmpty провалится
                .NotEmpty().WithMessage("Наименование услуги обязательно для заполнения.")
                .MaximumLength(100).WithMessage("Наименование не может превышать 100 символов.")
                .Must(name => !name.StartsWith(" ")).WithMessage("Наименование не должно начинаться с пробела.");

            RuleFor(x => x.IconClass)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Класс иконки обязателен.")
                .MaximumLength(50).WithMessage("Название класса иконки слишком длинное.");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("Статус активности должен быть указан.");
        }
    }
}
