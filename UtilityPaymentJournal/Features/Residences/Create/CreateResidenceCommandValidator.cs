using FluentValidation;

namespace UtilityPaymentJournal.Features.Residences.Create
{
    public class CreateResidenceCommandValidator : AbstractValidator<CreateResidenceCommand>
    {
        public CreateResidenceCommandValidator()
        {
            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Пожалуйста, введите адрес объекта недвижимости")
                .Length(5, 100)
                .WithMessage("Адрес должен содержать от 5 до 100 символов");
        }
    }
}
