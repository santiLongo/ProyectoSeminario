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
    // Task<bool> ViajeTieneFacturaActivaAsync(int idViaje);
    //Task<int> ObtenerProximoNumeroAsync(Factura.TipoFactudra tipo, int puntoVenta);
    void Remove(Factura factura);
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
        // var factura = await Query()
        //     .IncludeRecibos()
        //     .FirstOrDefaultAsync(f => f.IdFactura == idFactura);
        //
        // if (factura == null)
        //     throw new InvalidOperationException($"No se encontró la factura {idFactura}");
        //
        // factura.RecalcularEstado(factura.ReciboFacturas);
    }

    // public async Task<bool> ViajeTieneFacturaActivaAsync(int idViaje)
    // {
    //     return await _ctx.FacturasViaje
    //         .AnyAsync(fv => fv.IdViaje == idViaje && !fv.Factura.Anulada);
    // }

    // public async Task<int> ObtenerProximoNumeroAsync(Factura.TipoFactura tipo, int puntoVenta)
    // {
    //     var ultimo = await Query()
    //         .Where(f => f.Tipo == tipo && f.PuntoVenta == puntoVenta)
    //         .OrderByDescending(f => f.Numero)
    //         .Select(f => (int?)f.Numero)
    //         .FirstOrDefaultAsync();
    //
    //     return (ultimo ?? 0) + 1;
    // }

    public void Remove(Factura factura)
    {
        _ctx.Facturas.Remove(factura);
    }
}

public static class FacturaQueryExtensions
{
    public static IQueryable<Factura> IncludeDetalles(this IQueryable<Factura> query) =>
        query.Include(f => f.Detalles);
}
