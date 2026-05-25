using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Repositorios;

namespace Seminario.Services.RecibosServices.GetRecibo;

public class GetReciboHandler
{
    private readonly IAppDbContext _ctx;

    public GetReciboHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<GetReciboResponse> HandleAsync(GetReciboCommand command)
    {
        var recibo = await _ctx.ReciboRepo.Query()
            .IncludeFormasDePago()
            .IncludeFacturas()
            .IncludeContraparte()
            .FirstOrDefaultAsync(r => r.IdRecibo == command.IdRecibo!.Value);

        if (recibo == null)
            throw new InvalidOperationException("El recibo no existe");

        string? nombreContraparte = null;
        if (recibo.Cliente != null)
            nombreContraparte = recibo.Cliente.RazonSocial;
        else if (recibo.Proveedor != null)
            nombreContraparte = recibo.Proveedor.RazonSocial;
        else if (recibo.Taller != null)
            nombreContraparte = recibo.Taller.Nombre;

        var response = new GetReciboResponse
        {
            IdRecibo = recibo.IdRecibo,
            Tipo = (int)recibo.Tipo,
            FechaRecibo = recibo.FechaRecibo,
            MontoTotal = recibo.MontoTotal,
            IdMoneda = recibo.IdMoneda,
            TipoCambio = recibo.TipoCambio.HasValue ? (decimal?)recibo.TipoCambio.Value : null,
            IdCliente = recibo.IdCliente,
            IdProveedor = recibo.IdProveedor,
            IdTaller = recibo.IdTaller,
            NombreContraparte = nombreContraparte,
            Observaciones = recibo.Observaciones,
            Anulado = recibo.Anulado,
            FormasDePago = recibo.FormasDePago.Select(fp => new GetReciboFormaPagoResponse
            {
                IdReciboFormaPago = fp.IdReciboFormaPago,
                IdFormaPago = fp.IdFormaPago,
                DescripcionFormaPago = fp.FormaPago?.Descripcion,
                Monto = fp.Monto,
                IdPagoCheque = fp.IdPagoCheque,
                NroCheque = fp.PagoCheque?.NroCheque,
                CuitEmisor = fp.PagoCheque?.CuitEmisor,
                IdBanco = fp.PagoCheque?.IdBanco,
                FechaCobro = fp.PagoCheque?.FechaCobro,
                EsPropio = fp.PagoCheque?.EsPropio
            }).ToList(),
            FacturasImputadas = recibo.ReciboFacturas.Select(rf => new GetReciboFacturaResponse
            {
                IdReciboFactura = rf.IdReciboFactura,
                IdFactura = rf.IdFactura,
                PuntoVenta = rf.Factura?.PuntoVenta ?? 0,
                Numero = rf.Factura?.Numero ?? 0,
                FechaEmision = rf.Factura?.FechaEmision ?? DateTime.MinValue,
                ImporteAplicado = rf.ImporteAplicado
            }).ToList()
        };

        return response;
    }
}
