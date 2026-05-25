using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IFacturaRepo
{
    IQueryable<Factura> Query();
    Task<Factura?> FindByIdAsync(int id);
    void Add(Factura factura);
    Task RecalcularEstadoAsync(int idFactura);
    Task<bool> ViajeTieneFacturaActivaAsync(int idViaje);
    Task<int> ObtenerProximoNumeroAsync(Factura.TipoFactura tipo, int puntoVenta);
}

public class FacturaRepo : IFacturaRepo
{
    private readonly AppDbContext _ctx;

    public FacturaRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public IQueryable<Factura> Query()
    {
        return _ctx.Facturas.AsQueryable();
    }

    public async Task<Factura?> FindByIdAsync(int id)
    {
        return await Query().FirstOrDefaultAsync(f => f.IdFactura == id);
    }

    public void Add(Factura factura)
    {
        _ctx.Facturas.Add(factura);
    }

    public async Task RecalcularEstadoAsync(int idFactura)
    {
        var factura = await Query()
            .IncludeRecibos()
            .FirstOrDefaultAsync(f => f.IdFactura == idFactura);

        if (factura == null)
            throw new InvalidOperationException($"No se encontró la factura {idFactura}");

        factura.RecalcularEstado(factura.ReciboFacturas);
    }

    public async Task<bool> ViajeTieneFacturaActivaAsync(int idViaje)
    {
        return await _ctx.FacturasViaje
            .AnyAsync(fv => fv.IdViaje == idViaje && !fv.Factura.Anulada);
    }

    public async Task<int> ObtenerProximoNumeroAsync(Factura.TipoFactura tipo, int puntoVenta)
    {
        var ultimo = await Query()
            .Where(f => f.Tipo == tipo && f.PuntoVenta == puntoVenta)
            .OrderByDescending(f => f.Numero)
            .Select(f => (int?)f.Numero)
            .FirstOrDefaultAsync();

        return (ultimo ?? 0) + 1;
    }
}

public static class FacturaQueryExtensions
{
    public static IQueryable<Factura> IncludeDetalles(this IQueryable<Factura> query) =>
        query.Include(f => f.Detalles);

    public static IQueryable<Factura> IncludeViajes(this IQueryable<Factura> query) =>
        query.Include(f => f.FacturasViaje).ThenInclude(fv => fv.Viaje).ThenInclude(v => v.Cliente);

    public static IQueryable<Factura> IncludeMantenimientos(this IQueryable<Factura> query) =>
        query.Include(f => f.FacturasMantenimiento).ThenInclude(fm => fm.Mantenimiento);

    public static IQueryable<Factura> IncludeCompras(this IQueryable<Factura> query) =>
        query.Include(f => f.FacturasCompraRepuesto).ThenInclude(fc => fc.CompraRepuesto);

    public static IQueryable<Factura> IncludeRecibos(this IQueryable<Factura> query) =>
        query.Include(f => f.ReciboFacturas).ThenInclude(rf => rf.Recibo);

    public static IQueryable<Factura> IncludeContraparte(this IQueryable<Factura> query) =>
        query.Include(f => f.Cliente)
             .Include(f => f.Proveedor)
             .Include(f => f.Taller);
}
