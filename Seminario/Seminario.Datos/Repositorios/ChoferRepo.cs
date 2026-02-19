using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IChoferRepo
{
    IQueryable<Chofer> Query();
    Task<Chofer?> GetAsync(Func<IQueryable<Chofer>, IQueryable<Chofer>> querys);
    Task<List<Chofer>> GetAllByEstadoAsync(int estado);
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

    [Obsolete("No usar mas")]
    public IQueryable<Chofer> Query()
        => _ctx.Choferes;

    public async Task<Chofer?> GetAsync(Func<IQueryable<Chofer>, IQueryable<Chofer>> querys)
    {
        IQueryable<Chofer> query = _ctx.Choferes.AsQueryable();
        
        if (querys != null)
            query = querys(query);
        
        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<Chofer>> GetAllByEstadoAsync(int estado)
    {
        return await _ctx.Choferes.Where(c => estado == (int)GetAllChoferesEstados.Todos 
                                         || (estado == (int)GetAllChoferesEstados.Activos && c.FechaBaja == null)
                                         || (estado == (int)GetAllChoferesEstados.Inactivos && c.FechaBaja != null)).ToListAsync();
    }

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

    public static IQueryable<Chofer> ConViajesNoFinalizados(this IQueryable<Chofer> query)
        => query.Include(c => c.Viajes.Where(v => v.Estado == EstadosViaje.EnViaje.ToInt()));

    public static IQueryable<Chofer> WhereEqualIdChofer(this IQueryable<Chofer> query, int id) =>
        query.Where(q => q.IdChofer == id);
}

public enum GetAllChoferesEstados
{
    Activos = 1,
    Inactivos = 2,
    Todos = 3
}