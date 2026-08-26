using FluentValidation;

namespace UtilityPaymentJournal.Features.Account.SignIn
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("Введите логин");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Введите пароль");
        }
    }
}
