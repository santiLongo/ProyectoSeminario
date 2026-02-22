using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IPagosRepo
{
    void Add(Pago pago);
    void Anular(Pago pago);
    Task<Pago> FindByChequeAsync(int cheque);
    Task<Pago> FindByIdAsync(int id, bool asNoTracking = false, bool includeCheque = false);
}

public class PagosRepo : IPagosRepo
{
    private readonly AppDbContext _ctx;
    
    public PagosRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }


    public void Add(Pago pago)
    {
        _ctx.Pagos.Add(pago);
    }

    public void Anular(Pago pago)
    {
        throw new NotImplementedException();
    }

    public async Task<Pago> FindByChequeAsync(int cheque)
    {
        return await _ctx.Pagos.FirstOrDefaultAsync(p => p.IdPagoCheque == cheque);
    }

    public async Task<Pago> FindByIdAsync(int id, bool asNoTracking = false, bool includeCheque = false)
    {
        var query = _ctx.Pagos.AsQueryable();

        if (asNoTracking)
            query = query.AsNoTracking();

        if (includeCheque)
            query = query.Include(p => p.Cheque);
        
        return await query.FirstOrDefaultAsync(p => p.IdPagoCheque == id);
    }
}