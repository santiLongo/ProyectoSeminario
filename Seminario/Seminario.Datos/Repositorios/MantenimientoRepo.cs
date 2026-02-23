using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IMantenimientoRepo
{
    Task<Mantenimiento> UltimoMantenimientoCamion(int idCamion);
    Task<Mantenimiento> FindByIdAsync(int id, bool asNoTracking = false, bool includeTarea = false, bool includeObs = false,
        bool includeCamion = false, bool includeTaller = false);
    void Add(Mantenimiento mantenimiento);
    void Remove(Mantenimiento mantenimiento);
    void RemoveTareas(List<MantenimientoTarea> tareas);
    Task<decimal> TotalDePago(int idMantenimiento);
}

public class MantenimientoRepo : IMantenimientoRepo
{
    private readonly AppDbContext _ctx;

    public MantenimientoRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }


    public async Task<Mantenimiento> UltimoMantenimientoCamion(int idCamion)
    {
        return await _ctx.Mantenimientos
            .Where(m => m.IdVehiculo == idCamion)
            .OrderByDescending(m => m.FechaSalida)
            .FirstOrDefaultAsync();
    }

    public Task<Mantenimiento> FindByIdAsync(int id, bool asNoTracking = false, bool includeTarea = false, bool includeObs = false,
        bool includeCamion = false, bool includeTaller = false)
    {
        var query = _ctx.Mantenimientos.AsQueryable();
        
        if (asNoTracking)
            query = query.AsNoTracking();

        if (includeTarea)
            query = query.Include(m => m.Tareas);

        if (includeObs)
            query = query.Include(m => m.Observaciones);
        
        if (includeCamion)
            query = query.Include(m => m.Vehiculo);
        
        if (includeTaller)
            query = query.Include(m => m.Taller);
        
        return query.FirstOrDefaultAsync(m => m.IdMantenimiento == id);
    }

    public void Add(Mantenimiento mantenimiento)
    {
        _ctx.Mantenimientos.Add(mantenimiento);
    }

    public void Remove(Mantenimiento mantenimiento)
    {
        _ctx.Mantenimientos.Remove(mantenimiento);
    }

    public void RemoveTareas(List<MantenimientoTarea> tareas)
    {
        _ctx.MantenimientoTareas.RemoveRange(tareas);
    }

    public async Task<decimal> TotalDePago(int idMantenimiento)
    {
        return _ctx.Pagos
            .Where(p => p.PagoMantenimientos.Any(m => m.IdMantenimiento == idMantenimiento))
            .Sum(p => p.Monto);
    }
}