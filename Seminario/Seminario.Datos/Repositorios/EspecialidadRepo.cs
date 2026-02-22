using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IEspecialidadRepo
{
    Task<IEnumerable<Especialidad>> GetAll();
    Task<Especialidad> GetByIdAsync(int id, bool asNoTracking = false);
    void Add(Especialidad especialidad);
}

public class EspecialidadRepo : IEspecialidadRepo
{
    private readonly AppDbContext _ctx;
    
    public EspecialidadRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }


    public async Task<IEnumerable<Especialidad>> GetAll()
    {
        return await _ctx.Especialidades.ToListAsync();
    }

    public async Task<Especialidad> GetByIdAsync(int id, bool asNoTracking = false)
    {
        var query = _ctx.Especialidades.AsQueryable();
        
        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.FirstOrDefaultAsync(e => e.IdEspecialidad == id);
    }

    public void Add(Especialidad especialidad)
    {
        _ctx.Especialidades.Add(especialidad);
    }
}