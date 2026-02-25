using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Entidades;
using Seminario.Services.EventosService.Upsert.Command;

namespace Seminario.Services.EventosService.Upsert.Handler;

public class EventosUpsertHandler
{
    private readonly IAppDbContext _ctx;
    
    public  EventosUpsertHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task HandleAsync(EventosUpsertCommand command)
    {
        var evento = await _ctx.EventoRepo.FindByIdAsync(command.IdEvento);

        if (evento == null)
        {
            evento = new Evento();
            _ctx.EventoRepo.Add(evento);
        }
        
        evento.Titulo = command.Titulo;
        evento.Descripcion = command.Descripcion;
        evento.FechaEvento = command.FechaEvento;
        evento.IdTipoEvento = command.IdTipoEvento;
        evento.Inactivo = command.Inactivo;
        await _ctx.SaveChangesAsync();
    }
}