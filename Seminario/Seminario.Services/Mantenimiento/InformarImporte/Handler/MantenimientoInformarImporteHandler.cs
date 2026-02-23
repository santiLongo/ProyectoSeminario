using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.Mantenimiento.InformarImporte.Command;

namespace Seminario.Services.Mantenimiento.InformarImporte.Handler;

public class MantenimientoInformarImporteHandler
{
    private readonly IAppDbContext _ctx;
    
    public MantenimientoInformarImporteHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(MantenimientoInformarImporteCommand command)
    {
        var mantenimiento = await _ctx.MantenimientoRepo.FindByIdAsync(command.IdMantenimiento, includeTarea: true, includeObs: true);
        
        if (mantenimiento == null)
            throw new SeminarioException("No se encontro el mantenimiento", HttpStatusCode.NotFound);

        if (mantenimiento.Suspendido)
        {
            throw new SeminarioException("El mantenimiento se encuentra suspendido");
        }

        var totalPago = await _ctx.MantenimientoRepo.TotalDePago(mantenimiento.IdMantenimiento);
        
        if(mantenimiento.Importe.GetValueOrDefault() == totalPago && mantenimiento.Importe.GetValueOrDefault() > 0)
            throw new SeminarioException("El mantenimiento ya se encuentra pagado", HttpStatusCode.Locked);

        var totalTareas = mantenimiento.Tareas.Sum(t => t.Costo);

        if (command.Importe != totalTareas)
            throw new SeminarioException("El importe debe coincidir con el total de los costos de las tareas",
                HttpStatusCode.Ambiguous);
        
        mantenimiento.Importe =  command.Importe;
        mantenimiento.Observaciones.Add(new MantenimientoObservacion
        {
            Observacion = !string.IsNullOrEmpty(command.Observacion) ? command.Observacion : $"Informo Importe: {command.Importe}",
            Fecha = DateTime.Today,
        });
        await _ctx.SaveChangesAsync();
    }
}