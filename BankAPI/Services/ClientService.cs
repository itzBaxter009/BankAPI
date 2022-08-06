using BankAPI.Data;
using BankAPI.Data.BankModels;


namespace BankAPI.Services;

public class ClientService{
    private readonly BankContext _context;
    public ClientService(BankContext context)
    {
        _context=context;
    }

    public IEnumerable<Client> GetAll()
    {
        return _context.Clients.ToList();
    }

    public Client? GetId(int id)
    {
        return _context.Clients.Find(id);
    }

    public Client Create(Client newClient)
    {
        _context.Clients.Add(newClient);
        _context.SaveChanges();

        return newClient;
    }

    public void Update(int id, Client client)
    {
        var existingClient=GetId(id);

        if(existingClient is not null)
        {
            existingClient.Name = client.Name;
            existingClient.PhoneNumber = client.PhoneNumber;
            existingClient.Email = client.Email;

            _context.SaveChanges();
        }
        
    }

    public void Delete(int id)
    {
        var ClientToDelete=GetId(id);

        if(ClientToDelete is not null)
        {
            _context.Clients.Remove(ClientToDelete);
            _context.SaveChanges();
        }
        
    }
}