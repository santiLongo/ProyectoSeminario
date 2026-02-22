using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.CobrosServices.Anular.Command;

namespace Seminario.Services.CobrosServices.Anular.Handler;

public class CobrosAnularHandler
{
    private readonly IAppDbContext _ctx;
    
    public CobrosAnularHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(CobrosAnularCommand command)
    {
        var cobro = await _ctx.CobrosRepo.FindByIdCobroAsync(command.IdCobro, includeCheque: true);

        if (cobro == null)
            throw new SeminarioException("No se encontro el cobro", HttpStatusCode.NotFound);
        
        if(cobro.CobroAnulado != null)
            throw new SeminarioException("No se puede anular un cobro ya anulado", HttpStatusCode.Conflict);

        if (cobro.Cheque != null)
        {
            var pago = await _ctx.PagosRepo.FindByChequeAsync(cobro.IdPagoCheque.GetValueOrDefault());
            if (pago != null)
            {
                throw new SeminarioException("No se puede anular un cobro que tiene un cheque utilizado para pagar", HttpStatusCode.Conflict);
            }
        }
        
        await _ctx.CobrosRepo.Anular(cobro);
    }
}