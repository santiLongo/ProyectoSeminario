using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.Mantenimiento.InformarSalida.Command;

namespace Seminario.Services.Mantenimiento.InformarSalida.Handler;

public class MantenimientoInformarSalidaHandler
{
    private readonly IAppDbContext _ctx;

    public MantenimientoInformarSalidaHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(MantenimientoInformarSalidaCommand command)
    {
        var mantenimiento = await _ctx.MantenimientoRepo.FindByIdAsync(command.IdMantenimiento, includeObs: true);
        
        if (mantenimiento == null)
            throw new SeminarioException("No se encontro el mantenimiento", HttpStatusCode.NotFound);

        if (mantenimiento.Suspendido)
            throw new SeminarioException("El mantenimiento se encuentra suspendidos", HttpStatusCode.Conflict);

        if (mantenimiento.FechaSalida != null)
            throw new SeminarioException("El mantenimiento ya esta finalizado", HttpStatusCode.Conflict);

        if (command.FechaSalida >= DateTime.Now)
            throw new SeminarioException("No se puede informar una fecha de salida susperior a la de hoy",  HttpStatusCode.Ambiguous);
        
        mantenimiento.FechaSalida = command.FechaSalida;
        mantenimiento.Observaciones.Add(new MantenimientoObservacion
        {
            Observacion = !string.IsNullOrEmpty(command.Observacion) ? command.Observacion : $"Informo la fecha de salida: {command.FechaSalida}",
        });
        await _ctx.SaveChangesAsync();
    }
}