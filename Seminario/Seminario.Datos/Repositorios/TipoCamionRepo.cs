using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface ITipoCamionRepo
{
    Task<TipoCamion>  GetByIdAsync(int id, bool asNoTracking = false);
    Task<IEnumerable<TipoCamion>> GetAllAsync(string? tipo);
    void Add(TipoCamion tipoCamion);
    void Remove(TipoCamion tipoCamion);
}

public class TipoCamionRepo : ITipoCamionRepo
{
    private readonly AppDbContext _ctx;
    
    public TipoCamionRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<TipoCamion> GetByIdAsync(int id, bool asNoTracking = false)
    {
        var query = _ctx.TipoCamiones.AsQueryable();
        
        if(asNoTracking) query = query.AsNoTracking();
        
        return await _ctx.TipoCamiones.FirstOrDefaultAsync(t => t.IdTipoCamion == id);
    }

    public async Task<IEnumerable<TipoCamion>> GetAllAsync(string? tipo)
    {
        return await _ctx.TipoCamiones.Where(t => t.Descripcion.Contains(tipo) || tipo == null).ToListAsync();
    }

    public void Add(TipoCamion tipoCamion)
    {
        _ctx.TipoCamiones.Add(tipoCamion);
    }

    public void Remove(TipoCamion tipoCamion)
    {
        _ctx.TipoCamiones.Remove(tipoCamion);
    }
}