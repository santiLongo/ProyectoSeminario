using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;

namespace Seminario.Services.FacturasServices.GetAllFacturas;

public class GetAllFacturasHandler
{
    private readonly IAppDbContext _ctx;

    public GetAllFacturasHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<IList<GetAllFacturasResponse>> HandleAsync(GetAllFacturasCommand command)
    {
        var query = _ctx.FacturaRepo.Query()
            .IncludeRecibos()
            .IncludeContraparte();

        if (command.TipoFactura.HasValue)
            query = query.Where(f => (int)f.Tipo == command.TipoFactura.Value);

        if (command.IdCliente.HasValue)
            query = query.Where(f => f.IdCliente == command.IdCliente.Value);

        if (command.IdProveedor.HasValue)
            query = query.Where(f => f.IdProveedor == command.IdProveedor.Value);

        if (command.IdTaller.HasValue)
            query = query.Where(f => f.IdTaller == command.IdTaller.Value);

        if (command.Estado.HasValue)
            query = query.Where(f => (int)f.Estado == command.Estado.Value);

        if (command.FechaDesde.HasValue)
            query = query.Where(f => f.FechaEmision >= command.FechaDesde.Value);

        if (command.FechaHasta.HasValue)
            query = query.Where(f => f.FechaEmision <= command.FechaHasta.Value);

        var facturas = await query.ToListAsync();

        var result = facturas.Select(f =>
        {
            var totalAplicado = f.ReciboFacturas
                .Where(rf => rf.Recibo != null && !rf.Recibo.Anulado)
                .Sum(rf => rf.ImporteAplicado);

            string? nombreContraparte = null;
            if (f.Cliente != null)
                nombreContraparte = f.Cliente.RazonSocial;
            else if (f.Proveedor != null)
                nombreContraparte = f.Proveedor.RazonSocial;
            else if (f.Taller != null)
                nombreContraparte = f.Taller.Nombre;

            return new GetAllFacturasResponse
            {
                IdFactura = f.IdFactura,
                Tipo = (int)f.Tipo,
                PuntoVenta = f.PuntoVenta,
                Numero = f.Numero,
                FechaEmision = f.FechaEmision,
                FechaVencimiento = f.FechaVencimiento,
                Total = f.Total,
                Estado = (int)f.Estado,
                Anulada = f.Anulada,
                Confirmada = f.Confirmada,
                CAE = f.CAE,
                NombreContraparte = nombreContraparte,
                SaldoPendiente = f.Total - totalAplicado
            };
        }).ToList();

        return result;
    }
}
