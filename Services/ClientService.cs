using BankAPI.Data;
using BankAPI.Data.BankModels;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services;

public class ClientService
{
    private readonly MichuBankContext _context;

    public ClientService(MichuBankContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Client>>GetAll()
    {
        //return _context.Clients.ToList();
        return await _context.Clients.ToListAsync();
    }

    public async Task<Client?> GetById(int id)
    {
        return await _context.Clients.FindAsync(id);
    }

    public async Task<Client> Create(Client newClient)
    {
        _context.Clients.Add(newClient);
        await _context.SaveChangesAsync();

        return newClient;
    } 

    public async Task Update (int id,Client client)
    {
        var existingclient = await GetById(id);

        if (existingclient is not null)
        {
            //datos que se van a cambiar 
            existingclient.Name = client.Name;
            existingclient.PhoneNumber = client.PhoneNumber;
            existingclient.Email = client.Email;

            await _context.SaveChangesAsync();
        }
    }

    public async Task Delete(int id)
    {
        var clientToDelete = await GetById(id);

        if (clientToDelete is not null)
        {
             _context.Clients.Remove(clientToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
