using System.Net;
using Seminario.Core.Exceptions.SeminarioException;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.FacturasServices.FacturarV2.Models;

namespace Seminario.Services.FacturasServices.FacturarV2.Facturadores.FacturarViajes;

public class ValidarFacturarViajes
{
    private readonly IAppDbContext _ctx;

    public ValidarFacturarViajes(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Validar(FacturarViajeCommand model)
    {
        var facturada = model.Facturada;
        if (facturada)
        {
            var contiene = ContienePtoVtaYNumero(model);
            if (!contiene)
            {
                throw new SeminarioException("No se especifico el punto de venta y el numero de la factura",
                    HttpStatusCode.Conflict);
            }
        }

        var idCliente = model.IdCliente.GetValueOrDefault();
        var existe = _ctx.ClienteRepo.Existe(idCliente);
        if (!existe)
        {
            throw new SeminarioException("No se existe el cliente informado");
        }
        
        await ValidoDetalles(model);
    }

    private async Task ValidoDetalles(FacturarViajeCommand model)
    {
        var detalles = model.Detalles;
        if (!detalles.Any())
        {
            throw new SeminarioException("Debe informar al menos un detalle");
        }
        //
        foreach (var detalle in detalles)
        {
            var idViaje = detalle.IdViaje.GetValueOrDefault();
            
            var existe = _ctx.ViajeRepo.Existe(idViaje);
            if (!existe)
            {
                throw new SeminarioException($"No existe el viaje informado. Viaje id: {idViaje}");
            }
            
            var viaje = await _ctx.ViajeRepo.FindByIdAsync(idViaje);
            if (viaje == null)
            {
                throw new SeminarioException($"No existe el viaje informado. Viaje id: {idViaje}");
            }

            if (viaje.IdCliente != model.IdCliente)
            {
                throw new SeminarioException(
                    $"No conincide el cliente de la factura con el cliente del viaje, Viaje: {viaje.NroViaje}", HttpStatusCode.Conflict);
            }
        }
    }
    private static bool ContienePtoVtaYNumero(FacturarViajeCommand model)
    {
        return model.PuntoVentaReal != null && model.PuntoVentaReal > 0 && model.NumeroReal != null && model.NumeroReal > 0;
    }
}