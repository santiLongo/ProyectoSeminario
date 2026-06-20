using System.Net;
using Seminario.Core.Exceptions.SeminarioException;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Services.FacturasServices.FacturasEmitidas.UpdateEmitida;

public class UpdateEmitidaHandler
{
    private readonly IAppDbContext _ctx;

    public UpdateEmitidaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(int idFactura, UpdateEmitidaCommand command)
    {
        var factura = await _ctx.FacturaRepo.FindByIdAsync(idFactura);

        if (factura == null)
            throw new SeminarioException("La factura no existe", HttpStatusCode.NotFound);

        if (factura.Tipo != Factura.TipoFactura.Emitida)
            throw new SeminarioException("Solo se pueden editar facturas emitidas con este endpoint", HttpStatusCode.BadRequest);

        if (factura.Anulada)
            throw new SeminarioException("No se puede editar una factura anulada", HttpStatusCode.Conflict);

        if (factura.Confirmada)
            throw new SeminarioException("No se puede editar una factura ya confirmada", HttpStatusCode.Conflict);

        factura.FechaVencimiento = command.FechaVencimiento;
        factura.IdMoneda = command.IdMoneda;
        factura.TipoCambio = command.TipoCambio;
        factura.Observaciones = command.Observaciones;

        await _ctx.SaveChangesAsync();
    }
}
