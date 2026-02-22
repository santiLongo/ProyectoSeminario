using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface ITallerRepo
{
    Task<Taller> FindByIdAsync(int id, bool asNoTracking = false, bool includeEspecilidades = false);
    void Add(Taller taller);
    void RemoveRangeEspecialidades(IEnumerable<TallerEspecialidad> especialidades);
}

public class TallerRepo : ITallerRepo
{
    private readonly AppDbContext _ctx;
    
    public TallerRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Taller> FindByIdAsync(int id, bool asNoTracking = false, bool includeEspecilidades = false)
    {
        var query = _ctx.Talleres.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();
        
        if (includeEspecilidades)
            query = query.Include(e => e.TallerEspecialidades);
        
        return await query.FirstOrDefaultAsync(t => t.IdTaller == id);
    }

    public void Add(Taller taller)
    {
        _ctx.Talleres.Add(taller);
    }

    public void RemoveRangeEspecialidades(IEnumerable<TallerEspecialidad> especialidades)
    {
        _ctx.TallerEspecialidades.RemoveRange(especialidades);
    }
}