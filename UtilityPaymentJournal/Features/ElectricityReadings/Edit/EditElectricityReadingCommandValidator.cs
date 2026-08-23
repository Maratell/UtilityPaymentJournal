using FluentValidation;

namespace UtilityPaymentJournal.Features.ElectricityReadings.Edit
{
    public class EditElectricityReadingCommandValidator : AbstractValidator<EditElectricityReadingCommand>
    {
        public EditElectricityReadingCommandValidator()
        {
            // Валидация идентификаторов объектов
            RuleFor(x => x.ResidenceId)
                .NotEmpty().WithMessage("Пожалуйста, выберите жилой объект")
                .GreaterThan(0).WithMessage("Некорректный ID жилого объекта");

            RuleFor(x => x.UtilityProviderId)
                .NotEmpty().WithMessage("Пожалуйста, выберите поставщика услуг")
                .GreaterThan(0).WithMessage("Некорректный ID поставщика услуг");

            // Валидация дат (проверка минимального порога)
            RuleFor(x => x.SubmissionDate)
                .GreaterThanOrEqualTo(new DateTime(2020, 1, 1))
                .WithMessage("Указана слишком старая или некорректная дата подачи показаний")
                // Применяем правила только если дата передана
                .When(x => x.SubmissionDate.HasValue);

            RuleFor(x => x.PaymentDate)
                .GreaterThanOrEqualTo(new DateTime(2020, 1, 1))
                .WithMessage("Указана слишком старая или некорректная дата оплаты")
                // Применяем правила только если дата передана
                .When(x => x.PaymentDate.HasValue);

            // Валидация числовых значений показаний
            RuleFor(x => x.CurrentValue)
                .GreaterThanOrEqualTo(0).WithMessage("Текущее показание должно быть целым положительным числом");

            RuleFor(x => x.PreviousValue)
                .GreaterThanOrEqualTo(0).WithMessage("Показание за прошлый период должно быть целым положительным числом");

            RuleFor(x => x.ResultValue)
                .GreaterThanOrEqualTo(0).WithMessage("Результирующее показание должно быть целым положительным числом");

            // Кросс-валидацию пока отлючил
            //// 5. Кросс-валидация (бизнес-логика)
            //// Проверяем, что текущие показания не меньше прошлых
            //RuleFor(x => x.CurrentValue)
            //    .GreaterThanOrEqualTo(x => x.PreviousValue)
            //    .WithMessage("Текущее показание не может быть меньше показания за прошлый период");

            //// Проверяем правильность математического расчета разницы
            //RuleFor(x => x.ResultValue)
            //    .Equal(x => x.CurrentValue - x.PreviousValue)
            //    .WithMessage("Итоговый расход (разница) рассчитан неверно");

            // Валидация суммы платежа
            RuleFor(x => x.PaymentAmount)
                .InclusiveBetween(0.00m, 999999.99m)
                .WithMessage("Сумма должна быть положительным числом в диапазоне от 0 до 999999.99");
        }
    }
}
