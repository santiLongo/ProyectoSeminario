using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IReciboRepo
{
    IQueryable<Recibo> Query();
    Task<Recibo?> FindByIdAsync(int id);
    void Add(Recibo recibo);
}

public class ReciboRepo : IReciboRepo
{
    private readonly AppDbContext _ctx;

    public ReciboRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public IQueryable<Recibo> Query()
    {
        return _ctx.Recibos.AsQueryable();
    }

    public async Task<Recibo?> FindByIdAsync(int id)
    {
        return await Query().FirstOrDefaultAsync(r => r.IdRecibo == id);
    }

    public void Add(Recibo recibo)
    {
        _ctx.Recibos.Add(recibo);
    }
}

public static class ReciboQueryExtensions
{
    public static IQueryable<Recibo> IncludeFormasDePago(this IQueryable<Recibo> query) =>
        query.Include(r => r.FormasDePago).ThenInclude(fp => fp.FormaPago)
             .Include(r => r.FormasDePago).ThenInclude(fp => fp.PagoCheque);

    public static IQueryable<Recibo> IncludeFacturas(this IQueryable<Recibo> query) =>
        query.Include(r => r.ReciboFacturas).ThenInclude(rf => rf.Factura);

    public static IQueryable<Recibo> IncludeContraparte(this IQueryable<Recibo> query) =>
        query.Include(r => r.Cliente)
             .Include(r => r.Proveedor)
             .Include(r => r.Taller);
}
