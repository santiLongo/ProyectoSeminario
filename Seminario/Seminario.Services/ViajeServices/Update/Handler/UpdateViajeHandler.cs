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
        var viaje = _ctx.ViajeRepo.Query().WhereEnViaje().FirstOrDefaultAsync(v => v.IdViaje == command.IdViaje);

        if (viaje == null)
        {
            throw new InvalidOperationException($"El viaje no existe o se encuentra finalizado");
        }
    }
    
    private async Task<Camion> ValidarCamion(int idCamion)
    {
        var camion = await _ctx.CamionRepo.GetAsync( q => 
            q.IncludeMantenimientoActual()
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
            .ConNoViajesFinalizados()
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