using Microsoft.EntityFrameworkCore;
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
        
        viaje.FechaDescarga = command.FechaDescarga;
    }
}