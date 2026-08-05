using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.FacturasServices.FacturarV2.Models;

namespace Seminario.Services.FacturasServices.FacturarV2.Servicios;

public class ArmoFacturaService
{
    private readonly IAppDbContext _ctx;

    public ArmoFacturaService(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task ArmoFactura(FacturaModel model)
    {
        var factura = ArmoCabecera(model);

        ArmoDetalle(factura, model);

        factura.RecalculoCabecera();
        
        _ctx.FacturaRepo.Add(factura);

        await _ctx.SaveChangesAsync();
    }
    private Factura ArmoCabecera(FacturaModel model)
    {
        return new Factura
        {
            Tipo = model.Tipo,
            FechaEmision = model.FechaEmision,
            FechaVencimiento = model.FechaVencimiento,
            Subtotal = 0,
            ImporteIva = 0,
            Total = 0,
            IdMoneda = model.Moneda,
            TipoCambio = model.TipoCambio,
            Observaciones = string.Empty,
            IdCliente = model.IdCliente,
            IdProveedor = model.IdProveedor,
            IdTaller = model.IdTaller,
            PuntoVentaReal = model.PuntoVentaReal,
            NumeroReal = model.NumeroReal,
        };
    }
    
    private void ArmoDetalle(Factura factura, FacturaModel model)
    {
        factura.Detalles = new List<FacturaDetalle>();

        var detalles = model.Detalles;
        foreach (var detalle in detalles)
        {
            var precio  = detalle.Precio;
            var porcentajeIva = detalle.PorcentajeIva;

            var subTotal = CalculoSubTotal(detalle);
            var precioIva = CalcularIva(detalle, subTotal);
            var total = CalculoTotal(detalle, subTotal, precioIva);
                
            var deta = new FacturaDetalle
            {
                Orden = (int)detalle.Concepto,
                Descripcion = detalle.Descripcion,
                Cantidad = detalle.Cantidad,
                PrecioUnitario = precio,
                PorcentajeIva = porcentajeIva,
                PrecioIva = precioIva,
                Subtotal = subTotal,
                Total = total,
                IdViaje = detalle.IdViaje,
                IdMantenimiento = detalle.IdMantenimiento,
                IdCompraRepuesto = detalle.IdCompra,
            };
            
            factura.Detalles.Add(deta);
        }
    }
    private decimal CalcularIva(FacturaDetalleModel detalle, decimal subTotal)
    {
        return (subTotal * detalle.PorcentajeIva) / 100;
    }
    
    private decimal CalculoSubTotal(FacturaDetalleModel detalle)
    {
        return detalle.Cantidad * detalle.Precio;
    }
    
    private decimal CalculoTotal(FacturaDetalleModel detalle, decimal subTotal, decimal precioIva)
    {
        return subTotal + precioIva;
    }
}