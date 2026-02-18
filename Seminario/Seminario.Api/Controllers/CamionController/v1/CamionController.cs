using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Services.CamionCrud.DarDeAlta.Command;
using Seminario.Services.CamionCrud.DarDeAlta.Handler;
using Seminario.Services.CamionCrud.DarDeBaja.Command;
using Seminario.Services.CamionCrud.DarDeBaja.Handler;
using Seminario.Services.CamionCrud.Get.Command;
using Seminario.Services.CamionCrud.Get.Handler;
using Seminario.Services.CamionCrud.Get.Response;
using Seminario.Services.CamionCrud.GetAll.Command;
using Seminario.Services.CamionCrud.GetAll.Handler;
using Seminario.Services.CamionCrud.GetAll.Response;
using Seminario.Services.CamionCrud.Upsert.Command;
using Seminario.Services.CamionCrud.Upsert.Handler;

namespace Seminario.Api.Controllers.CamionController.v1;

[ApiController]
[Route("api/v1/camion")]
public class CamionController : ControllerBase
{
    private readonly IAppDbContext _ctx;

    public CamionController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    [HttpPost]
    [Route("upsert")]
    [SeminarioResponse]
    public async Task Upsert([FromBody] UpsertCamionCommand command)
    {
        var handler = new UpsertCamionHandler(_ctx);
        await handler.Handle(command);
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<CamionGetAllResponse>> GetAll([FromQuery] DataSourceRequest request,
        [FromQuery] CamionGetAllCommand command, [FromServices] IDbSession session)
    {
        var handler = new CamionGetAllHandler(session);
        var response = await handler.HandleAsync(command);
        return response.ToDataSourceResult(request);
    }
    
    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<CamionGetResponse> GetAll( [FromQuery] CamionGetCommand command)
    {
        var handler = new CamionGetHandler(_ctx);
        return await handler.HandleAsync(command);
    }
    
    [HttpPost("dar-baja")]
    [SeminarioResponse]
    public async Task DarBaja( [FromBody] CamionBajaCommand command)
    {
        var handler = new CamionBajaHandler(_ctx);
        await handler.HandleAsync(command);
    }
    
    [HttpPost("dar-alta")]
    [SeminarioResponse]
    public async Task DarAlta( [FromBody] CamionAltaCommand command)
    {
        var handler = new CamionAltaHandler(_ctx);
        await handler.HandleAsync(command);
    }
}