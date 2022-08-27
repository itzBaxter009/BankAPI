using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BankAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginClientController : ControllerBase
{

    private readonly LoginClientService loginClientService;
    private IConfiguration config;
    public LoginClientController(LoginClientService loginClientService, IConfiguration config)
    {
        this.loginClientService=loginClientService;
        this.config=config;
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> loginClient (LoginClientDto loginClientDto)
    {
        var client = await loginClientService.GetClient(loginClientDto);

        if(client is null)
            return BadRequest(new{ message = "Credenciales invalidas"});

        string jwtToken = GenerateToken(client);

        return Ok(new { token = jwtToken});
    }

    private string GenerateToken (Client client)
    {
        var claims=new[]
        {
           // new Claim(ClaimTypes.Role, "Cliente"),
            new Claim(ClaimTypes.Name, client.Name),
            new Claim(ClaimTypes.Email, client.Email)
        };

        var key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
        var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

        var securityToken=new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds);

        var token= new JwtSecurityTokenHandler().WriteToken(securityToken);

        return token;
    }
}