using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.FacturasServices.CrearFactura;

namespace Seminario.Services.FacturasServices.FacturasEmitidas.CrearEmitidaSinViaje;

public class CrearEmitidaSinViajeHandler
{
    private readonly IAppDbContext _ctx;

    public CrearEmitidaSinViajeHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<int> HandleAsync(CrearEmitidaSinViajeCommand command)
    {
        var cliente = await _ctx.ClienteRepo.FindByIdAsync(command.IdCliente!.Value);
        if (cliente == null)
            throw new InvalidOperationException("El cliente informado no existe");

        var (subtotal, totalConIva, detalles) = FacturaCalculos.CalcularDetalles(command.Detalles);

        var factura = new Factura
        {
            Tipo = Factura.TipoFactura.Emitida,
            PuntoVenta = 1,
            Numero = await _ctx.FacturaRepo.ObtenerProximoNumeroAsync(Factura.TipoFactura.Emitida, 1),
            FechaEmision = command.FechaEmision!.Value,
            FechaVencimiento = command.FechaVencimiento,
            Subtotal = subtotal,
            PorcentajeIva = command.PorcentajeIva!.Value,
            Total = totalConIva,
            IdMoneda = command.IdMoneda!.Value,
            TipoCambio = command.TipoCambio,
            Estado = Factura.EstadoFactura.Pendiente,
            Observaciones = command.Observaciones,
            Anulada = false,
            Confirmada = false,
            IdCliente = command.IdCliente
        };

        foreach (var detalle in detalles)
            factura.Detalles.Add(detalle);

        _ctx.FacturaRepo.Add(factura);
        await _ctx.SaveChangesAsync();

        return factura.IdFactura;
    }
}
