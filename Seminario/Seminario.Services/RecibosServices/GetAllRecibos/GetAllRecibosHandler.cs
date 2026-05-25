using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Repositorios;

namespace Seminario.Services.RecibosServices.GetAllRecibos;

public class GetAllRecibosHandler
{
    private readonly IAppDbContext _ctx;

    public GetAllRecibosHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IList<GetAllRecibosResponse>> HandleAsync(GetAllRecibosCommand command)
    {
        var query = _ctx.ReciboRepo.Query()
            .IncludeFacturas()
            .IncludeContraparte();

        if (command.TipoRecibo.HasValue)
            query = query.Where(r => (int)r.Tipo == command.TipoRecibo.Value);

        if (command.IdCliente.HasValue)
            query = query.Where(r => r.IdCliente == command.IdCliente.Value);

        if (command.IdProveedor.HasValue)
            query = query.Where(r => r.IdProveedor == command.IdProveedor.Value);

        if (command.IdTaller.HasValue)
            query = query.Where(r => r.IdTaller == command.IdTaller.Value);

        if (command.Anulado.HasValue)
            query = query.Where(r => r.Anulado == command.Anulado.Value);

        if (command.FechaDesde.HasValue)
            query = query.Where(r => r.FechaRecibo >= command.FechaDesde.Value);

        if (command.FechaHasta.HasValue)
            query = query.Where(r => r.FechaRecibo <= command.FechaHasta.Value);

        var recibos = await query.ToListAsync();

        var result = recibos.Select(r =>
        {
            string? nombreContraparte = null;
            if (r.Cliente != null)
                nombreContraparte = r.Cliente.RazonSocial;
            else if (r.Proveedor != null)
                nombreContraparte = r.Proveedor.RazonSocial;
            else if (r.Taller != null)
                nombreContraparte = r.Taller.Nombre;

            return new GetAllRecibosResponse
            {
                IdRecibo = r.IdRecibo,
                Tipo = (int)r.Tipo,
                FechaRecibo = r.FechaRecibo,
                MontoTotal = r.MontoTotal,
                Anulado = r.Anulado,
                NombreContraparte = nombreContraparte,
                CantFacturasImputadas = r.ReciboFacturas?.Count ?? 0
            };
        }).ToList();

        return result;
    }
}
