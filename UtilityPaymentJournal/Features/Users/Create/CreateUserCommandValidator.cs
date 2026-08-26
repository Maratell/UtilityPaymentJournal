using FluentValidation;

namespace UtilityPaymentJournal.Features.Users.Create
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Введите логин")
                .MaximumLength(50).WithMessage("Логин не должен превышать 50 символов");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Введите имя пользователя")
                .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Введите фамилию пользователя")
                .MaximumLength(100).WithMessage("Фамилия не должна превышать 100 символов");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Введите пароль")
                .MinimumLength(5).WithMessage("Пароль должен быть не менее 5 символов");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Выберите корректную роль пользователя");
        }
    }
}
