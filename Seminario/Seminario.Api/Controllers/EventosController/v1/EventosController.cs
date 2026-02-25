using System.Net;
using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Api.Middleware.ExceptionMiddleware;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Datos.Entidades;
using Seminario.Services.EventosService.Get.Command;
using Seminario.Services.EventosService.Get.Handler;
using Seminario.Services.EventosService.Get.Response;
using Seminario.Services.EventosService.GetAll.Command;
using Seminario.Services.EventosService.GetAll.Handler;
using Seminario.Services.EventosService.GetAll.Response;
using Seminario.Services.EventosService.Upsert.Command;
using Seminario.Services.EventosService.Upsert.Handler;

namespace Seminario.Api.Controllers.EventosController.v1;


[ApiController]
[Route("api/v1/eventos")]
public class EventosController
{
    private readonly IAppDbContext _ctx;
    
    public EventosController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<IEnumerable<EventosGetAllResponse>> GetAll([FromQuery] EventosGetAllCommand command, 
        [FromServices] IDbSession session)
    {
        var handler = new EventosGetAllHandler(session);
        return await handler.HandleAsync(command);
    }
    
    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<EventosGetResponse> Get([FromQuery] EventosGetCommand command)
    {
        var handler = new EventosGetHandler((_ctx));
        return await handler.HandleAsync(command);
    }
    
    [HttpPost("upsert")]
    [SeminarioResponse]
    public async Task Upsert([FromBody] EventosUpsertCommand command)
    {
        var handler = new EventosUpsertHandler((_ctx));
        await handler.HandleAsync(command);
    }
    
    [HttpPost("activar")]
    [SeminarioResponse]
    public async Task Activar([FromBody] int idEvento)
    {
        var evento = await _ctx.EventoRepo.FindByIdAsync(idEvento);
        await _ctx.EventoRepo.Activar(evento);
    }
    
    [HttpPost("desactivar")]
    [SeminarioResponse]
    public async Task Desctivar([FromBody] int idEvento)
    {
        var evento = await _ctx.EventoRepo.FindByIdAsync(idEvento);
        await _ctx.EventoRepo.Desactivar(evento);
    }
    
    [HttpGet("getTipos")]
    [SeminarioResponse]
    public async Task<DataSourceResult<TipoEvento>> GetTipos([FromQuery] DataSourceRequest request)
    {
        var tipos = await _ctx.EventoRepo.GetAllTipos();
        return tipos.ToDataSourceResult(request);
    }
    
    [HttpPost("addTipo")]
    [SeminarioResponse]
    public async Task AddTipo([FromBody] TipoEvento command)
    {
        var tipo = await _ctx.EventoRepo.FindTipoByIdAsync(command.IdTipo);
        
        if(tipo != null) throw new SeminarioException("Ya existe es tipo", HttpStatusCode.Conflict);

        tipo = new TipoEvento
        {
            Descripcion =  command.Descripcion,
            Nombre =  command.Nombre
        };
        
        _ctx.EventoRepo.Add(tipo);
        await _ctx.SaveChangesAsync();
    }
}