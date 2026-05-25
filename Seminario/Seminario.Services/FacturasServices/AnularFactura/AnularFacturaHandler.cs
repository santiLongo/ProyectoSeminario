using Microsoft.EntityFrameworkCore;
using Seminario.Datos;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;

namespace Seminario.Services.FacturasServices.AnularFactura;

public class AnularFacturaHandler
{
    private readonly IAppDbContext _ctx;

    public AnularFacturaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(AnularFacturaCommand command)
    {
        var factura = await _ctx.FacturaRepo.Query()
            .IncludeViajes()
            .IncludeRecibos()
            .FirstOrDefaultAsync(f => f.IdFactura == command.IdFactura!.Value);

        if (factura == null)
            throw new InvalidOperationException("La factura no existe");

        if (factura.Anulada)
            throw new InvalidOperationException("La factura ya se encuentra anulada");

        // Verificar que no tiene recibos aplicados activos
        var tieneRecibosActivos = factura.ReciboFacturas
            .Any(rf => rf.Recibo != null && !rf.Recibo.Anulado);

        if (tieneRecibosActivos)
            throw new InvalidOperationException("No se puede anular la factura porque tiene recibos aplicados");

        // Marcar como anulada
        factura.Anulada = true;
        factura.Estado = Factura.EstadoFactura.Anulada;

        // Revertir estado de viajes vinculados a Finalizado
        foreach (var fv in factura.FacturasViaje)
        {
            if (fv.Viaje != null)
            {
                fv.Viaje.Estado = EstadosViaje.Finalizado.ToInt();
            }
        }

        await _ctx.SaveChangesAsync();
    }
}
