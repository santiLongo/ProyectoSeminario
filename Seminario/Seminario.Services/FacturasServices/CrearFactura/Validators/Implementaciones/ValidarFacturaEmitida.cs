using Microsoft.EntityFrameworkCore;
using Seminario.Datos;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Enums;

namespace Seminario.Services.FacturasServices.CrearFactura;

public class ValidarFacturaEmitida : IValidarFactura
{
    public async Task ValidarAsync(CrearFacturaCommand command, IAppDbContext ctx)
    {
        if (command.IdCliente == null)
            throw new InvalidOperationException("El cliente es requerido para facturas emitidas");

        var cliente = await ctx.ClienteRepo.FindByIdAsync(command.IdCliente.Value);
        if (cliente == null)
            throw new InvalidOperationException("El cliente informado no existe");

        foreach (var vCmd in command.Viajes)
        {
            var viaje = await ctx.ViajeRepo.Query()
                .FirstOrDefaultAsync(v => v.IdViaje == vCmd.IdViaje!.Value);

            if (viaje == null)
                throw new InvalidOperationException($"El viaje {vCmd.IdViaje} no existe");

            if (viaje.Estado != EstadosViaje.Finalizado.ToInt() && viaje.Estado != EstadosViaje.EnViaje.ToInt())
                throw new InvalidOperationException($"El viaje {vCmd.IdViaje} no está en estado En Viaje ni Finalizado");

            var tieneFactura = await ctx.FacturaRepo.ViajeTieneFacturaActivaAsync(vCmd.IdViaje!.Value);
            if (tieneFactura)
                throw new InvalidOperationException($"El viaje {vCmd.IdViaje} ya tiene una factura activa");
        }
    }
}
