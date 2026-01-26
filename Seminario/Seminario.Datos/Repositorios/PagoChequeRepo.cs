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
}