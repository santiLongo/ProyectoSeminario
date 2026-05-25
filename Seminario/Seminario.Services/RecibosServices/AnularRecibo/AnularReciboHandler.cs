using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Repositorios;

namespace Seminario.Services.RecibosServices.AnularRecibo;

public class AnularReciboHandler
{
    private readonly IAppDbContext _ctx;

    public AnularReciboHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(AnularReciboCommand command)
    {
        var recibo = await _ctx.ReciboRepo.Query()
            .IncludeFormasDePago()
            .IncludeFacturas()
            .FirstOrDefaultAsync(r => r.IdRecibo == command.IdRecibo!.Value);

        if (recibo == null)
            throw new InvalidOperationException("El recibo no existe");

        if (recibo.Anulado)
            throw new InvalidOperationException("El recibo ya se encuentra anulado");

        // Marcar recibo como anulado
        recibo.Anulado = true;

        // Marcar cheques relacionados como rechazados
        foreach (var fp in recibo.FormasDePago)
        {
            if (fp.PagoCheque != null)
            {
                fp.PagoCheque.Rechazado = true;
            }
        }

        await _ctx.SaveChangesAsync();

        // Recalcular estado de las facturas que tenían este recibo imputado
        var idsFacturas = recibo.ReciboFacturas.Select(rf => rf.IdFactura).ToList();

        foreach (var idFactura in idsFacturas)
        {
            await _ctx.FacturaRepo.RecalcularEstadoAsync(idFactura);
        }

        if (idsFacturas.Any())
            await _ctx.SaveChangesAsync();
    }
}
