using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Services.TallerServices.Get.Command;
using Seminario.Services.TallerServices.Get.Handler;
using Seminario.Services.TallerServices.Get.Response;
using Seminario.Services.TallerServices.GetAll.Command;
using Seminario.Services.TallerServices.GetAll.Handler;
using Seminario.Services.TallerServices.GetAll.Response;
using Seminario.Services.TallerServices.Upsert.Command;
using Seminario.Services.TallerServices.Upsert.Handler;

namespace Seminario.Api.Controllers.TallerController.v1;

[ApiController]
[Route("api/v1/taller")]
public class TallerController : ControllerBase
{
    private readonly IAppDbContext _ctx;
    
    public TallerController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<TalleresGetAllResponse>> GetAll([FromQuery] DataSourceRequest request,
        [FromQuery] TalleresGetAllCommand command, [FromServices] IDbSession session)
    {
        var handler = new TalleresGetAllHandler(session);
        var response = await handler.HandleAsync(command);
        return response.ToDataSourceResult(request);
    }

    [HttpPost("upsert")]
    [SeminarioResponse]
    public async Task Upsert([FromBody] TalleresUpsertCommand command)
    {
        var handler = new TalleresUpsertHandler(_ctx);
        await handler.HandleAsync(command);
    }
    
    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<TalleresGetResponse> Get([FromQuery] TalleresGetCommand command)
    {
        var handler = new TalleresGetHandler(_ctx);
        return await handler.HandleAsync(command);
    }
}