using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BankAPI.Controllers;

//Ruteo
[AllowAnonymous]
[ApiController]
[Route("Api/[Controller]")]


public class BankTransactionController : ControllerBase
{
    //para modificar la base de datos
    private readonly BankTransactionService bankTransactionService;
    public BankTransactionController(BankTransactionService bankTransactionService)
    {
        this.bankTransactionService = bankTransactionService;
    }

    
    [HttpGet("MisCuentas")]
    //solo el cliente puede hace movimientos bancarios
    [Authorize(Roles = "Client")]
    public async Task<IActionResult> GetAccounts()
    {
        //obteniendo la info de los claims
        var nameClaim = HttpContext.User.FindFirst(ClaimTypes.Name);

        //extraer el nombre
        string name = nameClaim.Value;
        var accounts = await bankTransactionService.GetClientAccountsByClientName(name);
        return Ok(accounts);
    }

    /*[HttpPost("Retiros/{Id}/{TransactionType}")]
    //Solo los clientes pueden retirar dinero de SUS cuentas
    [Authorize(Roles = "Client")]
    {

    }*/
    
}