using Microsoft.EntityFrameworkCore;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;
using Seminario.Services.ViajeServices.Update.Command;

namespace Seminario.Services.ViajeServices.Update.Handler;

public class UpdateViajeHandler
{
    private readonly IAppDbContext _ctx;

    public UpdateViajeHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(UpdateViajeCommand command)
    {
        var viaje = await _ctx.ViajeRepo.Query()
            .IncludeDestinosProcedencias()
            .IncludeCamion()
            .IncludeChofer()
            .WhereEnViaje()
            .FirstOrDefaultAsync(v => v.IdViaje == command.IdViaje);

        if (viaje == null)
        {
            throw new InvalidOperationException($"El viaje no existe o se encuentra finalizado");
        }

        var camion = viaje.IdCamion ==  command.Camion ? viaje.Camion : await ValidarCamion(command.Camion);
        
        var chofer = viaje.IdChofer ==  command.Chofer ? viaje.Chofer : await ValidarChofer(command.Chofer);

        var newDestinos = await ValidarDestinoProcencias(command.Destinos);
        var newProcedencias = await ValidarDestinoProcencias(command.Procendecias);
        
        viaje.IdCamion = camion.IdCamion;
        viaje.Chofer.IdChofer = chofer.IdChofer;
        viaje.Carga = command.Carga;
        viaje.Kilos = command.Kilos;
        viaje.Kilometros = command.Kilometros;
        viaje.MontoTotal = command.MontoTotal;
        //
        var destinos = viaje.Destinos.ToList();
        var destinoABorrar = destinos
            .Where(d => !command.Destinos.Contains(d.IdLocalidad))
            .ToList();
        
        if(destinoABorrar.Any()) _ctx.DestinoRepo.RemoveDestinos(destinoABorrar);

        foreach (var dest in newDestinos)
        {
            if(destinos.Exists(d => d.IdLocalidad == dest.IdLocalidad)) continue;

            var newDest = Destino.Create();
            newDest.Localidad = dest;
            newDest.Viaje = viaje;
            viaje.Destinos.Add(newDest);
        }
        //
        var procedencias = viaje.Procendecias.ToList();
        var procedenciasABorrar = procedencias
            .Where(d => !command.Procendecias.Contains(d.IdLocalidad))
            .ToList();
        
        if(procedenciasABorrar.Any()) _ctx.ProcedenciaRepo.RemoveProcedencias(procedenciasABorrar);

        foreach (var proc in newProcedencias)
        {
            if(procedencias.Exists(d => d.IdLocalidad == proc.IdLocalidad)) continue;

            var newProc = Procedencia.Create();
            newProc.Localidad = proc;
            newProc.Viaje = viaje;
            viaje.Procendecias.Add(newProc);
        }
        
        await _ctx.SaveChangesAsync();
    }
    
    private async Task<Camion> ValidarCamion(int idCamion)
    {
        var camion = await _ctx.CamionRepo.GetAsync( q => 
            q.AsNoTracking()
                .IncludeMantenimientoActual()
                .IncludeCurrentViaje()
                .WhereEqualsIdCamion(idCamion)
        );
        
        if (camion == null)
        {
            throw new InvalidOperationException($"El camion {idCamion} no existe");
        }

        if (camion.FechaBaja != null)
        {
            throw new InvalidOperationException("El camion esta dado de baja");
        }

        if (camion.Mantenimientos.Any())
        {
            throw new InvalidOperationException("El camion tiene un mantenimiento que no a terminado aun");
        }
        
        if(camion.Viajes.Any())
        {
            throw new InvalidOperationException("El camion se encuentra en un viaje");
        }

        return camion;
    }

    private async Task<Chofer> ValidarChofer(int idChofer)
    {
        var chofer = await _ctx.ChoferRepo.Query()
            .AsNoTracking()
            .ConViajesNoFinalizados()
            .FirstOrDefaultAsync(c => c.IdChofer == idChofer);

        if (chofer == null)
        {
            throw new InvalidOperationException($"El chofer numero {idChofer} no existe");
        }
        
        if (chofer.Viajes.Any())
        {
            throw new InvalidOperationException("El chofer tien viajes no finalizados");
        }

        if (chofer.FechaBaja != null)
        {
            throw new InvalidOperationException("El chofer se encuentra dado de baja");
        }

        return chofer;
    }

    private async Task<List<Localidad>> ValidarDestinoProcencias(List<int> idsLocalidades)
    {
        var localidades = await _ctx.UbicacionRepo
            .LocalidadQuery()
            .GetAllLocalidades(idsLocalidades)
            .ToListAsync();

        if (localidades.Count != idsLocalidades.Count)
        {
            throw new InvalidOperationException($"El destinos {idsLocalidades.First(d => !localidades.Exists(e => e.IdLocalidad == d))} no existe");
        }
        
        return localidades;
    }
}