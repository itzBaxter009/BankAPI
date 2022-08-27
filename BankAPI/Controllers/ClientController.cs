using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;
using Microsoft.AspNetCore.Authorization;

namespace BankAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly ClientService _service;
    public ClientController(ClientService service){
        
        _service = service;
    }

    [HttpGet("getAll")]
    public async Task<IEnumerable<Client>>  Get()
    {
        return await _service.GetAll();
    }

    [HttpGet("getByID/{id}")]
    public async Task<ActionResult<Client>> GetById(int id)
    {
        var client = await _service.GetId(id);
        if(client is null )
            return ClientNotFound(id);
        
        return client;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create (Client client)
    {
        var newClient = await _service.Create(client);

        return CreatedAtAction(nameof(GetById),new { id = newClient.Id}, newClient);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, Client client)
    {
        if(id != client.Id)
            return BadRequest();
        
        var clientToUpdate = await _service.GetId(id);
        
        if(clientToUpdate is not null)
        {
            await _service.Update(id, client);
            return NoContent();
        }
        else
            return ClientNotFound(id);
        
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var clientToDelete = await _service.GetId(id);
        if(clientToDelete is not null)
        {
            await _service.Delete(id);
            return Ok();
        }
        else
            return ClientNotFound(id);
    }

    public NotFoundObjectResult ClientNotFound(int id){
        return NotFound(new {message = $"El cliente con el ID ={id} no esxite." });
    }
}    
