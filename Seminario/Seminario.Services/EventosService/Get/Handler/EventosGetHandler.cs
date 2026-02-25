using System.Net;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Services.EventosService.Get.Command;
using Seminario.Services.EventosService.Get.Response;

namespace Seminario.Services.EventosService.Get.Handler;

public class EventosGetHandler
{
    private readonly IAppDbContext _ctx;
    
    public EventosGetHandler(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<EventosGetResponse> HandleAsync(EventosGetCommand command)
    {
        var evento = await _ctx.EventoRepo.FindByIdAsync(command.IdEvento);

        if (evento == null)
            throw new SeminarioException("No se encontro el evento", HttpStatusCode.NotFound);

        return new EventosGetResponse
        {
            IdEvento = evento.IdEvento,
            Titulo = evento.Titulo,
            Descripcion = evento.Descripcion,
            FechaEvento = evento.FechaEvento.GetValueOrDefault(),
            IdTipoEvento = evento.IdTipoEvento,
            Inactivo = evento.Inactivo,
        };
    }
}