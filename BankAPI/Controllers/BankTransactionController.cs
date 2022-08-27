using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankAPI.Controllers;

//[Authorize]
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BankTransactionController : ControllerBase
{
    private readonly BankTransactionService _service;
    public BankTransactionController(BankTransactionService service)
    {
        _service=service;
    }

    [HttpGet("getAccounts")]
    public async Task<IEnumerable<Account>>  GetAccounts()
    {

        var currentUser = GetCurrentUser();
        var accounts = await _service.GetAccounts(currentUser);
        //if(accounts is null )
        //   return (IEnumerable<Account>)AccountsNotFound(currentUser.Name);
        
        return accounts;
    }

    [HttpPost("retiroEfectivo")]
    public async Task<IActionResult> WithdrawCash (BankTransactionCashDto bankTransaction)
    {
        var currentUser = GetCurrentUser();
        await _service.withdrawCash(currentUser, bankTransaction);
        return NoContent();
    }


   /* [HttpGet]
    public IActionResult Imprimirvalores()
    {
        var currentUser = GetCurrentUser();

        return Ok($"Hi {currentUser.Name}, email = {currentUser.Email}, eres {currentUser.Id}");
    } 

    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok("Hi,you're one public property");
    }
    */
    private Client GetCurrentUser(){
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        if(identity != null)
        {
            var userClaims = identity.Claims;
            
            return new Client
            {
                Name = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Name)?.Value,
                Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value
                //Role =userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
            }; 
        }
        return null;
    }

     public NotFoundObjectResult AccountsNotFound(string nombre){
        return NotFound(new {message = $"El usuario {nombre} no tiene cuentas" });
    }
}