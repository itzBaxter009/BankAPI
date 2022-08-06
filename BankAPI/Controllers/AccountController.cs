using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;

namespace BankAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase{
    private readonly AccountService _service;
    private readonly ClientService _serviceClient;
    public AccountController(AccountService service, ClientService clientService)
    {
        _service = service;
        _serviceClient=clientService;
        
    }

    [HttpGet]
    public IEnumerable<Account> Get()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Account> GetById(int id)
    {
        var account = _service.GetById(id);
        if(account is null )
            return NotFound();
        
        return account;
    }

    public ClientService Get_serviceClient()
    {
        return _serviceClient;
    }

    [HttpPost]
    public IActionResult Create (AccountModel account)
    {
        
        if(account.ClientId is not null)
       {
            int x=(int)account.ClientId;
            var IdClientBrowser=_serviceClient.GetId(x);
            if (IdClientBrowser is null)
                 return BadRequest();
        }
       
        _service.Create(account);
        return Ok();
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, AccountModel account)
    {
        if(id != account.Id)
            return BadRequest();
        
        var accountToUpdate = _service.GetById(id);
        
        if(accountToUpdate is not null)
        {
            if(accountToUpdate.ClientId != account.ClientId)
            {
                int x=(int)account.ClientId;
                var IdClientBrowser=_serviceClient.GetId(x);
                if (IdClientBrowser is null)
                    return BadRequest();
               
            }
            _service.Update(id, account);
            return NoContent();
        }
        else
            return NotFound();
        
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var accountToDelete = _service.GetById(id);
        if(accountToDelete is not null)
        {
            _service.Delete(id);
            return Ok();
        }
        else
            return NotFound();
    }
    
}