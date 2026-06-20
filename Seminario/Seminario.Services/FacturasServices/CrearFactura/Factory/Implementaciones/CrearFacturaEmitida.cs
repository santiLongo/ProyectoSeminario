using Microsoft.EntityFrameworkCore;
using Seminario.Datos;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Enums;

namespace Seminario.Services.FacturasServices.CrearFactura;

public class CrearFacturaEmitida : ICrearFactura
{
    private readonly IValidarFactura _validar;

    public CrearFacturaEmitida(IValidarFactura validar)
    {
        _validar = validar;
    }

    public async Task<int> CrearAsync(CrearFacturaCommand command, IAppDbContext ctx)
    {
        await _validar.ValidarAsync(command, ctx);

        var viajeVinculos = new List<(Viaje Viaje, decimal Monto)>();
        foreach (var vCmd in command.Viajes)
        {
            var viaje = await ctx.ViajeRepo.Query()
                .FirstOrDefaultAsync(v => v.IdViaje == vCmd.IdViaje!.Value);

            viajeVinculos.Add((viaje!, vCmd.MontoViaje!.Value));
        }

        var (subtotal, totalConIva, detalles) = FacturaCalculos.CalcularDetalles(command.Detalles);

        var factura = new Factura
        {
            Tipo = Factura.TipoFactura.Emitida,
            PuntoVenta = 1,
            Numero = await ctx.FacturaRepo.ObtenerProximoNumeroAsync(Factura.TipoFactura.Emitida, 1),
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
            IdCliente = command.IdCliente
        };

        foreach (var detalle in detalles)
            factura.Detalles.Add(detalle);

        foreach (var (viaje, monto) in viajeVinculos)
        {
            factura.FacturasViaje.Add(new FacturaViaje
            {
                IdViaje = viaje.IdViaje,
                Viaje = viaje,
                MontoViaje = monto
            });
            viaje.Estado = EstadosViaje.Facturado.ToInt();
        }

        ctx.FacturaRepo.Add(factura);
        await ctx.SaveChangesAsync();

        return factura.IdFactura;
    }
}
