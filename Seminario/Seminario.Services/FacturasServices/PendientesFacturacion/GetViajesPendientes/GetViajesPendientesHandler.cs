using Microsoft.EntityFrameworkCore;
using Seminario.Datos;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Enums;

namespace Seminario.Services.FacturasServices.PendientesFacturacion.GetViajesPendientes;

public class GetViajesPendientesHandler
{
    private readonly IAppDbContext _ctx;

    public GetViajesPendientesHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IList<GetViajesPendientesResponse>> HandleAsync()
    {
        var viajesYaFacturados = _ctx.FacturaRepo.Query()
            .Where(f => !f.Anulada && f.Tipo == Factura.TipoFactura.Emitida)
            .SelectMany(f => f.FacturasViaje.Select(fv => fv.IdViaje));

        var viajes = await _ctx.ViajeRepo.Query()
            .Include(v => v.Cliente)
            .Include(v => v.Chofer)
            .Include(v => v.Camion)
            .Where(v => v.Estado == EstadosViaje.Finalizado.ToInt() || v.Estado == EstadosViaje.Cobrado.ToInt())
            .Where(v => !viajesYaFacturados.Contains(v.IdViaje))
            .OrderBy(v => v.FechaDescarga)
            .ToListAsync();

        return viajes.Select(v => new GetViajesPendientesResponse
        {
            IdViaje = v.IdViaje,
            NroViaje = v.NroViaje,
            FechaPartida = v.FechaPartida,
            FechaDescarga = v.FechaDescarga,
            Estado = v.Estado ?? 0,
            MontoTotal = v.MontoTotal,
            IdCliente = v.IdCliente,
            RazonSocialCliente = v.Cliente?.RazonSocial,
            NombreChofer = $"{v.Chofer?.Nombre} {v.Chofer?.Apellido}".Trim(),
            PatenteCamion = v.Camion?.Patente
        }).ToList();
    }
}
