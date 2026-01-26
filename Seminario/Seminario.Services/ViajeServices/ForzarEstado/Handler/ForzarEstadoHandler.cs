using Microsoft.EntityFrameworkCore;
using Seminario.Datos;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;
using Seminario.Services.ViajeServices.ForzarEstado.Command;

namespace Seminario.Services.ViajeServices.ForzarEstado.Handler;

public class ForzarEstadoHandler
{
    private readonly IAppDbContext _ctx;

    public ForzarEstadoHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(ForzarEstadoCommand command)
    {
        if (!command.Estado.ExisteEstadoViaje())
        {
            throw new InvalidOperationException("El estado no se encontro");
        }
        
        if (!command.Seguridad)
        {
            throw new InvalidOperationException("Para forzar el estado es necesario tener seguridad");
        }
        
        var viaje = await _ctx.ViajeRepo.Query()
            .FirstOrDefaultAsync(v => v.IdViaje == command.IdViaje);

        if (viaje == null)
        {
            throw new InvalidOperationException("El viaje no se encontro ");
        }
        
        if (viaje.Estado == EstadosViaje.Cobrado.ToInt())
        {
            throw new InvalidOperationException("El viaje esta cobrado en su totalidad, no es forzable");
        }
        
        if (viaje.Estado == EstadosViaje.Suspendido.ToInt() && command.Estado != EstadosViaje.EnViaje.ToInt())
        {
            throw new InvalidOperationException("El viaje se encuentra suspendido y solo se puede pasar a en viaje");
        }
        
        if (command.Estado == EstadosViaje.EnViaje.ToInt())
        {
            await VerificarCamionYChofer(viaje);
            viaje.FechaDescarga = null;
        }
        
        if ((command.Estado == EstadosViaje.Finalizado.ToInt() 
             || command.Estado == EstadosViaje.Cobrado.ToInt()) 
            && viaje.Estado == EstadosViaje.EnViaje.ToInt())
        {
            viaje.FechaDescarga = DateTime.Today;
        }
        
        viaje.Estado = command.Estado;
        await _ctx.SaveChangesAsync();
    }

    private async Task VerificarCamionYChofer(Viaje viaje)
    {
        var camion = await _ctx.CamionRepo.GetAsync(q =>
            q.AsNoTracking()
                .IncludeCurrentViaje()
                .IncludeMantenimientoActual()
                .WhereEqualsIdCamion(viaje.IdCamion)
            );

        if (camion == null) throw new InvalidOperationException("Ups, parece que el camion no existe");
        //
        if(camion.Viajes.Any()) throw new InvalidOperationException("El camion se encuentra en un viaje");
        //
        if(camion.Mantenimientos.Any()) throw new InvalidOperationException("El camion se encuentra en un mantenimiento");
        
        var chofer = await _ctx.ChoferRepo.Query()
            .AsNoTracking()
            .ConViajesNoFinalizados()
            .FirstOrDefaultAsync(c => c.IdChofer == viaje.IdChofer);
        
        if (chofer == null) throw new InvalidOperationException("Ups, parece que el camion no existe");
        //
        if(chofer.Viajes.Any()) throw new InvalidOperationException("El chofer se encuentra en un viaje");
    }
}