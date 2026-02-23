using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Services.Mantenimiento.Get.Command;
using Seminario.Services.Mantenimiento.Get.Handler;
using Seminario.Services.Mantenimiento.Get.Response;
using Seminario.Services.Mantenimiento.GetAll.Command;
using Seminario.Services.Mantenimiento.GetAll.Handler;
using Seminario.Services.Mantenimiento.GetAll.Response;
using Seminario.Services.Mantenimiento.InformarImporte.Command;
using Seminario.Services.Mantenimiento.InformarImporte.Handler;
using Seminario.Services.Mantenimiento.InformarSalida.Command;
using Seminario.Services.Mantenimiento.InformarSalida.Handler;
using Seminario.Services.Mantenimiento.Upsert.Command;
using Seminario.Services.Mantenimiento.Upsert.Handler;

namespace Seminario.Api.Controllers.MantenimientoController.v1;

[ApiController]
[Route("api/v1/mantenimiento")]
public class MantenimientoController : ControllerBase
{
    private readonly IAppDbContext _ctx;
    
    public  MantenimientoController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<MantenimientoGetAllResponse>> GetAll(
        [FromQuery] MantenimientoGetAllCommand command,
        [FromQuery] DataSourceRequest request, [FromServices] IDbSession session)
    {
        var handler = new MantenimientoGetAllHandler(session);
        var response = await handler.HandleAsync(command);
        return response.ToDataSourceResult(request);
    }
    
    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<MantenimientoGetResponse> Get([FromQuery] MantenimientoGetCommand command)
    {
        var handler = new MantenimientoGetHandler(_ctx);
        return await handler.HandleAsync(command);
    }

    [HttpPost("upsert")]
    [SeminarioResponse]
    public async Task Upsert([FromBody] MantenimientoUpsertCommand command)
    {
        var handler = new MantenimientoUpsertHandler(_ctx);
        await handler.HandleAsync(command);
    }
    
    [HttpPost("informar-salida")]
    [SeminarioResponse]
    public async Task InfomarSalida([FromBody] MantenimientoInformarSalidaCommand command)
    {
        var handler = new MantenimientoInformarSalidaHandler(_ctx);
        await handler.HandleAsync(command);
    }
    
    [HttpPost("informar-importe")]
    [SeminarioResponse]
    public async Task InfomarImporte([FromBody] MantenimientoInformarImporteCommand command)
    {
        var handler = new MantenimientoInformarImporteHandler(_ctx);
        await handler.Handle(command);
    }
}