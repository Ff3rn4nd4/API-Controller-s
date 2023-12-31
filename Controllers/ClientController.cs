using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using Microsoft.AspNetCore.Authorization;

namespace BankAPI.Controllers;

//Ruteo
//[Authorize]
[ApiController]
[Route("Api/[Controller]")]
//solo el admin puede controlar dos datos de los clientes
[Authorize(Roles = "Admin")]

public class ClientController : ControllerBase
{
    //para modificar la base de datos
    private readonly ClientService _service;
    public ClientController(ClientService client)
    {
        _service = client;
    }

    //CRUD
    [HttpGet("GetAll")]
    public async Task<IEnumerable<Client>> Get()
    {
        return await _service.GetAll();
    }

    [HttpGet("{id}")]
     public async Task<ActionResult<Client>> GetById(int id)
    {
        var client = await _service.GetById(id);

        if (client is null)
            //enviar mensajes de error pero personalizados 
            //return NotFound(new { message = $"Ups!, el cliente con este ID = {id} no existe!." });
            return ClientNotFound(id);

        return client;
    }

    [HttpPost("Create")]
    public async Task<ActionResult> Create (Client client)
    {
       var newClient = await _service.Create(client);
        //accionDentroControlador+
        return CreatedAtAction(nameof(GetById), new {id = newClient.Id}, newClient);
    }

    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, Client client)
    {
        if (id != client.Id)
            return BadRequest(new { message = $"Ups!, el ID({id}) de la URL no coincide con el ID({client.Id}) del cuerpo de la solicitud!." });
        
        var existingclient = await _service.GetById(client.Id);
        
        if (existingclient is null)
            return ClientNotFound(id);

        var clientToUpdate = _service.GetById(id);

        if(clientToUpdate is not null)
        {
            await _service.Update(id, client);
            return NoContent();
        }else 
            {
                return ClientNotFound(id);
            } 
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingclient = await _service.GetById(id);
        if (existingclient is null)
            return ClientNotFound(id);
        /*
        _context.Clients.Remove(existingclient);
        _context.SaveChanges();
        return Ok();*/

        var clientToDelete = _service.GetById(id);

        if(clientToDelete is not null)
        {
            await _service.Delete(id);
            return NoContent();
        }else 
            {
                return NotFound();
            } 
    }

    //Esta linea es solo para que el swagger no tenga problema para abrirse
    //Ya que al no tener una etiqueta lo marcaba como un metodo ambiguo
    [HttpGet("ClienteNoSeEncontro")]
    public NotFoundObjectResult ClientNotFound(int id)
    {
        return NotFound(new { message = $"Ups!, el cliente con este ID = {id} no existe!." });
    }
}