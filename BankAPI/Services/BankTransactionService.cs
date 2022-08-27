using Microsoft.EntityFrameworkCore;
using BankAPI.Data;
using BankAPI.Data.BankModels;

namespace BankAPI.Services;

public class BankTransactionService{
    private readonly BankContext _context;
    public BankTransactionService(BankContext context)
    {
        _context=context;
    }

     public async Task<IEnumerable<Account>> GetAccounts(Client obj)
    {
        var client = _context.Clients.Where(a => a.Name.Equals(obj.Name)  & a.Email.Equals(obj.Email))
            .Select(a => new Client{
                Id=a.Id                
            })
            .SingleOrDefault();
        return await _context.Accounts.
        Where(a => a.ClientId == client.Id).
        Select(a => new Account{
            Id = a.Id,
            AccountType = a.AccountType,
            ClientId = a.ClientId,
            Balance = a.Balance,
            RegDate = a.RegDate
        }).ToListAsync();
    }

     public async Task<BankTransaction> withdrawCash(Client obj, BankTransactionCashDto bankTransactionCashDto)
    {
        //creamos objeto BankTransaction para vaciar el dto
        BankTransaction bankTransaction= new BankTransaction();
        //vaciamos el contenido del dto en el objeto
        bankTransaction.AccountId=bankTransactionCashDto.AccountId;
        bankTransaction.TransactionType=2;
        bankTransaction.Amount=bankTransactionCashDto.Amount;

        var account= _context.Accounts.Where( a => a.Id ==bankTransactionCashDto.AccountId && a.ClientId==obj.Id).Single();
        if(account is not null)
            account.Balance=(account.Balance - bankTransactionCashDto.Amount);
        //Console.WriteLine(account.)
        //Update(a => a.Balance= (a.Balance-bankTransactionCashDto.Amount));

        _context.BankTransactions.Add(bankTransaction);
       // _context.Accounts.Where(a => a.Id = bankTransaction.AccountId).Update(a => a.Balance = bankTransactionCashDto))
        account= _context.Accounts.Where( a => a.Id ==bankTransactionCashDto.AccountId && a.ClientId==obj.Id).Single();
        Console.WriteLine(account.Balance);
        await _context.SaveChangesAsync();
        

        return bankTransaction;
        
    }
}