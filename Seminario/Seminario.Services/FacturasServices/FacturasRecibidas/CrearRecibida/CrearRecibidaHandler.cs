using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.FacturasServices.CrearFactura;

namespace Seminario.Services.FacturasServices.FacturasRecibidas.CrearRecibida;

public class CrearRecibidaHandler
{
    private readonly IAppDbContext _ctx;

    public CrearRecibidaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<int> HandleAsync(CrearRecibidaCommand command)
    {
        if (command.IdProveedor == null && command.IdTaller == null)
            throw new InvalidOperationException("Debe informar un proveedor o un taller");

        if (command.IdProveedor != null)
        {
            var proveedor = await _ctx.ProveedorRepo.FindByIdAsync(command.IdProveedor.Value);
            if (proveedor == null)
                throw new InvalidOperationException("El proveedor informado no existe");
        }

        if (command.IdTaller != null)
        {
            var taller = await _ctx.TallerRepo.FindByIdAsync(command.IdTaller.Value);
            if (taller == null)
                throw new InvalidOperationException("El taller informado no existe");
        }

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
            Confirmada = false,
            IdProveedor = command.IdProveedor,
            IdTaller = command.IdTaller
        };

        foreach (var detalle in detalles)
            factura.Detalles.Add(detalle);

        foreach (var idMant in command.IdsMantenimiento)
        {
            var mant = await _ctx.MantenimientoRepo.FindByIdAsync(idMant);
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

        _ctx.FacturaRepo.Add(factura);
        await _ctx.SaveChangesAsync();

        return factura.IdFactura;
    }
}
