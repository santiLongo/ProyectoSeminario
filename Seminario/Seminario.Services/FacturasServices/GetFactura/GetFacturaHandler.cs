using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Repositorios;

namespace Seminario.Services.FacturasServices.GetFactura;

public class GetFacturaHandler
{
    private readonly IAppDbContext _ctx;

    public GetFacturaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<GetFacturaResponse> HandleAsync(GetFacturaCommand command)
    {
        var factura = await _ctx.FacturaRepo.Query()
            .IncludeDetalles()
            .IncludeViajes()
            .IncludeMantenimientos()
            .IncludeCompras()
            .IncludeRecibos()
            .IncludeContraparte()
            .FirstOrDefaultAsync(f => f.IdFactura == command.IdFactura!.Value);

        if (factura == null)
            throw new InvalidOperationException("La factura no existe");

        var totalAplicado = factura.ReciboFacturas
            .Where(rf => rf.Recibo != null && !rf.Recibo.Anulado)
            .Sum(rf => rf.ImporteAplicado);

        string? nombreContraparte = null;
        if (factura.Cliente != null)
            nombreContraparte = factura.Cliente.RazonSocial;
        else if (factura.Proveedor != null)
            nombreContraparte = factura.Proveedor.RazonSocial;
        else if (factura.Taller != null)
            nombreContraparte = factura.Taller.Nombre;

        var response = new GetFacturaResponse
        {
            IdFactura = factura.IdFactura,
            Tipo = (int)factura.Tipo,
            PuntoVenta = factura.PuntoVenta,
            Numero = factura.Numero,
            FechaEmision = factura.FechaEmision,
            FechaVencimiento = factura.FechaVencimiento,
            Subtotal = factura.Subtotal,
            PorcentajeIva = factura.PorcentajeIva,
            Total = factura.Total,
            IdMoneda = factura.IdMoneda,
            TipoCambio = factura.TipoCambio,
            Estado = (int)factura.Estado,
            Observaciones = factura.Observaciones,
            Anulada = factura.Anulada,
            Confirmada = factura.Confirmada,
            TipoComprobante = factura.TipoComprobante,
            CAE = factura.CAE,
            CAEFchVto = factura.CAEFchVto,
            IdCliente = factura.IdCliente,
            IdProveedor = factura.IdProveedor,
            IdTaller = factura.IdTaller,
            NombreContraparte = nombreContraparte,
            SaldoPendiente = factura.Total - totalAplicado,
            Detalles = factura.Detalles.OrderBy(d => d.Orden).Select(d => new GetFacturaDetalleResponse
            {
                IdFacturaDetalle = d.IdFacturaDetalle,
                Orden = d.Orden,
                Descripcion = d.Descripcion,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                PorcentajeIva = d.PorcentajeIva,
                Subtotal = d.Subtotal,
                Total = d.Total
            }).ToList(),
            Viajes = factura.FacturasViaje.Select(fv => new GetFacturaViajeResponse
            {
                IdFacturaViaje = fv.IdFacturaViaje,
                IdViaje = fv.IdViaje,
                NroViaje = fv.Viaje?.NroViaje,
                Cliente = fv.Viaje?.Cliente?.RazonSocial,
                MontoViaje = fv.MontoViaje
            }).ToList(),
            Mantenimientos = factura.FacturasMantenimiento.Select(fm => new GetFacturaMantenimientoResponse
            {
                IdFacturaMantenimiento = fm.IdFacturaMantenimiento,
                IdMantenimiento = fm.IdMantenimiento,
                Titulo = fm.Mantenimiento?.Titulo,
                ImporteMantenimiento = fm.ImporteMantenimiento
            }).ToList(),
            CompraRepuestos = factura.FacturasCompraRepuesto.Select(fc => new GetFacturaCompraResponse
            {
                IdFacturaCompraRepuesto = fc.IdFacturaCompraRepuesto,
                IdCompraRepuesto = fc.IdCompraRepuesto,
                ImporteCompra = fc.ImporteCompra
            }).ToList(),
            RecibosImputados = factura.ReciboFacturas.Select(rf => new GetFacturaReciboResponse
            {
                IdReciboFactura = rf.IdReciboFactura,
                IdRecibo = rf.IdRecibo,
                FechaRecibo = rf.Recibo?.FechaRecibo ?? DateTime.MinValue,
                ImporteAplicado = rf.ImporteAplicado,
                ReciboAnulado = rf.Recibo?.Anulado ?? false
            }).ToList()
        };

        return response;
    }
}
