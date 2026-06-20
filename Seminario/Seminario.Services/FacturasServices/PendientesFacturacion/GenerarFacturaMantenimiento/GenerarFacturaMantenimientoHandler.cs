using System.Net;
using Seminario.Core.Exceptions.SeminarioException;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Services.FacturasServices.PendientesFacturacion.GenerarFacturaMantenimiento;

public class GenerarFacturaMantenimientoHandler
{
    private readonly IAppDbContext _ctx;
    private const int PuntoVentaRecibidas = 2;

    public GenerarFacturaMantenimientoHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<int> HandleAsync(int idMantenimiento, GenerarFacturaMantenimientoCommand command)
    {
        var mant = await _ctx.MantenimientoRepo.FindByIdAsync(idMantenimiento, includeTaller: true, includeTarea: true);

        if (mant == null)
            throw new SeminarioException("El mantenimiento no existe", HttpStatusCode.NotFound);

        if (mant.FechaSalida == null)
            throw new SeminarioException("El mantenimiento no está terminado (sin fecha de salida)", HttpStatusCode.Conflict);

        if (mant.Suspendido)
            throw new SeminarioException("El mantenimiento está suspendido", HttpStatusCode.Conflict);

        if (mant.Tareas.Any(t => t.Costo == null || t.Costo <= 0))
            throw new SeminarioException("Existe una tarea del mantenimiento sin precio", HttpStatusCode.Conflict);

        var yaFacturado = _ctx.FacturaRepo.Query()
            .Any(f => !f.Anulada && f.FacturasMantenimiento.Any(fm => fm.IdMantenimiento == idMantenimiento));

        if (yaFacturado)
            throw new SeminarioException("El mantenimiento ya tiene una factura activa", HttpStatusCode.Conflict);

        var numero = await _ctx.FacturaRepo.ObtenerProximoNumeroAsync(Factura.TipoFactura.Recibida, PuntoVentaRecibidas);

        var factura = new Factura
        {
            Tipo = Factura.TipoFactura.Recibida,
            PuntoVenta = PuntoVentaRecibidas,
            Numero = numero,
            FechaEmision = DateTime.Today,
            FechaVencimiento = command.FechaVencimiento,
            PorcentajeIva = command.PorcentajeIva,
            IdMoneda = command.IdMoneda,
            Estado = Factura.EstadoFactura.Pendiente,
            Observaciones = command.Observaciones,
            Anulada = false,
            Confirmada = false,
            PuntoVentaReal = command.PtoVenta,
            NumeroReal = command.NumeroFactura,
            IdTaller = mant.IdTaller
        };

        var orden = 1;
        foreach (var tarea in mant.Tareas)
        {
            var iva = tarea.Costo.GetValueOrDefault() * (command.PorcentajeIva / 100m);

            var deta = new FacturaDetalle
            {
                Orden = orden++,
                Descripcion = $"Mantenimiento: {tarea.Descripcion}",
                Cantidad = 1,
                PrecioUnitario = tarea.Costo.GetValueOrDefault(),
                PorcentajeIva = command.PorcentajeIva,
                Subtotal = tarea.Costo.GetValueOrDefault(),
                Total = tarea.Costo.GetValueOrDefault() + iva
            };
            
            factura.Detalles.Add(deta);
        }

        factura.Subtotal = factura.Detalles.Sum(d => d.Subtotal);
        factura.Total = factura.Detalles.Sum(d => d.Total);
        
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
