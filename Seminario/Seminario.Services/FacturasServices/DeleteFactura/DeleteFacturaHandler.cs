using System.Net;
using Seminario.Core.Exceptions.SeminarioException;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Core.ExtensionMethods;

namespace Seminario.Services.FacturasServices.DeleteFactura;

public class DeleteFacturaHandler
{
    private readonly IAppDbContext _ctx;

    public DeleteFacturaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(DeleteFacturaCommand command)
    {
        var factura = await _ctx.FacturaRepo.FindByIdAsync(command.IdFactura);

        if (factura.IsNull())
        {
            throw new SeminarioException("No se encontro la factura para borrar", HttpStatusCode.NotFound);
        }

        if (factura!.Confirmada)
        {
            throw new SeminarioException("No se puede borrar una factura confirmada", HttpStatusCode.Conflict);
        }
        
        _ctx.FacturaRepo.Remove(factura);
        await _ctx.SaveChangesAsync();
    }
}