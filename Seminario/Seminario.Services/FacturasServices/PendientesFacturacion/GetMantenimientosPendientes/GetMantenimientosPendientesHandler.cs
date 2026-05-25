using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;

namespace Seminario.Services.FacturasServices.PendientesFacturacion.GetMantenimientosPendientes;

public class GetMantenimientosPendientesHandler
{
    private readonly IAppDbContext _ctx;

    public GetMantenimientosPendientesHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IList<GetMantenimientosPendientesResponse>> HandleAsync()
    {
        var mantenimientosYaFacturados = _ctx.FacturaRepo.Query()
            .Where(f => !f.Anulada && f.Tipo == Factura.TipoFactura.Recibida)
            .SelectMany(f => f.FacturasMantenimiento.Select(fm => fm.IdMantenimiento));

        var mantenimientos = await _ctx.MantenimientoRepo.Query()
            .Include(m => m.Taller)
            .Include(m => m.Vehiculo)
            .Where(m => m.FechaSalida != null && !m.Suspendido)
            .Where(m => !mantenimientosYaFacturados.Contains(m.IdMantenimiento))
            .OrderBy(m => m.FechaSalida)
            .ToListAsync();

        return mantenimientos.Select(m => new GetMantenimientosPendientesResponse
        {
            IdMantenimiento = m.IdMantenimiento,
            Titulo = m.Titulo,
            FechaEntrada = m.FechaEntrada,
            FechaSalida = m.FechaSalida,
            Importe = m.Importe,
            IdTaller = m.IdTaller,
            NombreTaller = m.Taller?.Nombre,
            PatenteCamion = m.Vehiculo?.Patente
        }).ToList();
    }
}
