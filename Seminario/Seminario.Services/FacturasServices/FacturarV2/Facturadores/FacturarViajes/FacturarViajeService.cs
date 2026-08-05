using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.FacturasServices.FacturarV2.Models;
using Seminario.Services.FacturasServices.FacturarV2.Servicios;

namespace Seminario.Services.FacturasServices.FacturarV2.Facturadores.FacturarViajes;

public class FacturarViajeService 
{
    private readonly IAppDbContext _ctx;
    
    private readonly ValidarFacturarViajes _validar;
    private readonly ArmoFacturaService _armoFacturaService;

    public FacturarViajeService(IAppDbContext ctx)
    {
        _ctx = ctx;
        _validar = new ValidarFacturarViajes(_ctx);
        _armoFacturaService = new ArmoFacturaService(ctx);
    }
    
    public async Task Facturar(FacturarViajeCommand model)
    {
        await _validar.Validar(model);

        var command = ArmoCabecera(model);

        await ArmoDetalles(command, model);
        
        await _armoFacturaService.ArmoFactura(command);
    }

    private FacturaModel ArmoCabecera(FacturarViajeCommand model)
    {
        return new()
        {
            Tipo = Factura.TipoFactura.Emitida,
            PuntoVentaReal = model.PuntoVentaReal,
            NumeroReal = model.NumeroReal,
            FechaEmision = model.FechaEmision,
            FechaVencimiento = model.FechaVencimiento,
            Moneda = model.Moneda,
            TipoCambio = model.TipoCambio,
            IdCliente = model.IdCliente
        };
    }
    
    private async Task ArmoDetalles(FacturaModel command, FacturarViajeCommand model)
    {
        command.Detalles = new List<FacturaDetalleModel>();
        
        var detalles = model.Detalles;
        foreach (var detalle in detalles)
        {
            var idViaje = detalle.IdViaje.GetValueOrDefault();
            var viaje = await _ctx.ViajeRepo.FindByIdAsync(idViaje);
            
            var deta =  new FacturaDetalleModel
            {
                Concepto = Concepto.Servicio,
                Descripcion = $"{viaje.NroViaje}",
                Cantidad = 1,
                Precio = detalle.Precio,
                PorcentajeIva = detalle.PorcentajeIva,
                CalcularIva = detalle.CalcularIva,
                CalcularTotal = detalle.CalcularTotal,
                IdViaje = viaje.IdViaje
            };
            
            command.Detalles.Add(deta);
        }
    }
}