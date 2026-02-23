using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IProveedorRepo
{
    Task<Proveedor> FindByIdAsync(int id, bool asNoTracking = false, bool includeEspecilidades = false);
    void Add(Proveedor taller);
    void RemoveRangeEspecialidades(IEnumerable<ProveedorEspecialidad> especialidades);
}

public class ProveedorRepo : IProveedorRepo
{
    private readonly AppDbContext _ctx;
    
    public ProveedorRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Proveedor> FindByIdAsync(int id, bool asNoTracking = false, bool includeEspecilidades = false)
    {
        var query = _ctx.Proveedores.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();
        
        if (includeEspecilidades)
            query = query.Include(e => e.ProveedorEspecialidades);
        
        return await query.FirstOrDefaultAsync(t => t.IdProveedor == id);
    }

    public void Add(Proveedor taller)
    {
        _ctx.Proveedores.Add(taller);
    }

    public void RemoveRangeEspecialidades(IEnumerable<ProveedorEspecialidad> especialidades)
    {
        _ctx.ProveedorEspecialidades.RemoveRange(especialidades);
    }
}