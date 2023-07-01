using BankAPI.Data;
using BankAPI.Data.BankModels;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class AccountService
{
    private readonly MichuBankContext _context;
    
    public AccountService(MichuBankContext context)
    {
        _context = context;
    }

    public IEnumerable<Account> GetAll()
    {
        return _context.Accounts.ToList();
    }

    public Account GetById(int id)
    {
        return _context.Accounts.Find(id);
    }

    public IEnumerable<Account> GetByClientId(int? ClientId)
    {
        if(ClientId == 0)
        {
            return _context.Accounts.Where(a => a.ClientId == null).ToList();
        }else
            {
                return _context.Accounts.Where(a => a.ClientId == ClientId).ToList();
            }

    }

    public Client ClientExist(int? ClientId)
    {
        return _context.Clients.FirstOrDefault(c => c.Id == ClientId);
    }

    public Account Create(Account newAccount, Client client)
    {
        newAccount.Client = client;

        _context.Accounts.Add(newAccount);
        _context.SaveChanges();

        return newAccount;
    } 

    public void Update(int id, int clientId, Account account)
    {
        var existingaccount = GetById(account.Id);
        
        if(existingaccount is not null)
        {
            //datos que se van a cambiar 
            existingaccount.AccountType = account.AccountType;
            existingaccount.ClientId = account.ClientId;
            existingaccount.Balance = account.Balance;

            _context.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        var accountDelete = GetById(id);

        if (accountDelete is not null)
        {
             _context.Accounts.Remove(accountDelete);
            _context.SaveChanges();
        }
    }
}