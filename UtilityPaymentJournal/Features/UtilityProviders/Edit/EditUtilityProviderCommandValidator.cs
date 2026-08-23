using FluentValidation;

namespace UtilityPaymentJournal.Features.UtilityProviders.Edit
{
    public class EditUtilityProviderCommandValidator : AbstractValidator<EditUtilityProviderCommand>
    {
        public EditUtilityProviderCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Пожалуйста, введите наименование поставщика коммунальных услуг")
                .Length(5, 100)
                .WithMessage("Наименование должно содержать от 5 до 100 символов");
        }
    }
}
