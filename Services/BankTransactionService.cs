using System.Security.Claims;
using BankAPI.Data;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class BankTransactionService
{
    private readonly MichuBankContext _context;

    public BankTransactionService(MichuBankContext context)
    {
        _context = context;
    }

    public async Task<IList<AccountDtoOut>> GetClientAccountsByClientName(string name)
    {
        var accounts = await _context.Accounts
            .Where(a => a.Client.Name == name)
            .Select(a => new AccountDtoOut
            {
                Id = a.Id,
                AccountName = a.AccountTypeNavigation.Name ,
                ClientName = a.Client.Name,
                Balance = a.Balance,
                RegDate = a.RegDate
            })
            .ToListAsync();

        return accounts;
    }

}