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
        if (command.Estado.ExisteEstadoViaje())
        {
            throw new InvalidOperationException("El estado no se encontro");
        }
        
        var viaje = await _ctx.ViajeRepo.Query()
            .FirstOrDefaultAsync(v => v.IdViaje == command.IdViaje);

        if (viaje == null)
        {
            throw new InvalidOperationException("El viaje no se encontro ");
        }

        if (viaje.Estado > EstadosViaje.Finalizado.ToInt() && !command.Seguridad)
        {
            throw new InvalidOperationException("El viaje posee un estado que requiere seguridad para modificar");
        }

        if (viaje.Estado == EstadosViaje.Cobrado.ToInt())
        {
            throw new InvalidOperationException("El viaje esta cobrado en su totalidad, no es forzable");
        }

        viaje.Estado = command.Estado;
        await _ctx.SaveChangesAsync();
    }
}