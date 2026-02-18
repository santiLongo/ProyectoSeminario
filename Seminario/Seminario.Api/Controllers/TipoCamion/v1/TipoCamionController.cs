using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Services.TipoCamionCrud.Delete.Command;
using Seminario.Services.TipoCamionCrud.Delete.Handler;
using Seminario.Services.TipoCamionCrud.GetAll.Command;
using Seminario.Services.TipoCamionCrud.GetAll.Handler;
using Seminario.Services.TipoCamionCrud.GetAll.Model;
using Seminario.Services.TipoCamionCrud.Upsert.Command;
using Seminario.Services.TipoCamionCrud.Upsert.Handler;

namespace Seminario.Api.Controllers.TipoCamion.v1;

[Route("api/v1/tipo-camion")]
[ApiController]
public class TipoCamionController : ControllerBase
{
    private readonly IAppDbContext _ctx;

    public TipoCamionController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }
    
    [HttpGet]
    [Route("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<GetAllTipoCamionModel>> GetAll([FromQuery] GetAllTipoCamionCommand command,
        [FromQuery] DataSourceRequest request)
    {
        var handler = new GetAllTipoCamionHandler(_ctx);
        var response = await handler.Handle(command);
        return response.ToDataSourceResult(request);
    }
    
    [HttpPost]
    [Route("upsert")]
    [SeminarioResponse]
    public async Task Upsert([FromBody] UpsertTipoCamionCommand command)
    {
        var handler = new UpsertTipoCamionHandler(_ctx);
        await handler.Handle(command);
    }
    
    [HttpPost]
    [Route("delete")]
    [SeminarioResponse]
    public async Task Delete([FromBody] DeleteTipoCamionCommand command)
    {
        var handler = new DeleteTipoCamionHandler(_ctx);
        await handler.Handle(command);
    }
}