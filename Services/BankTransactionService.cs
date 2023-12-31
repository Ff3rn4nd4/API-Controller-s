using System.Security.Claims;
using BankAPI.Data;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class BankTransactionService
{
    private readonly MichuBankContext _context;
    private readonly AccountService accountService;
    public BankTransactionService(MichuBankContext context, AccountService accountService)
    {
        _context = context;
        this.accountService = accountService;
    }

    public async Task<List<BankTransaction>> GetAllById(int AccountId, string name)
    {
        return await _context.BankTransactions
                .Where(a => a.Account.Client.Name == name)
                .ToListAsync();
    }
    
    public async Task<List<AccountDtoOut>> GetClientAccountsByClientName(string name)
    {
        var accounts = await _context.Accounts
            .Where(a => a.Client.Name == name)
            .Select(a => new AccountDtoOut
            {
                Id = a.Id,
                AccountName = a.AccountTypeNavigation.Name,
                AccountType = a.AccountType ,
                ClientName = a.Client.Name,
                ClientId = a.Client.Id,
                Balance = a.Balance,
                RegDate = a.RegDate
            })
            .ToListAsync();

        return accounts;
    }

    public async Task<BankTransaction> CreateRetiro(BankTransactionDtoIn BankTranDto,string name, Account account)
    {
        decimal currentBalance = account.Balance;
        decimal newBalance;

        //Retiro
        if(BankTranDto.TransactionType == 2 || BankTranDto.TransactionType == 4)
        {
            newBalance = currentBalance - BankTranDto.Amount;
        }else return null;

        //Comprobar si no esta en 0's o en negativos 
        if(newBalance <= 0 )
        {
            return null;
        }

        // Actualizar el saldo de la cuenta
        account.Balance = newBalance;
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();


        var newBankTranDto = new BankTransaction();

        newBankTranDto.AccountId = account.Id;
        newBankTranDto.TransactionType = BankTranDto.TransactionType;
        newBankTranDto.Amount = newBalance;

        _context.BankTransactions.Add(newBankTranDto);
        await _context.SaveChangesAsync();

        return newBankTranDto;
    }

    public async Task<BankTransaction> CreateDeposito(BankTransactionDtoIn BankTranDto, Account account)
    {
        decimal currentBalance = account.Balance;
        decimal newBalance;

        //Retiro
        if(BankTranDto.TransactionType == 1 || BankTranDto.TransactionType == 3)
        {
            newBalance = currentBalance + BankTranDto.Amount;
        }else return null;

        // Actualizar el saldo de la cuenta
        account.Balance = newBalance;
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();

        var newBankTranDto = new BankTransaction();

        newBankTranDto.AccountId = account.Id;
        newBankTranDto.TransactionType = BankTranDto.TransactionType;
        newBankTranDto.Amount = newBalance;

        _context.BankTransactions.Add(newBankTranDto);
        await _context.SaveChangesAsync();

        return newBankTranDto;
    }

    public async Task DeleteCuentas(int AccountId)
    {
        var accountDelete = await accountService.GetById(AccountId);

        if (accountDelete is not null)
        {
            _context.Accounts.Remove(accountDelete);
            await _context.SaveChangesAsync();
        }
    }
}