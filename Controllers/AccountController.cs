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
    private readonly AccountService accountService;
    private readonly AccountTypeService accountTypeService;

    public AccountController(AccountService accountService, AccountTypeService accountTypeService)
    {
        this.accountService = accountService;
        this.accountTypeService = accountTypeService;
    }

    //CRUD
    [HttpGet ("GetAll")]
    public async Task<IEnumerable<Account>> Get()
    {
        return await accountService.GetAll();
    }

    [HttpGet("{id}")]
     public async Task<ActionResult<Account>> GetById(int id)
    {
        var account = await accountService.GetById(id);

        if (account is null)
            return NotFound();

        return account;
    }

    [HttpGet("GetByClientId/{ClientId}")]
     public async Task<IEnumerable<Account>> GetByClientId(int? ClientId)
    {
        var accounts = await accountService.GetByClientId(ClientId);

        return accounts;
    }

    [HttpPost]
    public async Task<ActionResult> Create (Account account)
    {
        var clientExist = await accountService.ClientExist(account.ClientId);

        if (clientExist == null)
            return BadRequest("Ups!, ese cliente no existe");

        string validationResult = await ValidationAccount(account);

        if(!validationResult.Equals("Valid"))
            return BadRequest(new{message = validationResult});

        var newAccount =  accountService.Create(account, clientExist);
        return CreatedAtAction(nameof(GetById) ,new {id = newAccount}, newAccount);
    }

    public async Task<string> ValidationAccount(Account account)
    {
        string result = "Valid";

        var accountType = await accountTypeService.GetById(account.AccountType);

        if(accountType is null)
            result = $"El tipo de cuenta {account.AccountType} no existe";
    
        return result;
    }

    [HttpPut ("{id}/{clientId}")]
    public async Task<IActionResult> Update(int id, int clientId, Account account)
    {
        if (id != account.Id)
            return BadRequest("No existe una cuenta con esta id!");
        
        if (clientId != account.ClientId)
            return BadRequest("No existe este cliente!");
        
        var existingaccount = accountService.GetById(account.Id);
        if (existingaccount is null)
            return NotFound();

        var accountToUpdate =  await accountService.GetById(account.Id);

        if(accountToUpdate is not null)
        {
            await accountService.Update(id, clientId, account);
            return NoContent();
        }else
            {
                return NotFound();
            }
       
    }

    [HttpDelete ("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var existingaccount = await accountService.GetById(id);
        if (existingaccount is null)
            return NotFound();

        var accountToDelete = accountService.GetById(id);
        if (accountToDelete is not null)
        {
            await accountService.Delete(id);
            return NoContent();
        }else  
            { 
                return NotFound();
            }
       
    }
}