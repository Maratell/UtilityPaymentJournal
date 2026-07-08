namespace UtilityPaymentJournal.DTO.Account
{
    // Данные для входа
    public class SignInDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
