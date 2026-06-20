using System.Net;
using Seminario.Core.Exceptions.SeminarioException;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Services.FacturasServices.FacturasEmitidas.ConfirmarEmitida;

public class ConfirmarEmitidaHandler
{
    private readonly IAppDbContext _ctx;

    public ConfirmarEmitidaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(int idFactura, ConfirmarEmitidaCommand command)
    {
        var factura = await _ctx.FacturaRepo.FindByIdAsync(idFactura);

        if (factura == null)
            throw new SeminarioException("La factura no existe", HttpStatusCode.NotFound);

        if (factura.Tipo != Factura.TipoFactura.Emitida)
            throw new SeminarioException("Solo se pueden confirmar facturas emitidas con este endpoint", HttpStatusCode.BadRequest);

        if (factura.Anulada)
            throw new SeminarioException("No se puede confirmar una factura anulada", HttpStatusCode.Conflict);

        if (factura.Confirmada)
            throw new SeminarioException("La factura ya está confirmada", HttpStatusCode.Conflict);

        factura.Confirmada = true;

        if (!string.IsNullOrWhiteSpace(command.CAE))
        {
            factura.CAE = command.CAE;
            factura.CAEFchVto = command.CAEFchVto;
            factura.TipoComprobante = command.TipoComprobante;
        }

        await _ctx.SaveChangesAsync();
    }
}
