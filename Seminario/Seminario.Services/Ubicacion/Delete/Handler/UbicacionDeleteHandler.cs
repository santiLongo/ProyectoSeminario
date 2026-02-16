using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.Ubicacion.Delete.Command;

namespace Seminario.Services.Ubicacion.Delete.Handler;

public class UbicacionDeleteHandler
{
    private readonly IAppDbContext _ctx;
    
    public UbicacionDeleteHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(UbicacionDeleteCommand command)
    {
        var localidad = await _ctx.UbicacionRepo.GetLocalidadByIdAsync(command.Id);
        
        if (localidad == null)
            throw new SeminarioException("No se encontro el localidad para borrar", HttpStatusCode.NotFound);
        
        _ctx.UbicacionRepo.Remove(localidad);
        await _ctx.SaveChangesAsync();
    }
}