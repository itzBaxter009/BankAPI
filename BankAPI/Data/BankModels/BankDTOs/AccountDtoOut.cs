
namespace BankAPI.Data.BankModels
{
    public class AccountDtoOut
    {
        public AccountDtoOut()
        {

        }
        
        public int Id { get; set; }
        public string AccountName { get; set; }
        public string ClientName { get; set; }
        public decimal Balance { get; set; }
        public DateTime RegDate { get; set; }
    }
}
