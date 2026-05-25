using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Services.FacturasServices.CrearFactura;

public class CrearFacturaRecibida : ICrearFactura
{
    private readonly IValidarFactura _validar;

    public CrearFacturaRecibida(IValidarFactura validar)
    {
        _validar = validar;
    }

    public async Task<int> CrearAsync(CrearFacturaCommand command, IAppDbContext ctx)
    {
        await _validar.ValidarAsync(command, ctx);

        var (subtotal, totalConIva, detalles) = FacturaCalculos.CalcularDetalles(command.Detalles);

        var factura = new Factura
        {
            Tipo = Factura.TipoFactura.Recibida,
            PuntoVenta = command.PuntoVenta!.Value,
            Numero = command.Numero!.Value,
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
            IdProveedor = command.TipoOrigen == 1 ? command.IdProveedor : null,
            IdTaller = command.TipoOrigen == 2 ? command.IdTaller : null
        };

        factura.CreatedAt(DateTime.Now, "sistema");

        foreach (var detalle in detalles)
            factura.Detalles.Add(detalle);

        foreach (var idMant in command.IdsMantenimiento)
        {
            var mant = await ctx.MantenimientoRepo.FindByIdAsync(idMant);
            if (mant == null)
                throw new InvalidOperationException($"El mantenimiento {idMant} no existe");

            factura.FacturasMantenimiento.Add(new FacturaMantenimiento
            {
                IdMantenimiento = idMant,
                ImporteMantenimiento = mant.Importe ?? 0
            });
        }

        foreach (var idCompra in command.IdsCompraRepuesto)
        {
            factura.FacturasCompraRepuesto.Add(new FacturaCompraRepuesto
            {
                IdCompraRepuesto = idCompra,
                ImporteCompra = 0
            });
        }

        ctx.FacturaRepo.Add(factura);
        await ctx.SaveChangesAsync();

        return factura.IdFactura;
    }
}
