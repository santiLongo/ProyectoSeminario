using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Services.FacturasServices.FacturasRecibidas.ConfirmarRecibida;

public class ConfirmarRecibidaHandler
{
    private readonly IAppDbContext _ctx;

    public ConfirmarRecibidaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(int idFactura, ConfirmarRecibidaCommand command)
    {
        var factura = await _ctx.FacturaRepo.FindByIdAsync(idFactura);

        if (factura == null)
            throw new SeminarioException("La factura no existe", HttpStatusCode.NotFound);

        if (factura.Tipo != Factura.TipoFactura.Recibida)
            throw new SeminarioException("Solo se pueden confirmar facturas recibidas con este endpoint", HttpStatusCode.BadRequest);

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
