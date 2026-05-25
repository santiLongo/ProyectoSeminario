using Microsoft.AspNetCore.Mvc;
using Seminario.Api.FilterResponse;
using Seminario.Datos.Contextos.AppDbContext;
using Seminario.Datos.DataSourceResult.Clases;
using Seminario.Datos.DataSourceResult.ExtesionMethods;
using Seminario.Datos.Entidades;
using Seminario.Services.FacturasServices.AnularFactura;
using Seminario.Services.FacturasServices.FacturasRecibidas.CrearRecibida;
using Seminario.Services.FacturasServices.GetAllFacturas;
using Seminario.Services.FacturasServices.GetFactura;

namespace Seminario.Api.Controllers.FacturasRecibidas.v1;

[ApiController]
[Route("api/v1/facturas-recibidas")]
public class FacturasRecibidasController : ControllerBase
{
    private readonly IAppDbContext _ctx;

    public FacturasRecibidasController(IAppDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpPost("add")]
    [SeminarioResponse]
    public async Task<int> Add([FromBody] CrearRecibidaCommand command)
    {
        var handler = new CrearRecibidaHandler(_ctx);
        return await handler.HandleAsync(command);
    }

    [HttpGet("getAll")]
    [SeminarioResponse]
    public async Task<DataSourceResult<GetAllFacturasResponse>> GetAll(
        [FromQuery] DataSourceRequest request,
        [FromQuery] GetAllFacturasCommand command)
    {
        command.TipoFactura = (int)Factura.TipoFactura.Recibida;
        var handler = new GetAllFacturasHandler(_ctx);
        var response = await handler.HandleAsync(command);
        return response.ToDataSourceResult(request);
    }

    [HttpGet("get")]
    [SeminarioResponse]
    public async Task<GetFacturaResponse> Get([FromQuery] GetFacturaCommand command)
    {
        var handler = new GetFacturaHandler(_ctx);
        return await handler.HandleAsync(command);
    }

    [HttpPost("anular")]
    [SeminarioResponse]
    public async Task Anular([FromBody] AnularFacturaCommand command)
    {
        var handler = new AnularFacturaHandler(_ctx);
        await handler.HandleAsync(command);
    }
}
