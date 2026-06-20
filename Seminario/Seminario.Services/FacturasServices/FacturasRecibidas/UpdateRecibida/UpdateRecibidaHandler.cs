using System.Net;
using Microsoft.EntityFrameworkCore;
using Seminario.Core.Exceptions.SeminarioException;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;
using Seminario.Services.FacturasServices.CrearFactura;

namespace Seminario.Services.FacturasServices.FacturasRecibidas.UpdateRecibida;

public class UpdateRecibidaHandler
{
    private readonly IAppDbContext _ctx;

    public UpdateRecibidaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(int idFactura, UpdateRecibidaCommand command)
    {
        if (command.IdProveedor == null && command.IdTaller == null)
            throw new SeminarioException("Debe informar un proveedor o un taller", HttpStatusCode.BadRequest);

        if (command.Detalles == null || command.Detalles.Count == 0)
            throw new SeminarioException("Debe informar al menos un detalle", HttpStatusCode.BadRequest);

        var factura = await _ctx.FacturaRepo.Query()
            .IncludeDetalles()
            .FirstOrDefaultAsync(f => f.IdFactura == idFactura);

        if (factura == null)
            throw new SeminarioException("La factura no existe", HttpStatusCode.NotFound);

        if (factura.Tipo != Factura.TipoFactura.Recibida)
            throw new SeminarioException("Solo se pueden editar facturas recibidas con este endpoint", HttpStatusCode.BadRequest);

        if (factura.Anulada)
            throw new SeminarioException("No se puede editar una factura anulada", HttpStatusCode.Conflict);

        if (factura.Confirmada)
            throw new SeminarioException("No se puede editar una factura ya confirmada", HttpStatusCode.Conflict);

        factura.IdProveedor = command.IdProveedor;
        factura.IdTaller = command.IdTaller;
        factura.FechaEmision = command.FechaEmision;
        factura.FechaVencimiento = command.FechaVencimiento;
        factura.PorcentajeIva = command.PorcentajeIva;
        factura.IdMoneda = command.IdMoneda;
        factura.TipoCambio = command.TipoCambio;
        factura.Observaciones = command.Observaciones;

        factura.Detalles.Clear();
        var (subtotal, totalConIva, detalles) = FacturaCalculos.CalcularDetalles(command.Detalles);
        foreach (var detalle in detalles)
            factura.Detalles.Add(detalle);

        factura.Subtotal = subtotal;
        factura.Total = totalConIva;

        await _ctx.SaveChangesAsync();
    }
}
