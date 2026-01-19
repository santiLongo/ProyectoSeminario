using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IChoferRepo
{
    IQueryable<Chofer> Query();
    void Add(Chofer chofer);
    void Remove(Chofer chofer);
}

public class ChoferRepo : IChoferRepo
{
    private readonly AppDbContext _ctx;

    public ChoferRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public IQueryable<Chofer> Query()
        => _ctx.Choferes;

    public void Add(Chofer chofer)
    {
        _ctx.Choferes.Add(chofer);
    }

    public void Remove(Chofer chofer)
    {
        _ctx.Choferes.Remove(chofer);
    }
}

public static class ChoferQueryExtensions
{
    public static IQueryable<Chofer> Activos(this IQueryable<Chofer> query)
        => query.Where(c => c.FechaBaja == null);

    public static IQueryable<Chofer> ConNoViajesFinalizados(this IQueryable<Chofer> query)
        => query.Include(c => c.Viajes.Where(v => v.FechaDescarga == null));
}