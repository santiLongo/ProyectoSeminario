using Microsoft.EntityFrameworkCore;
using Seminario.Datos;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Datos.Repositorios;
using Seminario.Services.ViajeServices.Add.Command;

namespace Seminario.Services.ViajeServices.Add.Handler;

public class AddViajeHandler
{
    private readonly IAppDbContext _ctx;
    
    public AddViajeHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(AddViajeCommand command)
    {
        var viaje = Viaje.Create();
        //
        viaje.Camion = await ValidarCamion(command.Camion.GetValueOrDefault());
        viaje.IdCamion = viaje.Camion.IdCamion;
        //
        viaje.Chofer = await ValidarChofer(command.Chofer.GetValueOrDefault());
        viaje.IdChofer = viaje.Chofer.IdChofer;
	    //
        var cliente = await _ctx.ClienteRepo.FindByIdAsync(command.Cliente.GetValueOrDefault());
        
        if (cliente == null)
        {
            throw new InvalidOperationException("El cliente informado no existe");
        }

        viaje.Cliente = cliente;
        viaje.IdCliente = cliente.IdCliente;
        //
        var destinos = await ValidarDestinoProcencias(command!.Destinos);
        var procedencias = await ValidarDestinoProcencias(command!.Procendecias);
        //
        viaje.Carga = command.Carga;
        viaje.Estado = EstadosViaje.EnViaje.ToInt();
        viaje.Kilometros =  command.Kilometros;
        viaje.Kilos = command.Kilos;
        viaje.IdMoneda = command.IdMoneda;
        viaje.MontoTotal = command.MontoTotal.GetValueOrDefault();
        viaje.PrecioKm = (float)(viaje.MontoTotal / viaje.Kilometros)!;
        viaje.FechaAlta = DateTime.Today;
        viaje.FechaPartida = command.FechaPartida.GetValueOrDefault();
        //
        foreach (var dest in destinos)
        {
            var destino = Destino.Create();
            destino.IdLocalidad = dest.IdLocalidad;
            destino.Localidad = dest;
            destino.IdViaje =  viaje.IdViaje;
            destino.Viaje = viaje;
            viaje.Destinos.Add(destino);
        }
        
        foreach (var proc in procedencias)
        {
            var procedencia = Procedencia.Create();
            procedencia.IdLocalidad = proc.IdLocalidad;
            procedencia.Localidad = proc;
            procedencia.IdViaje =  viaje.IdViaje;
            procedencia.Viaje = viaje;
            viaje.Procendecias.Add(procedencia);
        }
        //
        
        _ctx.ViajeRepo.Add(viaje);
        await _ctx.SaveChangesAsync();
        
        viaje.NroViaje = $"V-{viaje.IdViaje}";

        await _ctx.SaveChangesAsync();
    }

    private async Task<Camion> ValidarCamion(int idCamion)
    {
        var camion = await _ctx.CamionRepo.GetAsync( q => 
                q .IncludeMantenimientoActual()
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