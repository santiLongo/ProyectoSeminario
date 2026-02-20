using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.ChoferesCrud.Alta.Command;

namespace Seminario.Services.ChoferesCrud.Alta.Handler;

public class ChoferesAltaHandler
{
    private readonly IAppDbContext _ctx;
    
    public ChoferesAltaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(ChoferesAltaCommand command)
    {
        var chofer = await _ctx.ChoferRepo.GetAsync(command.IdChofer);

        if (chofer == null)
            throw new SeminarioException("No se encontro el chofer", HttpStatusCode.NotFound);
        
        chofer.FechaBaja = null;
        chofer.FechaAlta = DateTime.Today;
        await _ctx.SaveChangesAsync();
    }
}