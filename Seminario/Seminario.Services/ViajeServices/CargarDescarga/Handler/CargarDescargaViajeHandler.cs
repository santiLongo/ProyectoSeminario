using Microsoft.EntityFrameworkCore;
using Seminario.Datos;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.ViajeServices.CargarDescarga.Command;

namespace Seminario.Services.ViajeServices.CargarDescarga.Handler;

public class CargarDescargaViajeHandler
{
    private readonly IAppDbContext _ctx;

    public CargarDescargaViajeHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task Handle(CargarDescargaViajeCommand command)
    {
        var viaje = await _ctx.ViajeRepo.Query().FirstOrDefaultAsync(v => v.IdViaje == command.IdViaje);
        
        if (viaje == null) throw new InvalidOperationException("No se encontro el viaje");

        if (viaje.Estado > EstadosViaje.EnViaje.ToInt()) throw new InvalidOperationException("Este viaje ya esta finalizado o suspendido, no se puede cambiar la fecha de descarga");

        if (command.FechaDescarga.HasValue)
        {
            if (command.FechaDescarga.GetValueOrDefault() > DateTime.Now) 
                throw new InvalidOperationException("La fecha de descarga no puede ser superior al dia de hoy");
        }
        
        viaje.FechaDescarga = command.FechaDescarga ?? DateTime.Now;
        await _ctx.SaveChangesAsync();
    }
}