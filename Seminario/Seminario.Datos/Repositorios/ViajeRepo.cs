using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IViajeRepo
{
    IQueryable<Viaje> Query();
    void Add(Viaje viaje);
    void Remove(Viaje viaje);
}

public class ViajeRepo : IViajeRepo
{
    private readonly AppDbContext _ctx;
    
    public ViajeRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }


    public IQueryable<Viaje> Query()
    {
        return _ctx.Viajes.AsQueryable();
    }

    public void Add(Viaje viaje)
    {
        _ctx.Viajes.Add(viaje);
    }

    public void Remove(Viaje viaje)
    {
        _ctx.Viajes.Remove(viaje);
    }
}

public static class ViajeQueryExtensions
{
    public static IQueryable<Viaje> WhereEnViaje(this IQueryable<Viaje> query) =>
        query.Where(v => (v.FechaDescarga == null || v.FechaDescarga > DateTime.Today) 
                         && v.Estado == EstadosViaje.EnViaje.ToInt());

    public static IQueryable<Viaje> IncludeDestinosProcedencias(this IQueryable<Viaje> query) =>
        query.Include(c => c.Destinos).Include(c => c.Procendecias);
    
    public static IQueryable<Viaje> IncludeCamion(this IQueryable<Viaje> query) =>
        query.Include(c => c.Camion);
    
    public static IQueryable<Viaje> IncludeChofer(this IQueryable<Viaje> query) =>
        query.Include(c => c.Chofer);
    
    public static IQueryable<Viaje> Forzable(this IQueryable<Viaje> query) =>
        query.Where(v => v.Estado < EstadosViaje.Suspendido.ToInt());

    public static IQueryable<Viaje> IncludeCliente(this IQueryable<Viaje> query) =>
        query.Include(v => v.Cliente);
}