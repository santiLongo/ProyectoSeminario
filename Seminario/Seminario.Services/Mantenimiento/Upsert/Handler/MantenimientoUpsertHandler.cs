using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.Mantenimiento.Upsert.Command;

namespace Seminario.Services.Mantenimiento.Upsert.Handler;

public class MantenimientoUpsertHandler
{
    private readonly IAppDbContext _ctx;

    public MantenimientoUpsertHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(MantenimientoUpsertCommand command)
    {
        var mantenimiento = await _ctx.MantenimientoRepo.FindByIdAsync(command.IdMantenimiento.GetValueOrDefault(), includeTarea: true);

        if (mantenimiento == null)
        {
            mantenimiento = new Datos.Entidades.Mantenimiento();
            _ctx.MantenimientoRepo.Add(mantenimiento);
            mantenimiento.IdVehiculo = mantenimiento.IdVehiculo;
        }

        if (mantenimiento.Suspendido)
        {
            throw new SeminarioException("No se puede modificar el mantenimiento, si esta suspendido",
                HttpStatusCode.NotModified);
        }
        
        if (mantenimiento.FechaSalida != null)
        {
            throw new SeminarioException("No se puede modificar un mantenimiento terminado",
                HttpStatusCode.NotModified);
        }
        
        mantenimiento.Titulo = command.Titulo;
        mantenimiento.IdTaller = command.IdTaller;
        mantenimiento.FechaEntrada = command.FechaEntrada;
        
        
        var remove = mantenimiento.Tareas
            .Where(e => !command.Tareas.Exists(c => c.IdTarea == e.IdTarea))
            .ToList();
        
        if (remove.Any())
        {
            _ctx.MantenimientoRepo.RemoveTareas(remove);
        }
        
        var tareas = mantenimiento.Tareas.ToList();

        foreach (var item in command.Tareas)
        {
            var tarea = mantenimiento.Tareas.FirstOrDefault(t => t.IdTarea == item.IdTarea);

            if (tarea == null)
            {
                tarea = new MantenimientoTarea();
                mantenimiento.Tareas.Add(tarea);
            }
            
            tarea.Descripcion = item.Descripcion;
            tarea.Costo = item.Costo;
        }

        await _ctx.SaveChangesAsync();
    }
}