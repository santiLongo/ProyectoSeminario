using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.Dapper;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Services.ClientesServices.Commands.BajaAlta;
using Seminario.Services.ClientesServices.Commands.Get;
using Seminario.Services.ClientesServices.Commands.GetAll;
using Seminario.Services.ClientesServices.Commands.Upsert;
using Seminario.Services.ClientesServices.Models;

namespace Seminario.Api.Controllers.ClienteController.v1;

[ApiController]
[Route("api/v1/cliente")]
public class ClienteController : ControllerBase
{
    private readonly IAppDbContext _ctx;
    
    public ClienteController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<ClientesGetAllResponse>> GetAll([FromQuery] DataSourceRequest request,
        [FromServices] IDbExecutor executor, [FromQuery] ClientesGetAllCommand command)
    {
        var handler = new ClientesGetAllHandler(executor);
        var response = await handler.HandleAsync(command);
        return response.ToDataSourceResult(request);
    }
    
    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<ClienteFormModel> Get([FromQuery] int idCliente)
    {
        var handler = new ClienteGetHandler(_ctx);
        return await handler.HandleAsync(idCliente);
    }
    
    [HttpPost("upsert")]
    [SeminarioResponse]
    public async Task Upsert([FromBody] ClienteFormModel form)
    {
        var handler = new ClienteUpsertHandler(_ctx);
        await handler.HandleAsync(form);
    }
    
    [HttpPost("baja-alta")]
    [SeminarioResponse]
    public async Task BajaAlta([FromBody] int idCliente)
    {
        var handler = new ClienteBajaAltaHandler(_ctx);
        await handler.HandleAsync(idCliente);
    }
}