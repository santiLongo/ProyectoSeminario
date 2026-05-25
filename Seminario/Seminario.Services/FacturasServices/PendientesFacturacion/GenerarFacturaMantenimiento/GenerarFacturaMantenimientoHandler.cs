using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Services.FacturasServices.PendientesFacturacion.GenerarFacturaMantenimiento;

public class GenerarFacturaMantenimientoHandler
{
    private readonly IAppDbContext _ctx;

    public GenerarFacturaMantenimientoHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<int> HandleAsync(int idMantenimiento, GenerarFacturaMantenimientoCommand command)
    {
        var mant = await _ctx.MantenimientoRepo.FindByIdAsync(idMantenimiento, includeTaller: true);

        if (mant == null)
            throw new InvalidOperationException("El mantenimiento no existe");

        if (mant.FechaSalida == null)
            throw new InvalidOperationException("El mantenimiento no está terminado (sin fecha de salida)");

        if (mant.Suspendido)
            throw new InvalidOperationException("El mantenimiento está suspendido");

        var yaFacturado = _ctx.FacturaRepo.Query()
            .Any(f => !f.Anulada && f.FacturasMantenimiento.Any(fm => fm.IdMantenimiento == idMantenimiento));

        if (yaFacturado)
            throw new InvalidOperationException("El mantenimiento ya tiene una factura activa");

        var subtotal = mant.Importe ?? 0;
        var ivaImporte = subtotal * (command.PorcentajeIva / 100m);
        var total = subtotal + ivaImporte;

        var factura = new Factura
        {
            Tipo = Factura.TipoFactura.Recibida,
            PuntoVenta = command.PuntoVenta!.Value,
            Numero = command.Numero!.Value,
            FechaEmision = DateTime.Today,
            FechaVencimiento = command.FechaVencimiento,
            Subtotal = subtotal,
            PorcentajeIva = command.PorcentajeIva,
            Total = total,
            IdMoneda = command.IdMoneda,
            Estado = Factura.EstadoFactura.Pendiente,
            Observaciones = command.Observaciones,
            Anulada = false,
            Confirmada = false,
            IdTaller = mant.IdTaller
        };

        factura.Detalles.Add(new FacturaDetalle
        {
            Orden = 1,
            Descripcion = $"Mantenimiento: {mant.Titulo}",
            Cantidad = 1,
            PrecioUnitario = subtotal,
            PorcentajeIva = command.PorcentajeIva,
            Subtotal = subtotal,
            Total = total
        });

        factura.FacturasMantenimiento.Add(new FacturaMantenimiento
        {
            IdMantenimiento = mant.IdMantenimiento,
            ImporteMantenimiento = mant.Importe ?? 0
        });

        _ctx.FacturaRepo.Add(factura);
        await _ctx.SaveChangesAsync();

        return factura.IdFactura;
    }
}
