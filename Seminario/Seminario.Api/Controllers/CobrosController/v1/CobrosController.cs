using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Services.CobrosServices.Add.Command;
using Seminario.Services.CobrosServices.Add.Handler;
using Seminario.Services.CobrosServices.Anular.Command;
using Seminario.Services.CobrosServices.Anular.Handler;
using Seminario.Services.CobrosServices.Get.Command;
using Seminario.Services.CobrosServices.Get.Handler;
using Seminario.Services.CobrosServices.Get.Response;
using Seminario.Services.CobrosServices.GetAll.Command;
using Seminario.Services.CobrosServices.GetAll.Handler;
using Seminario.Services.CobrosServices.GetAll.Response;
using Seminario.Services.CobrosServices.Update.Command;
using Seminario.Services.CobrosServices.Update.Handler;

namespace Seminario.Api.Controllers.CobrosController.v1;

[ApiController]
[Route("api/v1/cobros")]
public class CobrosController : ControllerBase
{
    private readonly IAppDbContext _ctx;

    public CobrosController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpPost("add")]
    [SeminarioResponse]
    public async Task Add([FromBody] AddCobroCommand command)
    {
        var handler = new AddCobroHandler(_ctx);
        await handler.Handle(command);
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<CobrosGetAllResponse>> GetAll([FromQuery] DataSourceRequest request,
        [FromQuery] CobrosGetAllCommand command, [FromServices] IDbSession session)
    {
        var handler = new CobrosGetAllHandler(session, _ctx);
        var response = await handler.HandleAsync(command);
        return response.ToDataSourceResult(request);
    }
    
    [HttpPost("update")]
    [SeminarioResponse]
    public async Task Update([FromBody] CobrosUpdateCommand command)
    {
        var handler = new CobrosUpdateHandler(_ctx);
        await handler.Handle(command);
    }

    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<CobrosGetResponse> Get([FromQuery] CobrosGetCommand command)
    {
        var handler = new CobrosGetHandler(_ctx);
        var response = await handler.HandleAsync(command);
        return response;
    }

    [HttpPost("anular")]
    [SeminarioResponse]
    public async Task Anular([FromBody] CobrosAnularCommand command)
    {
        var handler = new CobrosAnularHandler(_ctx);
        await handler.HandleAsync(command);
    }
}