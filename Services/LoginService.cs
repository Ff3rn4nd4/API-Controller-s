using BankAPI.Data;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class LoginService
{
    private readonly MichuBankContext _context;

    public LoginService(MichuBankContext context)
    {
        _context = context;
    }

    public async Task<Administrator?> GetAdmin(AdminDto admin)
    {
        return await _context.Administrators.
                    SingleOrDefaultAsync(x => x.Email == admin.Email && x.Pwd == admin.Pwd);
    } 

    public async Task<Client?> GetPassClient(ClientDto clientP)
    {
        return await _context.Clients.
                    SingleOrDefaultAsync(x => x.Email == clientP.Email && x.Pwd == clientP.Pwd);
    }
}