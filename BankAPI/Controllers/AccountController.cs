using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using Microsoft.AspNetCore.Authorization;

namespace BankAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase{
    private readonly AccountService _service;
    private readonly ClientService _serviceClient;
    public AccountController(AccountService service, ClientService clientService)
    {
        _service = service;
        _serviceClient=clientService;
        
    }

    [HttpGet("getAll")]
    public async Task<IEnumerable<AccountDtoOut>> GetAll()
    {
        return await _service.GetAll();
    }

    [HttpGet("getByID/{id}")]
    public async Task<ActionResult<AccountDtoOut>> GetById(int id)
    {
        var account = await _service.GetDtoById(id);
        if(account is null )
            return AccountNotFound(id);
        
        return account;
    }

    /*public ClientService Get_serviceClient()
    {
        return _serviceClient;
    }*/

    [HttpPost("create")]
    public async Task<IActionResult> Create (AccountDtoIn account)
    {
        
        if(account.ClientId is not null)
       {
            int x=(int)account.ClientId;
            var IdClientBrowser= await _serviceClient.GetId(x);
            if (IdClientBrowser is null)
                 return BadRequest();
        }
       
        await _service.Create(account);
        return Ok();
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, AccountDtoIn account)
    {
        if(id != account.Id)
            return BadRequest();
        
        var accountToUpdate = await _service.GetById(id);
        
        if(accountToUpdate is not null)
        {
            if(accountToUpdate.ClientId != account.ClientId)
            {
                int x=account.ClientId.GetValueOrDefault();
                var IdClientBrowser = await _serviceClient.GetId(x);
                if (IdClientBrowser is null)
                    return BadRequest();
               
            }
            await _service.Update(id, account);
            return NoContent();
        }
        else
            return AccountNotFound(id);
        
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var accountToDelete = await _service.GetById(id);
        if(accountToDelete is not null)
        {
            await _service.Delete(id);
            return Ok();
        }
        else
            return AccountNotFound(id);
    }

     public NotFoundObjectResult AccountNotFound(int id){
        return NotFound(new {message = $"La cuenta con el ID ={id} no esxite." });
    }
    
}