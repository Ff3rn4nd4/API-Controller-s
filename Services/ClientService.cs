using BankAPI.Data;
using BankAPI.Data.BankModels;

namespace BankAPI.Services;

public class ClientService
{
    private readonly MichuBankContext _context;

    public ClientService(MichuBankContext context)
    {
        _context = context;
    }

    public IEnumerable<Client> GetAll()
    {
        return _context.Clients.ToList();
    }

    public Client? GetById(int id)
    {
        return _context.Clients.Find(id);
    }

    public Client Create(Client newClient)
    {
        _context.Clients.Add(newClient);
        _context.SaveChanges();

        return newClient;
    } 

    public void Update(int id,Client client)
    {
        var existingclient = GetById(id);

        if (existingclient is not null)
        {
            //datos que se van a cambiar 
            existingclient.Name = client.Name;
            existingclient.PhoneNumber = client.PhoneNumber;
            existingclient.Email = client.Email;

            _context.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        var clientToDelete = GetById(id);

        if (clientToDelete is not null)
        {
             _context.Clients.Remove(clientToDelete);
            _context.SaveChanges();
        }
    }
}
