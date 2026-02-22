using Microsoft.EntityFrameworkCore;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface ICobrosRepo
{
    Task<Cobro> FindByIdCobroAsync(int id, bool asNoTracking = false, bool includeViaje = false, bool includeCheque = false);
    Task<List<Cobro>> FindCobrosByViajeAsync(int idViaje);
    void Add(Cobro cobro);
    Task Anular(Cobro cobro);
    Task Anular(int idCobro);
}

public class CobrosRepo : ICobrosRepo
{
    private readonly AppDbContext _ctx;

    public CobrosRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<Cobro> FindByIdCobroAsync(int id, bool asNoTracking = false, bool includeViaje = false, bool includeCheque = false)
    {
        var query = _ctx.Cobros.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();
        
        if (includeViaje)
            query = query.Include(v => v.Viaje);
        
        if (includeCheque)
            query = query.Include(v => v.Cheque);
        
        return await query.FirstOrDefaultAsync(x => x.IdCobro == id);
    }

    public async Task<List<Cobro>> FindCobrosByViajeAsync(int idViaje)
    {
        return await _ctx.Cobros.Where(x => x.IdViaje == idViaje).ToListAsync();
    }

    public void Add(Cobro cobro)
    {
        _ctx.Cobros.Add(cobro);
    }

    public async Task Anular(Cobro cobro)
    {
        var anulador = new Cobro
        {
            IdViaje = cobro.IdViaje,
            FechaRecibo = cobro.FechaRecibo,
            Monto = cobro.Monto * -1,
            IdMoneda = cobro.IdMoneda,
            TipoCambio = cobro.TipoCambio,
            IdFormaPago = cobro.IdFormaPago,
            CobroAnulado = cobro.IdCobro,
        };

        if (anulador.IdPagoCheque > 0)
        {
            var cheque = cobro.Cheque ?? await _ctx.PagoChequeRepo.FindByIdAsync(anulador.IdPagoCheque.GetValueOrDefault());
            cheque.Rechazado = true;
        }
        
        _ctx.Cobros.Add(anulador);
        await _ctx.SaveChangesAsync();
        await _ctx.ViajeRepo.ActualizarEstadoAsync(anulador.IdViaje);
    }

    public async Task Anular(int idCobro)
    {
        var cobro = await FindByIdCobroAsync(idCobro);

        if (cobro == null)
            return;
        
        //Es un cobro anulador
        if(cobro.Monto <= 0)
            return;
            
        await Anular(cobro);
    }
}