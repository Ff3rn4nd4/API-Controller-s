using BankAPI.Services;
using BankAPI.Data.BankModels;
using Microsoft.AspNetCore.Mvc;
using BankAPI.Data.DTOs;

namespace BankAPI.Controllers;

//Ruteo
[ApiController]
[Route("[Controller]")]

public class AccountController : ControllerBase
{
    //para modificar la base de datos
    private readonly AccountService accountService;
    private readonly AccountTypeService accountTypeService;
    private readonly ClientService clientService;

    public AccountController(AccountService accountService, AccountTypeService accountTypeService, ClientService clientService)
    {
        this.accountService = accountService;
        this.accountTypeService = accountTypeService;
        this.clientService = clientService;
    }

    //CRUD
    [HttpGet ("GetAll")]
    public async Task<IEnumerable<AccountDtoOut>> Get()
    {
        return await accountService.GetAll();
    }

    [HttpGet("{id}")]
     public async Task<ActionResult<AccountDtoOut>> GetById(int id)
    {
        var account = await accountService.GetDtoById(id);

        if (account is null)
            return HandleNotFound(id);

        return account;
    }

    [HttpGet("GetByClientId/{ClientId}")]
     public async Task<IEnumerable<Account>> GetByClientId(int? ClientId)
    {
        var accounts = await accountService.GetByClientId(ClientId);

        return accounts;
    }

    [HttpPost]
    public async Task<IActionResult> Create (AccountDtoIn account)
    {
        /*var clientExist = await accountService.ClientExist(account.ClientId);

        if (clientExist == null)
            return BadRequest("Ups!, ese cliente no existe");*/

        string validationResult = await ValidationAccount(account);

        if(!validationResult.Equals("Valid"))
            return BadRequest(new { message = validationResult });

        var newAccount = await accountService.Create(account);
        return CreatedAtAction(nameof(GetById) ,new {id = newAccount.Id}, newAccount);
    }

    [HttpPut ("{id}/{clientId}")]
    public async Task<IActionResult> Update(int id, AccountDtoIn account)
    {
        /*if (id != account.Id)
            return BadRequest(new{message = $"Ups!, el Id {id} de la URL no coincide con el ID({account.Id}) del cuerpo de la solicitud"});
        
        if (clientId != account.ClientId)
            return BadRequest("No existe este cliente!");
        
        var existingaccount = accountService.GetById(account.Id);
        if (existingaccount is null)
            return AccountNotFound(id);*/

        var accountToUpdate =  await accountService.GetById(account.Id);

        if(accountToUpdate is not null)
        {
            string validationResult = await ValidationAccount(account);

            if(!validationResult.Equals("Valid"))
                return BadRequest(new { message= validationResult } );
            
            await accountService.Update(account);
            return NoContent();
        }else
            {
                return HandleNotFound(id);
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

    public NotFoundObjectResult HandleNotFound(int id)
    {
        return NotFound(new{message = $"La cuenta con ese Id={id} no existe"});
    }

    public async Task<string> ValidationAccount(AccountDtoIn account)
    {
        string result = "Valid";

        var accountType = await accountTypeService.GetById(account.AccountType);

        if(accountType is null)
            result = $"El tipo de cuenta {account.AccountType} no existe";
        
        var clientID = account.ClientId.GetValueOrDefault();

        var client = await clientService.GetById(clientID);

        if(client is null)
            result = $"El cliente {clientID} no existe";
    
        return result;
    }
}