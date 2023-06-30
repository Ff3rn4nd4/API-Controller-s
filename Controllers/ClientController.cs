using Microsoft.AspNetCore.Mvc;
using BankAPI.Data;
using BankAPI.Data.BankModels;

namespace BankAPI.Controllers;

//Ruteo
[ApiController]
[Route("[Controller]")]

public class ClientController : ControllerBase
{
    //para modificar la base de datos
    private readonly MichuBankContext _context;
    public ClientController(MichuBankContext context)
    {
        _context = context;
    }

    //CRUD
    [HttpGet]
    public IEnumerable<Client> Get()
    {
        return _context.Clients.ToList();
    }

    [HttpGet("{id}")]
     public ActionResult<Client> GetById(int id)
    {
        var client = _context.Clients.Find(id);

        if (client is null)
            return NotFound();

        return client;
    }

    [HttpPost]
    public ActionResult Create (Client client)
    {
        //agregar en el contexto
        _context.Clients.Add(client);
        //guardar 
        _context.SaveChanges();

        //accionDentroControlador+
        return CreatedAtAction(nameof(GetById), new {id = client.Id}, client);
    }
}