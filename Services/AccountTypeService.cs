using BankAPI.Data;
using BankAPI.Data.BankModels;

namespace BankAPI.Services;
public class AccountTypeService
{
    private readonly MichuBankContext _context;
   
    public AccountTypeService(MichuBankContext context)
    {
        _context = context;
    }

    public async Task<AccountType> GetById(int id)
    {
        return await _context.AccountTypes.FindAsync(id);
    }
}