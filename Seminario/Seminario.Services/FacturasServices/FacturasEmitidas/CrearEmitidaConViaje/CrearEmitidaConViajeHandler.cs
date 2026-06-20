using Microsoft.EntityFrameworkCore;
using Seminario.Datos;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Enums;
using Seminario.Datos.Repositorios;
using Seminario.Services.FacturasServices.CrearFactura;

namespace Seminario.Services.FacturasServices.FacturasEmitidas.CrearEmitidaConViaje;

public class CrearEmitidaConViajeHandler
{
    private readonly IAppDbContext _ctx;

    public CrearEmitidaConViajeHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<int> HandleAsync(CrearEmitidaConViajeCommand command)
    {
        if (!command.Viajes.Any())
            throw new InvalidOperationException("Debe asociar al menos un viaje a la factura");

        var cliente = await _ctx.ClienteRepo.FindByIdAsync(command.IdCliente!.Value);
        if (cliente == null)
            throw new InvalidOperationException("El cliente informado no existe");

        var viajeVinculos = new List<(Viaje Viaje, decimal Monto)>();
        foreach (var vCmd in command.Viajes)
        {
            var viaje = await _ctx.ViajeRepo.Query()
                .FirstOrDefaultAsync(v => v.IdViaje == vCmd.IdViaje!.Value);

            if (viaje == null)
                throw new InvalidOperationException($"El viaje {vCmd.IdViaje} no existe");

            if (viaje.Estado != EstadosViaje.Finalizado.ToInt() && viaje.Estado != EstadosViaje.EnViaje.ToInt())
                throw new InvalidOperationException($"El viaje {vCmd.IdViaje} no está en estado En Viaje ni Finalizado");

            var tieneFactura = await _ctx.FacturaRepo.ViajeTieneFacturaActivaAsync(vCmd.IdViaje!.Value);
            if (tieneFactura)
                throw new InvalidOperationException($"El viaje {vCmd.IdViaje} ya tiene una factura activa");

            viajeVinculos.Add((viaje, vCmd.MontoViaje!.Value));
        }

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

        _ctx.FacturaRepo.Add(factura);
        await _ctx.SaveChangesAsync();

        return factura.IdFactura;
    }
}
