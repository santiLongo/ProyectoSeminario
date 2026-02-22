using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.CobrosServices.Get.Command;
using Seminario.Services.CobrosServices.Get.Response;

namespace Seminario.Services.CobrosServices.Get.Handler;

public class CobrosGetHandler
{
    private readonly IAppDbContext _ctx;

    public CobrosGetHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<CobrosGetResponse> HandleAsync(CobrosGetCommand command)
    {
        var cobro = await _ctx.CobrosRepo.FindByIdCobroAsync(command.IdCobro, includeCheque: true);

        if (cobro == null)
            throw new SeminarioException("No se encontro el cobro", HttpStatusCode.NotFound);

        var response = new CobrosGetResponse
        {
            IdCobro = cobro.IdCobro,
            Monto = cobro.Monto,
            IdMoneda = cobro.IdMoneda,
            TipoCambio = cobro.TipoCambio,
            IdFormaPago = cobro.IdFormaPago,
        };
        
        if(cobro.Cheque != null)
        {
            response.DatosCheque = new DatosCobroCheque
            {
                IdCheque = cobro.IdPagoCheque,
                NroCheque = cobro.Cheque.NroCheque,
                CuitEmisor = cobro.Cheque.CuitEmisor,
                IdBanco = cobro.Cheque.IdBanco,
                FechaCobro = cobro.Cheque.FechaCobro,
            };
        }
        
        return response;
    }
}