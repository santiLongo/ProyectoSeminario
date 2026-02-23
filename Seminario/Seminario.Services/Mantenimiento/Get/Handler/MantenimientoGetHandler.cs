using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.Mantenimiento.Get.Command;
using Seminario.Services.Mantenimiento.Get.Response;
using Seminario.Services.Mantenimiento.Upsert.Command;
using Seminario.Services.Mantenimiento.Upsert.Handler;

namespace Seminario.Services.Mantenimiento.Get.Handler;

public class MantenimientoGetHandler
{
    private readonly IAppDbContext _ctx;
    
    public MantenimientoGetHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<MantenimientoGetResponse> HandleAsync(MantenimientoGetCommand command)
    {
        var mantenimiento = await _ctx.MantenimientoRepo.FindByIdAsync(command.IdMantenimiento, includeTarea: true);

        if (mantenimiento == null)
            throw new SeminarioException("No se encontro el mantenimiento", HttpStatusCode.NotFound);

        return new MantenimientoGetResponse
        {
            IdMantenimiento = mantenimiento.IdMantenimiento,
            Titulo = mantenimiento.Titulo,
            IdCamion = mantenimiento.IdVehiculo,
            FechaEntrada = mantenimiento.FechaEntrada,
            IdTaller = mantenimiento.IdTaller,
            Tareas = mantenimiento.Tareas.Select(t => new MantenimientoGetTareas
            {
                IdTarea = t.IdTarea,
                Descripcion = t.Descripcion,
                Costo = t.Costo
            }).ToList()
        };
    }
}