using BankAPI.Data;
using BankAPI.Data.BankModels;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers;

//Ruteo
[ApiController]
[Route("[Controller]")]

public class AccountController : ControllerBase
{
    //para modificar la base de datos
    private readonly MichuBankContext _context;
    public AccountController(MichuBankContext context)
    {
        _context = context;
    }

    //CRUD
    [HttpGet ("GetAll")]
    public IEnumerable<Account> Get()
    {
        return _context.Accounts.ToList();
    }

    [HttpGet("{id}")]
     public ActionResult<Account> GetById(int id)
    {
        var account = _context.Accounts.Find(id);

        if (account is null)
            return NotFound();

        return account;
    }

    [HttpGet("GetByClientId/{ClientId}")]
     public ActionResult<IEnumerable<Account>> GetByClientId(int? ClientId)
    {
        var accounts = _context.Accounts.Where(a => a.ClientId == ClientId).ToList();
        var nullAccounts = _context.Accounts.Where(a => a.ClientId == null).ToList();

        if (ClientId == 0)
            return nullAccounts;
        else
            return accounts;
    }

    [HttpPost]
    public ActionResult Create (Account account)
    {
        //checar si existe 
        var clientExist = _context.Clients.FirstOrDefault(c => c.Id == account.ClientId);

        if (clientExist == null)
            return BadRequest("Ups!, ese cliente no existe"); 

        //agregar en el contexto
        _context.Accounts.Add(account);
        //guardar 
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById) ,new {id = account.Id}, account);
    }

    [HttpPut ("{id}")]
    public IActionResult Update(int id, int clientId, Account account)
    {
        if (id != account.Id)
            return BadRequest("No existe una cuenta con esta id!");
        
        if (clientId != account.ClientId)
            return BadRequest("No existe este cliente!");
        
        var existingaccount = _context.Accounts.Find(id);
        if (existingaccount is null)
            return NotFound();

        //datos que se van a cambiar 
        existingaccount.AccountType = account.AccountType;
        existingaccount.ClientId = account.ClientId;
        existingaccount.Balance = account.Balance;

        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete ("{id}")]
    public IActionResult Delete(int id)
    {
        var existingaccount = _context.Accounts.Find(id);
        if (existingaccount is null)
            return NotFound();

        _context.Accounts.Remove(existingaccount);
        _context.SaveChanges();
        return Ok();
    }
}