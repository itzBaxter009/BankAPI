
namespace BankAPI.Data.BankModels
{
    public class AccountDtoIn
    {
        public AccountDtoIn()
        {

        }
        
        public int Id { get; set; }
        public int AccountType { get; set; }
        public int? ClientId { get; set; }
        public decimal Balance { get; set; }
        public DateTime RegDate { get; set; }
    }
}
