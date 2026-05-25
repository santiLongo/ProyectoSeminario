using Microsoft.EntityFrameworkCore;
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
            throw new InvalidOperationException("La factura no existe");

        if (factura.Tipo != Factura.TipoFactura.Emitida)
            throw new InvalidOperationException("Solo se pueden confirmar facturas emitidas");

        if (factura.Anulada)
            throw new InvalidOperationException("No se puede confirmar una factura anulada");

        if (factura.Confirmada)
            throw new InvalidOperationException("La factura ya está confirmada");

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
