using FluentValidation;

namespace UtilityPaymentJournal.Features.UtilityProviders.Create
{
    public class CreateUtilityProviderCommandValidator : AbstractValidator<CreateUtilityProviderCommand>
    {
        public CreateUtilityProviderCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Пожалуйста, введите наименование поставщика коммунальных услуг")
                .Length(5, 100)
                .WithMessage("Наименование должно содержать от 5 до 100 символов");
        }
    }
}
