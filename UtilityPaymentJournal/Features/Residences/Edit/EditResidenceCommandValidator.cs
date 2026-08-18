using FluentValidation;

namespace UtilityPaymentJournal.Features.Residences.Edit
{
    public class EditResidenceCommandValidator : AbstractValidator<EditResidenceCommand>
    {
        public EditResidenceCommandValidator()
        {
            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Пожалуйста, введите адрес объекта недвижимости")
                .Length(5, 100)
                .WithMessage("Адрес должен содержать от 5 до 100 символов");
        }
    }
}
