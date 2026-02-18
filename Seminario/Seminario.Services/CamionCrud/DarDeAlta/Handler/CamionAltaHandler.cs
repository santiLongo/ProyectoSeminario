using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.CamionCrud.DarDeAlta.Command;

namespace Seminario.Services.CamionCrud.DarDeAlta.Handler;

public class CamionAltaHandler
{
    private readonly IAppDbContext _ctx;
    
    public CamionAltaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(CamionAltaCommand command)
    {
        var camion = await _ctx.CamionRepo.GetCamionByIdAsync(command.Id);
        //
        if (camion == null)
            throw new SeminarioException("No se encontro el camion buscado", HttpStatusCode.NotFound);
        //
        camion.FechaBaja =  null;
        camion.FechaAlta = DateTime.Today;
        await _ctx.SaveChangesAsync();
    }
}