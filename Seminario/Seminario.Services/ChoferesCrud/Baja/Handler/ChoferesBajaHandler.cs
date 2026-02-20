using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.ChoferesCrud.Baja.Command;

namespace Seminario.Services.ChoferesCrud.Baja.Handler;

public class ChoferesBajaHandler
{
    private readonly IAppDbContext _ctx;
    
    public ChoferesBajaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(ChoferesBajaCommand command)
    {
        var chofer = await _ctx.ChoferRepo.GetAsync(command.IdChofer);

        if (chofer == null)
            throw new SeminarioException("No se encontro el chofer", HttpStatusCode.NotFound);
        
        chofer.FechaBaja = DateTime.Today;
        await _ctx.SaveChangesAsync();
    }
}