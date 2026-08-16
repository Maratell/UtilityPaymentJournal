namespace UtilityPaymentJournal.Common.Exceptions
{
    public class IdentityValidationException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public IdentityValidationException(IEnumerable<string> errors)
            : base("Произошла ошибка валидации при работе с пользователем.")
        {
            Errors = errors;
        }
    }
}
