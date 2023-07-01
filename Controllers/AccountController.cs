using BankAPI.Services;
using BankAPI.Data.BankModels;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

//Ruteo
[ApiController]
[Route("[Controller]")]

public class AccountController : ControllerBase
{
    //para modificar la base de datos
    private readonly AccountService _service;
    public AccountController(AccountService account)
    {
        _service = account;
    }

    //CRUD
    [HttpGet ("GetAll")]
    public IEnumerable<Account> Get()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
     public ActionResult<Account> GetById(int id)
    {
        var account = _service.GetById(id);

        if (account is null)
            return NotFound();

        return account;
    }

    [HttpGet("GetByClientId/{ClientId}")]
     public IEnumerable<Account> GetByClientId(int? ClientId)
    {
        var accounts = _service.GetByClientId(ClientId);

        return accounts;
    }

    [HttpPost]
    public ActionResult Create (Account account)
    {
        var clientExist = _service.ClientExist(account.ClientId);

        if (clientExist == null)
            return BadRequest("Ups!, ese cliente no existe"); 

        var newAccount = _service.Create(account, clientExist);
        return CreatedAtAction(nameof(GetById) ,new {id = newAccount}, newAccount);
    }
    
    [HttpPut ("{id}/{clientId}")]
    public IActionResult Update(int id, int clientId, Account account)
    {
        if (id != account.Id)
            return BadRequest("No existe una cuenta con esta id!");
        
        if (clientId != account.ClientId)
            return BadRequest("No existe este cliente!");
        
        var existingaccount = _service.GetById(account.Id);
        if (existingaccount is null)
            return NotFound();

        var accountToUpdate = _service.GetById(account.Id);

        if(accountToUpdate is not null)
        {
             _service.Update(id, clientId, account);
            return NoContent();
        }else
            {
                return NotFound();
            }
       
    }

    [HttpDelete ("{id}")]
    public IActionResult Delete(int id)
    {
        var existingaccount = _service.GetById(id);
        if (existingaccount is null)
            return NotFound();

        var accountToDelete = _service.GetById(id);
        if (accountToDelete is not null)
        {
            _service.Delete(id);
            return NoContent();
        }else  
            { 
                return NotFound();
            }
       
    }
}