using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Datos.Repositorios;

public interface IPagoChequeRepo
{
    void Add(PagoCheque cheque);
    void Remove(PagoCheque cheque);
    Task<PagoCheque> FindByNroYBancoAsync(int numeroCheque, int idBanco);
    Task<PagoCheque> FindByIdAsync(int id, bool asNoTracking = false, bool includePago = false, bool includeCobro = false);
}

public class PagoChequeRepo : IPagoChequeRepo
{
    private readonly AppDbContext _ctx;

    public PagoChequeRepo(AppDbContext ctx)
    {
        _ctx = ctx;
    }


    public void Add(PagoCheque cheque)
    {
        _ctx.PagoCheques.Add(cheque);
    }

    public void Remove(PagoCheque cheque)
    {
        _ctx.PagoCheques.Remove(cheque);
    }

    public async Task<PagoCheque> FindByNroYBancoAsync(int numeroCheque, int idBanco)
    {
        return await _ctx.PagoCheques.FirstOrDefaultAsync(c => c.NroCheque == numeroCheque && c.IdBanco == idBanco);
    }

    public async Task<PagoCheque> FindByIdAsync(int id, bool asNoTracking = false, bool includePago = false, bool includeCobro = false)
    {
        var query = _ctx.PagoCheques.AsQueryable();
        
        if(asNoTracking)
            query = query.AsNoTracking();
        
        if (includePago)
            query = query.Include(p => p.Pago);
        
        if (includeCobro)
            query = query.Include(p => p.Cobro);

        return await query.FirstOrDefaultAsync(c => c.IdPagoCheque == id);
    }
}