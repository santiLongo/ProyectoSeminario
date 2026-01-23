using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IMantenimientoRepo
{
    Task<Mantenimiento> UltimoMantenimientoCamion(int idCamion);
    void Add(Mantenimiento mantenimiento);
    void Remove(Mantenimiento mantenimiento);
}

public class MantenimientoRepo : IMantenimientoRepo
{
    private readonly AppDbContext _ctx;
    private IMantenimientoRepo _mantenimientoRepoImplementation;

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

    public void Add(Mantenimiento mantenimiento)
    {
        _mantenimientoRepoImplementation.Add(mantenimiento);
    }

    public void Remove(Mantenimiento mantenimiento)
    {
        _mantenimientoRepoImplementation.Remove(mantenimiento);
    }
}