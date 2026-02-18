using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.CamionCrud.DarDeBaja.Command;

namespace Seminario.Services.CamionCrud.DarDeBaja.Handler;

public class CamionBajaHandler
{
    private readonly IAppDbContext _ctx;
    
    public CamionBajaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(CamionBajaCommand command)
    {
        var camion = await _ctx.CamionRepo.GetCamionByIdAsync(command.Id);
        //
        if (camion == null)
            throw new SeminarioException("No se encontro el camion buscado", HttpStatusCode.NotFound);
        //
        camion.FechaBaja =  DateTime.Now;
        await _ctx.SaveChangesAsync();
    }
}