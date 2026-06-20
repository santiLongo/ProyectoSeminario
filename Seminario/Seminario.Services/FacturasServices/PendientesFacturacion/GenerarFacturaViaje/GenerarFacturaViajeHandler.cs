using Microsoft.EntityFrameworkCore;
using Seminario.Datos;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Enums;

namespace Seminario.Services.FacturasServices.PendientesFacturacion.GenerarFacturaViaje;

public class GenerarFacturaViajeHandler
{
    private readonly IAppDbContext _ctx;

    public GenerarFacturaViajeHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<int> HandleAsync(int idViaje, GenerarFacturaViajeCommand command)
    {
        var viaje = await _ctx.ViajeRepo.Query()
            .Include(v => v.Cliente)
            .FirstOrDefaultAsync(v => v.IdViaje == idViaje);

        if (viaje == null)
            throw new InvalidOperationException("El viaje no existe");

        if (viaje.Estado != EstadosViaje.Finalizado.ToInt() && viaje.Estado != EstadosViaje.Cobrado.ToInt())
            throw new InvalidOperationException("El viaje debe estar en estado Finalizado o Cobrado para generar una factura");

        var tieneFactura = await _ctx.FacturaRepo.ViajeTieneFacturaActivaAsync(idViaje);
        if (tieneFactura)
            throw new InvalidOperationException("El viaje ya tiene una factura activa");

        var subtotal = viaje.MontoTotal;
        var ivaImporte = subtotal * (command.PorcentajeIva / 100m);
        var total = subtotal + ivaImporte;

        var descripcion = string.IsNullOrEmpty(viaje.NroViaje)
            ? "Servicio de transporte"
            : $"Servicio de transporte - Viaje {viaje.NroViaje}";

        var factura = new Factura
        {
            Tipo = Factura.TipoFactura.Emitida,
            PuntoVenta = 1,
            Numero = await _ctx.FacturaRepo.ObtenerProximoNumeroAsync(Factura.TipoFactura.Emitida, 1),
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
            IdCliente = viaje.IdCliente
        };

        factura.Detalles.Add(new FacturaDetalle
        {
            Orden = 1,
            Descripcion = descripcion,
            Cantidad = 1,
            PrecioUnitario = subtotal,
            PorcentajeIva = command.PorcentajeIva,
            Subtotal = subtotal,
            Total = total
        });

        factura.FacturasViaje.Add(new FacturaViaje
        {
            IdViaje = viaje.IdViaje,
            Viaje = viaje,
            MontoViaje = viaje.MontoTotal
        });

        viaje.Estado = EstadosViaje.Facturado.ToInt();

        _ctx.FacturaRepo.Add(factura);
        await _ctx.SaveChangesAsync();

        return factura.IdFactura;
    }
}
