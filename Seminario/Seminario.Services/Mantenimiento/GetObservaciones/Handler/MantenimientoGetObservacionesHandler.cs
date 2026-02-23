using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.Mantenimiento.GetObservaciones.Command;
using Seminario.Services.Mantenimiento.GetObservaciones.Response;

namespace Seminario.Services.Mantenimiento.GetObservaciones.Handler;

public class MantenimientoGetObservacionesHandler
{
    private readonly IAppDbContext _ctx;
    
    public MantenimientoGetObservacionesHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<MantenimientoGetObservacionesResponse>> HandleAsync(
        MantenimientoGetObservacionesCommand command)
    {
        var mantenimiento = await _ctx.MantenimientoRepo.FindByIdAsync(command.IdMantenimiento, includeObs: true);

        if (mantenimiento == null)
            throw new SeminarioException("No se encontro el mantenimiento", HttpStatusCode.NotFound);

        return mantenimiento.Observaciones.Select(o => new MantenimientoGetObservacionesResponse
        {
            IdObservacion = o.IdObservacion,
            IdMantenimiento = o.IdMantenimiento,
            Descripcion = o.Observacion,
            UserDateTime = o.UserDateTime,
            UserName = o.UserName,
        }).ToList();
    }
}