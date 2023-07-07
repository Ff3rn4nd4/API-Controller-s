using BankAPI.Data;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class AccountService
{
    private readonly MichuBankContext _context;
    
    public AccountService(MichuBankContext context)
    {
        _context = context;
    }

    public  async Task<IEnumerable<AccountDtoOut>> GetAll()
    {
        return await _context.Accounts.Select(a => new AccountDtoOut
        {
            Id = a.Id,
            AccountName = a.AccountTypeNavigation.Name,
            ClientName = a.Client.Name,
            Balance = a.Balance,
            RegDate = a.RegDate
        }).ToListAsync();
    }

    public async Task<Account> GetById(int id)
    {
        return await _context.Accounts.FindAsync(id);
    }

     public  async Task<AccountDtoOut?> GetDtoById(int id)
    {
        return await _context.Accounts.
        Where(a => a.Id == id).
        Select(a => new AccountDtoOut
        {
            Id = a.Id,
            AccountName = a.AccountTypeNavigation.Name,
            ClientName = a.Client.Name,
            Balance = a.Balance,
            RegDate = a.RegDate
        }).SingleOrDefaultAsync();
    }

    public  async Task<IEnumerable<Account>> GetByClientId(int? ClientId)
    {
        if(ClientId == 0)
        {
            return await _context.Accounts.Where(a => a.ClientId == null).ToListAsync();
        }else
            {
                return await _context.Accounts.Where(a => a.ClientId == ClientId).ToListAsync();
            }

    }

    /*public  async Task<Client> ClientExist(int? ClientId)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.Id == ClientId);
    }*/

    public async Task<Account> Create(AccountDtoIn newAccountDto)
    {
        var newAccount = new Account();
        //newAccount.Client = client;

        newAccount.AccountType = newAccountDto.AccountType;
        newAccount.ClientId = newAccountDto.ClientId;
        newAccount.Balance = newAccountDto.Balance;

        _context.Accounts.Add(newAccount);
        await _context.SaveChangesAsync();

        return newAccount;
    } 

    public async Task Update(AccountDtoIn account)
    {
        var existingaccount = await GetById(account.Id);
        
        if(existingaccount is not null)
        {
            //datos que se van a cambiar 
            existingaccount.AccountType = account.AccountType;
            existingaccount.ClientId = account.ClientId;
            existingaccount.Balance = account.Balance;

            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(int id)
    {
        var accountDelete = await GetById(id);

        if (accountDelete is not null)
        {
            _context.Accounts.Remove(accountDelete);
            await _context.SaveChangesAsync();
        }
    }
}