using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IClienteRepo
{
    Task<Cliente> FindByIdAsync(int id, bool asNoTracking = false);

    Task<IEnumerable<Cliente>> GetAllAsync(bool asNoTracking = false);

    void Add(Cliente cliente);

    void RemoveById(Cliente cliente);
}
public class ClienteRepo : IClienteRepo
{
    private readonly AppDbContext _db;

    public ClienteRepo(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Cliente> FindByIdAsync(int id, bool asNoTracking = false)
    {
        if (asNoTracking)
        {
            return await _db.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.IdCliente == id);
        }
        return await _db.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id);
    }

    public async Task<IEnumerable<Cliente>> GetAllAsync(bool asNoTracking = false)
    {
        if (asNoTracking)
        {
            return await _db.Clientes.AsNoTracking().ToListAsync();
        }

        return await _db.Clientes.ToListAsync();

    }

    public void Add(Cliente cliente)
    {
        _db.Clientes.Add(cliente);
    }

    public void RemoveById(Cliente cliente)
    {
        _db.Clientes.Remove(cliente);
    }
}