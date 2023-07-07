using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTOs;

namespace BankAPI.Controllers;

//Ruteo
[ApiController]
[Route("Api/[Controller]")]

public class LoginController: ControllerBase
{
    private readonly LoginService loginService;
    public LoginController(LoginService loginService)
    {
        this.loginService = loginService;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Login(AdminDto adminDto)
    {
        var admin = await loginService.GetAdmin(adminDto);

        if(admin is null)
            return BadRequest(new { messagge = "Credenciales invalidas."});
        
        return Ok( new{ token = "some value"});
    }
}