namespace BankAPI.Models
{
    public class CreateAccountRequest
    {
        public string Owner { get; set; }
        public decimal InitialBalance { get; set; }
    }
}