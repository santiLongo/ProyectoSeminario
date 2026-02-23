using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Services.ProveedorServices.Get.Command;
using Seminario.Services.ProveedorServices.Get.Handler;
using Seminario.Services.ProveedorServices.Get.Response;
using Seminario.Services.ProveedorServices.GetAll.Command;
using Seminario.Services.ProveedorServices.GetAll.Handler;
using Seminario.Services.ProveedorServices.GetAll.Response;
using Seminario.Services.ProveedorServices.Upsert.Command;
using Seminario.Services.ProveedorServices.Upsert.Handler;

namespace Seminario.Api.Controllers.ProveedorController.v1;

[ApiController]
[Route("api/v1/proveedor")]
public class ProveedorController : ControllerBase
{
    private readonly IAppDbContext _ctx;

    public ProveedorController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<ProveedoresGetAllResponse>> GetAll([FromQuery] DataSourceRequest request,
        [FromQuery] ProveedoresGetAllCommand command, [FromServices] IDbSession session)
    {
        var handler = new ProveedoresGetAllHandler(session);
        var response = await handler.HandleAsync(command);
        return response.ToDataSourceResult(request);
    }

    [HttpPost("upsert")]
    [SeminarioResponse]
    public async Task Upsert([FromBody] ProveedoresUpsertCommand command)
    {
        var handler = new ProveedoresUpsertHandler(_ctx);
        await handler.HandleAsync(command);
    }

    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<ProveedoresGetResponse> Get([FromQuery] ProveedoresGetCommand command)
    {
        var handler = new ProveedoresGetHandler(_ctx);
        return await handler.HandleAsync(command);
    }
}