using System.Security.AccessControl;
using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface ICamionRepo
{
    Task<Camion> GetCamionByIdAsync(int id, bool includeMantenimientos = false, bool asNoTracking = false);
    Task<bool> TieneMantenimetoActualAsync(Camion camion);
    Task<bool> EstaEnViajesAsync(Camion camion);    
    void Add(Camion camion);
    void Remove(Camion camion);
    Task<Camion?> GetAsync(
        Func<IQueryable<Camion>, IQueryable<Camion>> querys
    );
}

public class CamionRepo : ICamionRepo
{
    private readonly  AppDbContext _ctx;
    private ICamionRepo _camionRepoImplementation;

    public CamionRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    public async Task<Camion> GetCamionByIdAsync(int id, bool includeMantenimientos = false, bool asNoTracking = false)
    {
        var query = _ctx.Camiones.AsQueryable();

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (includeMantenimientos)
        {
            query = query.Include(c => c.Mantenimientos);
        }

        return await query.FirstOrDefaultAsync(c => c.IdCamion == id);
    }

    public async Task<bool> EstaEnViajesAsync(Camion camion)
    {
        var existe = await _ctx.Viajes
            .AnyAsync(m => m.IdCamion == camion.IdCamion && m.FechaDescarga == null);
        
        return existe;
    }
    
    public async Task<bool> TieneMantenimetoActualAsync(Camion camion)
    {
        var existe = await _ctx.Mantenimientos
            .AnyAsync(m => m.IdVehiculo == camion.IdCamion && m.FechaSalida == null);
        
        return existe;
    }

    public void Add(Camion camion)
    {
        _ctx.Add(camion);
    }

    public void Remove(Camion camion)
    {
        _ctx.Remove(camion);
    }

    public async Task<Camion?> GetAsync(Func<IQueryable<Camion>, IQueryable<Camion>> querys)
    {
        IQueryable<Camion> query = _ctx.Camiones.AsQueryable();
        
        if (querys != null)
            query = querys(query);
        
        return await query.FirstOrDefaultAsync();
    }
}

public static class CamionQueryExtension
{
    public static IQueryable<Camion> IncludeMantenimientoActual(this IQueryable<Camion> query) =>
        query.Include(c => c.Mantenimientos.Where(m => m.FechaSalida == null));
    
    public static IQueryable<Camion> IncludeCurrentViaje(this IQueryable<Camion> query) => 
        query.Include(c => c.Viajes.Where(m => m.FechaDescarga == null));
    
    public static IQueryable<Camion> WhereEqualsIdCamion(this IQueryable<Camion> query, int  idCamion) => 
        query.Where(c => c.IdCamion == idCamion);
}