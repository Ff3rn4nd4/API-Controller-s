using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using BankAPI.Data.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BankAPI.Controllers;

//Ruteo
[ApiController]
[Route("Api/[Controller]")]

public class LoginController: ControllerBase
{
    private readonly LoginService loginService;
    private IConfiguration config;
    public LoginController(LoginService loginService, IConfiguration config)
    {
        this.loginService = loginService;
        this.config = config;
    }

    [HttpPost("Admin/authenticate")]
    public async Task<IActionResult> Login(AdminDto adminDto)
    {
        var admin = await loginService.GetAdmin(adminDto);

        if(admin is null)
            return BadRequest(new { messagge = "Credenciales invalidas."});
        
        string jwtToken = GenerateTokenAdmin(admin);
        
        return Ok( new{ token = jwtToken});
    }

    [HttpPost("Client/authenticate")]
    public async Task<IActionResult> Login(ClientDto clientP)
    {
        var client =  await loginService.GetPassClient(clientP);

        if(client is null)
            return BadRequest(new { messagge = "Credenciales invalidas."});

        string jwtToken = GenerateTokenClient(client);
        
        return Ok( new{ token = jwtToken});
    }

    //generacion de token
    private string GenerateTokenAdmin(Administrator admin)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Name, admin.Name),
            new Claim(ClaimTypes.Email, admin.Email),  
            new Claim("AdminType", admin.AdminType)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value)); 
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var securityToken = new JwtSecurityToken(
                                    claims: claims,
                                    expires: DateTime.Now.AddMinutes(3),
                                    signingCredentials: creds);

        string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }

    private string GenerateTokenClient(Client client)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Role, "Client"),
            new Claim(ClaimTypes.Name, client.Name),
            new Claim(ClaimTypes.Email, client.Email) 
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value)); 
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

        var securityToken = new JwtSecurityToken(
                                    claims: claims,
                                    expires: DateTime.Now.AddMinutes(3),
                                    signingCredentials: creds);

        string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }
}