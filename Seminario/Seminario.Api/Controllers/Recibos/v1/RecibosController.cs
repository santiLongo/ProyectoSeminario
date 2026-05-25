using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Services.RecibosServices.AddRecibo;
using Seminario.Services.RecibosServices.AnularRecibo;
using Seminario.Services.RecibosServices.GetRecibo;
using Seminario.Services.RecibosServices.GetAllRecibos;

namespace Seminario.Api.Controllers.Recibos.v1;

[ApiController]
[Route("api/v1/recibos")]
public class RecibosController : ControllerBase
{
    private readonly IAppDbContext _ctx;

    public RecibosController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpPost("add")]
    [SeminarioResponse]
    public async Task<int> Add([FromBody] AddReciboCommand command)
    {
        var handler = new AddReciboHandler(_ctx);
        return await handler.HandleAsync(command);
    }

    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<GetReciboResponse> Get([FromQuery] GetReciboCommand command)
    {
        var handler = new GetReciboHandler(_ctx);
        return await handler.HandleAsync(command);
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<GetAllRecibosResponse>> GetAll(
        [FromQuery] DataSourceRequest request,
        [FromQuery] GetAllRecibosCommand command)
    {
        var handler = new GetAllRecibosHandler(_ctx);
        var response = await handler.HandleAsync(command);
        return response.ToDataSourceResult(request);
    }

    [HttpPost("anular")]
    [SeminarioResponse]
    public async Task Anular([FromBody] AnularReciboCommand command)
    {
        var handler = new AnularReciboHandler(_ctx);
        await handler.HandleAsync(command);
    }
}
