using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;

namespace BankAPI.Controllers;

//Ruteo
[ApiController]
[Route("[Controller]")]

public class ClientController : ControllerBase
{
    //para modificar la base de datos
    private readonly ClientService _service;
    public ClientController(ClientService client)
    {
        _service = client;
    }

    //CRUD
    [HttpGet]
    public IEnumerable<Client> Get()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
     public ActionResult<Client> GetById(int id)
    {
        var client = _service.GetById(id);

        if (client is null)
            return NotFound();

        return client;
    }

    [HttpPost]
    public ActionResult Create (Client client)
    {
       var newClient = _service.Create(client);
        //accionDentroControlador+
        return CreatedAtAction(nameof(GetById), new {id = newClient.Id}, newClient);
    }

    [HttpPut ("{id}")]
    public IActionResult Update(int id, Client client)
    {
        if (id != client.Id)
            return BadRequest();
        
        var existingclient = _service.GetById(client.Id);
        
        if (existingclient is null)
            return NotFound();

        var clientToUpdate = _service.GetById(id);

        if(clientToUpdate is not null)
        {
            _service.Update(id, client);
            return NoContent();
        }else 
            {
                return NotFound();
            } 
    }

    [HttpDelete ("{id}")]
    public IActionResult Delete(int id)
    {
        var existingclient = _service.GetById(id);
        if (existingclient is null)
            return NotFound();
        /*
        _context.Clients.Remove(existingclient);
        _context.SaveChanges();
        return Ok();*/

        var clientToDelete = _service.GetById(id);

        if(clientToDelete is not null)
        {
            _service.Delete(id);
            return NoContent();
        }else 
            {
                return NotFound();
            } 
    }
}