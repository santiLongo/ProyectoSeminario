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