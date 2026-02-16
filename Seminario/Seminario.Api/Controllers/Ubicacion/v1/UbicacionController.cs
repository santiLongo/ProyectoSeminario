using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Services.Ubicacion.Delete.Command;
using Seminario.Services.Ubicacion.Delete.Handler;
using Seminario.Services.Ubicacion.Get.Command;
using Seminario.Services.Ubicacion.Get.Handler;
using Seminario.Services.Ubicacion.Get.Response;
using Seminario.Services.Ubicacion.GetAll.Command;
using Seminario.Services.Ubicacion.GetAll.Handler;
using Seminario.Services.Ubicacion.GetAll.Response;
using Seminario.Services.Ubicacion.Upsert.Command;
using Seminario.Services.Ubicacion.Upsert.Handler;

namespace Seminario.Api.Controllers.Ubicacion.v1;
[Route("api/v1/ubicacion")]
public class UbicacionController : ControllerBase
{
    private readonly IAppDbContext _ctx;
    
    public UbicacionController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<UbicacionGetAllResponse>> GetAll([FromQuery] DataSourceRequest request, 
        [FromQuery] UbicacionGetAllCommand command, [FromServices] IDbSession session)
    {
        var handler = new UbicacionGetAllHandler(session);
        var response = await handler.HandleAsync(command);
        return response.ToDataSourceResult<UbicacionGetAllResponse>(request);
    }
    
    [HttpPost]
    [Route("upsert")]
    [SeminarioResponse]
    public async Task Upsert([FromBody] UpsertUbicacionCommand command)
    {
        var handler = new UpsertUbicacionHandler(_ctx);
        await handler.Handle(command);
    }
    
    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<UbicacionGetResponse> Get([FromQuery] UbicacionGetCommand command,
        [FromServices] IDbSession session)
    {
        var handler = new UbicacionGetHandler(session);
        var response = await handler.HandleAsync(command);
        return response;
    }
    
    [HttpPost("delete")]
    [SeminarioResponse]
    public async Task Delete([FromBody] UbicacionDeleteCommand command)
    {
        var handler = new UbicacionDeleteHandler(_ctx);
        await handler.HandleAsync(command);
    }
}